using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viato.Api.Models;
using Viato.Api.Tor;

namespace Viato.Api.Controllers
{
    [ApiController]
    [Route("pipelines")]
    public class PipelinesController : Controller
    {
        private readonly ViatoContext _dbContext;

        public PipelinesController(ViatoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{pipelineId}/tor")]
        [ProducesResponseType(200, Type = typeof(List<TorModel>))]
        public async Task<IActionResult> Get([FromRoute] long pipelineId, [FromQuery] decimal amount, [FromQuery] int count = 1)
        {
            if (amount < 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount should be greater than 0");
                return BadRequest();
            }

            var pipline = await _dbContext.ContributionPipelines.FindAsync(pipelineId);
            if (pipline == null)
            {
                return NotFound();
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
    }
}
