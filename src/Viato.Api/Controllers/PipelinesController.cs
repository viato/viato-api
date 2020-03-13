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
    [Route("pipelines")]
    public class PipelinesController : Controller
    {
        private readonly ViatoContext _dbContext;
        private readonly IMapper _mapper;

        public PipelinesController(ViatoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery]int skip = 0,
            [FromQuery]int take = 10,
            [FromQuery]OrderType order = OrderType.Newest)
        {
            take = take > Constants.MaxPageSize ? Constants.MaxPageSize : take;
            IQueryable<ContributionPipeline> pipelinesQuery = _dbContext.ContributionPipelines;

            switch (order)
            {
                case OrderType.Newest:
                    pipelinesQuery = pipelinesQuery.OrderByDescending(x => x.Id);
                    break;
                case OrderType.Popular:
                    pipelinesQuery = pipelinesQuery.OrderByDescending(x => x.Contributions.Count);
                    break;
            }

            var pipelines = await pipelinesQuery
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(_mapper.Map<List<PipelineModel>>(pipelines));
        }

        [Authorize]
        [HttpGet("my")]
        public IActionResult GetMy([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
            take = take > Constants.MaxPageSize ? Constants.MaxPageSize : take;

            var userId = User.GetUserId();
            var pipelines = _dbContext.ContributionPipelines
                .Where(c => c.SourceOrganizaton.AppUserId == userId || c.DestinationOrganization.AppUserId == userId)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take);

            return Ok(_mapper.Map<List<PipelineModel>>(pipelines));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]long id)
        {
            var pipeline = await _dbContext.ContributionPipelines.FindAsync(id);
            if (pipeline == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PipelineModel>(pipeline));
        }

        [HttpGet("{id}/contributions")]
        public async Task<IActionResult> GetContributionsAsync(
            [FromRoute]long id,
            [FromQuery]int skip = 0,
            [FromQuery]int take = 10)
        {
            var pipeline = await _dbContext.ContributionPipelines.FindAsync(id);
            if (pipeline == null)
            {
                return NotFound();
            }

            var contributions = pipeline.Contributions
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToList();

            return Ok(_mapper.Map<List<ContributionModel>>(contributions));
        }

        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetAllAsync(
            [FromRoute]long id,
            [FromQuery]int skip = 0,
            [FromQuery]int take = 10)
        {
            take = take > Constants.MaxPageSize ? Constants.MaxPageSize : take;

            var pipeline = await _dbContext.ContributionPipelines.FindAsync(id);
            if (pipeline == null)
            {
                return NotFound();
            }

            IEnumerable<Post> posts = pipeline.Posts;

            if (User.TryGetUserId(out long userId))
            {
                posts = posts.Where(p => p.AuthorOrganization.AppUserId == userId || p.Status == PostStatus.Published);
            }

            posts = posts.OrderByDescending(p => p.Id).Skip(skip).Take(take);

            return Ok(_mapper.Map<List<PostModel>>(posts));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreatePipelineModel model)
        {
            if (!await ValidateCreateModelAsync(model))
            {
                return BadRequest(ModelState);
            }

            var ecKey = new ECKey();
            var pipeline = new ContributionPipeline()
            {
                AmountLimit = model.AmountLimit ?? 0,
                CollectedAmount = 0,
                ContributionCurrency = model.ContributionCurrency,
                DateLimit = model.DateLimit,
                Description = model.Description,
                DestinationOrganizationId = model.DestinationOrganizationId,
                DisplayName = model.DisplayName,
                SourceOrganizationId = model.SourceOrganizationId,
                Status = model.Status,
                Types = model.Types,
                OwnerPrivateKey = ecKey.GetPrivateKey().ToHex(), // TODO remove this (generate key in frontend)
                OwnerPublicKey = ecKey.GetPublicKey().ToHex(), // TODO receive pubkey from frontend
            };

            _dbContext.ContributionPipelines.Add(pipeline);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<PipelineModel>(pipeline));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]long id, [FromBody]UpdatePipelineModel model)
        {
            var pipeline = await _dbContext.ContributionPipelines.FindAsync(id);
            if (pipeline == null)
            {
                return NotFound();
            }

            if (pipeline.SourceOrganizaton.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            pipeline.DisplayName = model.DisplayName;
            pipeline.Description = model.Description;

            if (model.Status.HasValue)
            {
                pipeline.Status = model.Status.Value;
            }

            if (pipeline.Types.HasFlag(ContributionPipelineTypes.LimitByAmount) && model.AmountLimit != null)
            {
                pipeline.AmountLimit = model.AmountLimit.Value;
            }

            if (pipeline.Types.HasFlag(ContributionPipelineTypes.LimitByDate) && model.DateLimit != null)
            {
                pipeline.DateLimit = model.DateLimit;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<PipelineModel>(pipeline));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]long id)
        {
            var pipeline = await _dbContext.ContributionPipelines.FindAsync(id);
            if (pipeline == null || pipeline.Status == ContributionPipelineStatus.Suspended)
            {
                return NotFound();
            }

            if (pipeline.SourceOrganizaton.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            if (!_dbContext.Contributions.Any())
            {
                _dbContext.ContributionPipelines.Remove(pipeline);
            }
            else
            {
                pipeline.Status = ContributionPipelineStatus.Suspended;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<PipelineModel>(pipeline));
        }

        [Authorize]
        [HttpGet("{id}/tor")]
        [ProducesResponseType(200, Type = typeof(List<TorModel>))]
        public async Task<IActionResult> GenerateTorsAsync([FromRoute]long id, [FromQuery]decimal amount, [FromQuery]int count = 1)
        {
            if (amount < 0)
            {
                ModelState.AddModelError(nameof(amount), "Contribution amount should be greater than 0.");
                return BadRequest(ModelState);
            }

            if (count <= 0)
            {
                ModelState.AddModelError(nameof(count), "Tor generation count should be greater than 0.");
                return BadRequest(ModelState);
            }

            var pipline = await _dbContext.ContributionPipelines.FindAsync(id);
            if (pipline == null)
            {
                return NotFound();
            }

            if (pipline.SourceOrganizaton.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            if (pipline.SourceOrganizaton.Status != OrganizationStatus.Verified)
            {
                ModelState.AddModelError(string.Empty, "Organization should be verified in order to generate tors for pipeline.");
                return BadRequest(ModelState);
            }

            var torList = new List<TorModel>();
            for (int i = 1; i <= count; i++)
            {
                var tor = new TorToken(
                    Guid.NewGuid(),
                    pipline.SourceOrganizationId,
                    pipline.DestinationOrganizationId,
                    amount);

                var torToken = tor.Protect(pipline.OwnerPrivateKey.HexToByteArray());
                torList.Add(new TorModel()
                {
                    Id = tor.Id,
                    Token = torToken,
                });
            }

            return Ok(torList);
        }

        private async Task<bool> ValidateCreateModelAsync(CreatePipelineModel model)
        {
            if (model.Types.HasFlag(ContributionPipelineTypes.LimitByDate) && model.DateLimit <= DateTimeOffset.UtcNow)
            {
                ModelState.AddModelError(nameof(model.DateLimit), "Pipeline with date limit should have date limit greater than today.");
                return false;
            }

            if (model.Types.HasFlag(ContributionPipelineTypes.LimitByAmount) && (model.AmountLimit == null || model.AmountLimit <= 0))
            {
                ModelState.AddModelError(nameof(model.AmountLimit), "Pipeline with amount limit should have amount limit greater than 0.");
                return false;
            }

            var srcOrg = await _dbContext.Organizations.FindAsync(model.SourceOrganizationId);
            if (srcOrg.AppUserId != User.GetUserId())
            {
                ModelState.AddModelError(nameof(model.SourceOrganizationId), "Can't create pipeline for organization not owned by you.");
                return false;
            }

            if (srcOrg.Type != OrganizationType.Source)
            {
                ModelState.AddModelError(nameof(model.SourceOrganizationId), "Can't create pipeline from not source organization.");
                return false;
            }

            var dstOrg = await _dbContext.Organizations.FindAsync(model.DestinationOrganizationId);
            if (dstOrg.Type != OrganizationType.Destination)
            {
                ModelState.AddModelError(nameof(model.DestinationOrganizationId), "Can't create pipeline to not destination organization.");
                return false;
            }

            return true;
        }
    }
}
