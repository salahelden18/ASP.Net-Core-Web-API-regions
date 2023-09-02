using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _region;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository region, IMapper mapper, ILogger<RegionsController> logger) 
        {
            _dbContext = dbContext;
            _region = region;
            _mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() 
        {
            logger.LogInformation("GetAll Action Method was invoked");
            var regions = await _region.GetAllAsync();

            var regionsDto = _mapper.Map<List<RegionDto>>(regions);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id) 
        {
            var region = await _region.GetByIdAsync(id);

            if(region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            
                regionDomainModel = await _region.CreateAsync(regionDomainModel);

                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await _region.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel  == null) { return NotFound(); }

            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var regionDomainModel =  await _region.DeleteAsync(id);

            if (regionDomainModel == null) 
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
