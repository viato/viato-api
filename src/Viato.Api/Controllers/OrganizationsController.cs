using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viato.Api.Auth;
using Viato.Api.Entities;
using Viato.Api.Models;
using Viato.Api.Services;

namespace Viato.Api.Controllers
{
    [ApiController]
    [Route("organizations")]
    public class OrganizationsController : Controller
    {
        private readonly ViatoContext _dbContext;
        private readonly IDnsProofService _dnsService;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        private readonly int _maxPageSize = 50;
        private readonly int _maxLogoSizeInMb = 5;
        private readonly string[] _allowedLogoExtensions = new string[] { ".jpg", ".png" };

        public OrganizationsController(
            ViatoContext dbContext,
            IDnsProofService dnsService,
            IBlobService blobService,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dnsService = dnsService ?? throw new ArgumentNullException(nameof(dnsService));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
            take = take > _maxPageSize ? _maxPageSize : take;
            var organizations = await _dbContext.Organizations.Skip(skip).Take(take).ToListAsync();
            return Ok(_mapper.Map<List<OrganizationModel>>(organizations));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]long id)
        {
            var organization = await _dbContext.Organizations.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }

        [Authorize]
        [HttpGet("my")]
        public IActionResult GetMy([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
            take = take > _maxPageSize ? _maxPageSize : take;

            var userId = User.GetUserId();
            var contributions = _dbContext.Organizations
                .Where(c => c.AppUserId == userId)
                .Skip(skip)
                .Take(take);

            return Ok(contributions);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateOrUpdateOrganizationModel model)
        {
            if (!model.OrganizationType.HasValue)
            {
                ModelState.AddModelError(nameof(model.OrganizationType), "OrganizationType is required.");
                return BadRequest(ModelState);
            }

            var websiteUri = new Uri(model.Website);
            if (await OrganizationWithSameWebsiteExistsAsync(websiteUri))
            {
                ModelState.AddModelError(nameof(model.Website), "Website is already used for other organization.");
                return BadRequest(ModelState);
            }

            var organization = new Organization()
            {
                AppUserId = User.GetUserId(),
                Descripiton = model.Descripiton,
                Name = model.Name,
                Type = model.OrganizationType.Value,
                Status = OrganizationStatus.NotVerified,
                Website = model.Website.ToString(),
                Domain = websiteUri.DnsSafeHost,
            };

            _dbContext.Organizations.Add(organization);

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]long id, [FromBody]CreateOrUpdateOrganizationModel model)
        {
            var organization = await _dbContext.Organizations.FindAsync(id);

            if (organization == null || organization.Status == OrganizationStatus.Suspended)
            {
                return NotFound();
            }

            if (organization.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            organization.Name = model.Name;
            organization.Descripiton = model.Descripiton;

            if (new Uri(organization.Website) != new Uri(model.Website))
            {
                var websiteUri = new Uri(model.Website);
                if (await OrganizationWithSameWebsiteExistsAsync(websiteUri))
                {
                    ModelState.AddModelError(nameof(model.Website), "Website is already used for other organization.");
                    return BadRequest(ModelState);
                }

                organization.Website = model.Website.ToString();
                organization.Domain = websiteUri.DnsSafeHost;
                organization.Status = OrganizationStatus.NotVerified;
            }

            if (model.OrganizationType != null && !organization.ContributionPiplines.Any())
            {
                organization.Type = model.OrganizationType.Value;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }

        [Authorize]
        [HttpPut("{id}/verify-dns")]
        public async Task<IActionResult> VerifyDnsAsync([FromRoute]long id)
        {
            var organization = await _dbContext.Organizations.FindAsync(id);
            if (organization == null || organization.Status == OrganizationStatus.Suspended)
            {
                return NotFound();
            }

            if (organization.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            var isVerified = await _dnsService.VerifyProofAsync(organization);

            if (isVerified)
            {
                organization.Status = OrganizationStatus.Verified;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }

        [Authorize]
        [HttpPut("{id}/update-logo")]
        public async Task<IActionResult> UpdateLogoAsync([FromRoute]long id, [FromForm]IFormFile file)
        {
            if (file == null)
            {
                ModelState.AddModelError("File", $"No file found in the request.");
                return BadRequest(ModelState);
            }

            var organization = await _dbContext.Organizations.FindAsync(id);
            if (organization == null || organization.Status == OrganizationStatus.Suspended)
            {
                return NotFound();
            }

            if (organization.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (!_allowedLogoExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", $"Only {string.Join(",", _allowedLogoExtensions)} images are allowed.");
                return BadRequest(ModelState);
            }

            if (file.Length / 1024 / 1024 > _maxLogoSizeInMb)
            {
                ModelState.AddModelError("File", $"Max logo upload size is {_maxLogoSizeInMb} mb.");
                return BadRequest(ModelState);
            }

            var logoUri = await _blobService.UploadOrganizationLogoAsync(organization, file.OpenReadStream(), fileExtension);

            organization.LogoBlobUri = logoUri.AbsoluteUri;
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]long id)
        {
            var organization = await _dbContext.Organizations.FindAsync(id);
            if (organization == null || organization.Status == OrganizationStatus.Suspended)
            {
                return NotFound();
            }

            if (organization.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            var pipelines = await _dbContext.ContributionPipelines
                .Where(p => p.SourceOrganizationId == organization.Id || p.DestinationOrganizationId == organization.Id)
                .ToListAsync();

            if (!pipelines.Any())
            {
                _dbContext.Organizations.Remove(organization);
                await _dbContext.SaveChangesAsync();
            }
            else if (pipelines.SelectMany(p => p.Contributions).Count() > 0)
            {
                organization.Status = OrganizationStatus.Suspended;
                await _dbContext.SaveChangesAsync();
            }

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }

        private async Task<bool> OrganizationWithSameWebsiteExistsAsync(Uri website)
        {
            return await _dbContext.Organizations.AnyAsync(s => s.Domain == website.DnsSafeHost);
        }
    }
}
