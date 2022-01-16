using System;
using _360o.Server.Merchants.API.V1.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _360o.Server.Merchants.API.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly MerchantsContext _merchantsContext;
        private readonly IMapper _mapper;

        public MerchantsController(MerchantsContext merchantsContext, IMapper mapper)
        {
            _merchantsContext = merchantsContext;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<MerchantDTO>> CreateMerchantAsync([FromBody] CreateMerchantRequest request)
        {
            var merchant = new Merchant(User.Identity.Name, request.DisplayName);

            foreach (var place in request.Places)
            {
                merchant.AddPlace(_mapper.Map<Place>(place));
            }

            _merchantsContext.Add(merchant);

            await _merchantsContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMerchantByIdAsync), new { id = merchant.Id }, _mapper.Map<MerchantDTO>(merchant));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Merchant))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MerchantDTO>> GetMerchantByIdAsync(Guid id)
        {
            var merchant = await _merchantsContext.Merchants.Include(m => m.Places).SingleOrDefaultAsync(m => m.Id == id);

            if (merchant == null)
            {
                return NotFound();
            }

            return _mapper.Map<MerchantDTO>(merchant);
        }
    }
}

