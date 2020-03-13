using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viato.Api.Auth;
using Viato.Api.Entities;
using Viato.Api.Models;

namespace Viato.Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase
    {
        private readonly ViatoContext _dbContext;
        private readonly IMapper _mapper;

        public PostsController(ViatoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]long id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.Status == PostStatus.Draft && (!User.TryGetUserId(out long userId) || userId != post.AuthorOrganization.AppUserId))
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostModel>(post));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateOrUpdatePostModel model)
        {
            if (!model.ContributionPipelineId.HasValue)
            {
                ModelState.AddModelError(nameof(model.ContributionPipelineId), "ContributionPipelineId is required.");
                return BadRequest(ModelState);
            }

            var pipeline = await _dbContext.ContributionPipelines.FindAsync(model.ContributionPipelineId.Value);
            if (pipeline == null)
            {
                return NotFound();
            }

            var userId = User.GetUserId();
            if (pipeline.DestinationOrganization.AppUserId != userId || pipeline.SourceOrganizaton.AppUserId != userId)
            {
                return StatusCode(401);
            }

            var post = new Post()
            {
                AuthorOrganizationId = pipeline.DestinationOrganization.AppUserId == userId ? pipeline.DestinationOrganizationId : pipeline.SourceOrganizationId,
                ContributionPipelineId = pipeline.Id,
                Title = model.Title,
                Status = model.Status,
                Body = model.Body,
            };

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            return Ok(post);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]long id, [FromBody]CreateOrUpdatePostModel model)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Body = model.Body;
            post.Status = model.Status;
            post.Title = model.Title;

            await _dbContext.SaveChangesAsync();

            return Ok(post);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]long id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.AuthorOrganization.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            _dbContext.Posts.Remove(post);

            return Ok(post);
        }
    }
}
