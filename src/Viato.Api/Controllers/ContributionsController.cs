using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viato.Api.Auth;
using Viato.Api.Entities;
using Viato.Api.Misc;
using Viato.Api.Models;
using Viato.Api.Tor;

namespace Viato.Api.Controllers
{
    [ApiController]
    [Route("contributions")]
    public class ContributionsController : Controller
    {
        private readonly ViatoContext _dbContext;
        private readonly IMapper _mapper;

        public ContributionsController(ViatoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
            take = take > Constants.MaxPageSize ? Constants.MaxPageSize : take;

            var contributions = await _dbContext.Contributions
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(_mapper.Map<List<ContributionModel>>(contributions));
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAsync([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
            take = take > Constants.MaxPageSize ? Constants.MaxPageSize : take;

            var userId = User.GetUserId();
            var contributions = await _dbContext.Contributions
                .Where(c => c.ContributorId == userId)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(_mapper.Map<List<ContributionModel>>(contributions));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]long id)
        {
            var contribution = await _dbContext.Contributions.FindAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ContributionModel>(contribution));
        }

        [HttpGet("scan-tor")]
        public async Task<IActionResult> ScanAsync([FromQuery]string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError(nameof(token), "Tor token is required.");
                return BadRequest(ModelState);
            }

            if (!TorToken.TryParse(token, out TorToken torToken))
            {
                ModelState.AddModelError(nameof(token), "Tor token is in invalid format.");
                return BadRequest(ModelState);
            }

            var pipeline = await _dbContext.ContributionPipelines.FindAsync(torToken.PipelineId);
            if (pipeline == null)
            {
                return StatusCode(AppHttpErrors.TorPipelineNotFound);
            }

            if (pipeline.Status != ContributionPipelineStatus.Active)
            {
                return StatusCode(AppHttpErrors.TorPipelineIsNotAcitve);
            }

            var contribution = new Contribution()
            {
                Amount = torToken.Amount,
                ContributionPipelineId = pipeline.Id,
                TorTokenId = torToken.Id.ToString(),
                TorToken = token,
            };

            StagedContribution stagedContribution = null;

            if (User.TryGetUserId(out long userId))
            {
                contribution.ContributorId = userId;
            }
            else
            {
                stagedContribution = new StagedContribution()
                {
                    ContributionId = contribution.Id,
                };
                _dbContext.StagedContributions.Add(stagedContribution);
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new ScanTorResultModel()
            {
                Contribution = _mapper.Map<ContributionModel>(contribution),
                StagedContributionId = stagedContribution?.Id.ToString(),
            });
        }
    }
}
