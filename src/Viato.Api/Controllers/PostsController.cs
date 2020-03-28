using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viato.Api.Auth;
using Viato.Api.Entities;
using Viato.Api.Misc;
using Viato.Api.Models;
using Viato.Api.Services;

namespace Viato.Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase
    {
        private readonly ViatoContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public PostsController(
            ViatoContext dbContext,
            UserManager<AppUser> userManager,
            IBlobService blobService,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        [Produces(typeof(PostModel))]
        public async Task<IActionResult> GetAsync([FromRoute]long id)
        {
            var post = await _dbContext.Posts
                .Include(x => x.AuthorOrganization)
                .SingleOrDefaultAsync(x => x.Id == id);

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
        [Produces(typeof(Post))]
        public async Task<IActionResult> CreateAsync([FromBody]CreateOrUpdatePostModel model)
        {
            if (!model.ContributionPipelineId.HasValue)
            {
                ModelState.AddModelError(nameof(model.ContributionPipelineId), "ContributionPipelineId is required.");
                return BadRequest(ModelState);
            }

            var pipeline = await _dbContext.ContributionPipelines
                .Include(x => x.SourceOrganizaton)
                .Include(x => x.DestinationOrganization)
                .SingleOrDefaultAsync(x => x.Id == model.ContributionPipelineId.Value);

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

            return Ok(_mapper.Map<PostModel>(post));
        }

        [Authorize]
        [HttpPut("{id}")]
        [Produces(typeof(Post))]
        public async Task<IActionResult> UpdateAsync([FromRoute]long id, [FromBody]CreateOrUpdatePostModel model)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userOrgs = await _dbContext.Organizations.Where(x => x.AppUserId == user.Id).ToListAsync();
            if (!userOrgs.Any(o => o.Id == post.AuthorOrganizationId))
            {
                return StatusCode(401);
            }

            post.Body = model.Body;
            post.Status = model.Status;
            post.Title = model.Title;

            await _dbContext.SaveChangesAsync();

            _mapper.Map<PostModel>(post);

            return Ok(post);
        }

        [Authorize]
        [HttpPut("{id}/update-image")]
        [Produces(typeof(PostModel))]
        public async Task<IActionResult> UpdateLogoAsync([FromRoute]long id, [FromForm]IFormFile file)
        {
            if (file == null)
            {
                ModelState.AddModelError("File", $"No file found in the request.");
                return BadRequest(ModelState);
            }

            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userOrgs = await _dbContext.Organizations.Where(x => x.AppUserId == user.Id).ToListAsync();
            if (!userOrgs.Any(o => o.Id == post.AuthorOrganizationId))
            {
                return StatusCode(401);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (!Constants.AllowedImageExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", $"Only {string.Join(",", Constants.AllowedImageExtensions)} images are allowed.");
                return BadRequest(ModelState);
            }

            if (file.Length / 1024 / 1024 > Constants.MaxImageSizeInMb)
            {
                ModelState.AddModelError("File", $"Max image upload size is {Constants.MaxImageSizeInMb} mb.");
                return BadRequest(ModelState);
            }

            var imageUir = await _blobService.UploadPostCoverImageAsync(post, file.OpenReadStream(), fileExtension);

            post.ImageBlobUri = imageUir.AbsoluteUri;
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<PostModel>(post));
        }

        [Authorize]
        [HttpDelete("{id}")]
        [Produces(typeof(PostModel))]
        public async Task<IActionResult> DeleteAsync([FromRoute]long id)
        {
            var post = await _dbContext.Posts
                .Include(x => x.AuthorOrganization)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.AuthorOrganization.AppUserId != User.GetUserId())
            {
                return NotFound();
            }

            _dbContext.Posts.Remove(post);

            return Ok(_mapper.Map<PostModel>(post));
        }
    }
}
