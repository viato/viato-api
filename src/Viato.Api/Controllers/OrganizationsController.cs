using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viato.Api.Models;

namespace Viato.Api.Controllers
{
    [Route("organizations")]
    public class OrganizationsController : ControllerBase
    {
        private readonly ViatoContext _dbContext;
        private readonly IMapper _mapper;
        private readonly int _defaultPageSise = 10;

        public OrganizationsController(
            ViatoContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery(Name = "p")]uint page = 1)
        {
            var organizations = await _dbContext.Organizations.Skip((int)page * _defaultPageSise).ToListAsync();
            return Ok(_mapper.Map<List<OrganizationModel>>(organizations));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllAsync([FromRoute]long id)
        {
            var organization = await _dbContext.Organizations.FindAsync(id);

            if (organization == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<OrganizationModel>(organization));
        }
    }
}
