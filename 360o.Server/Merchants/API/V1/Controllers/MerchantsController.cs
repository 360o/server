﻿using System;
using _360o.Server.Merchants.API.V1.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Npgsql;

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

            try
            {
                await _merchantsContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                // See https://www.postgresql.org/docs/current/errcodes-appendix.html
                if (e.InnerException is PostgresException innerException && innerException.SqlState == "23505")
                {
                    return Conflict(new ErrorDTO
                    {
                        Code = ErrorCode.NameAlreadyExists,
                        Message = e.InnerException.Message
                    });
                }

                throw;
            }

            return CreatedAtAction(nameof(GetMerchantByIdAsync), new { id = merchant.Id }, _mapper.Map<MerchantDTO>(merchant));
        }

        [HttpGet]
        public async Task<IEnumerable<MerchantDTO>> ListMerchantsAsync([FromQuery] ListMerchantsRequest request)
        {
            var merchants = _merchantsContext.Merchants.AsQueryable();

            if (request.Latitude.HasValue && request.Longitude.HasValue && request.Radius.HasValue)
            {
                var locationPoint = new Point(request.Longitude.Value, request.Latitude.Value);
                merchants = merchants.Include(m => m.Places.Where(p => p.Point.Distance(locationPoint) <= request.Radius.Value));
            }
            else
            {
                merchants = merchants.Include(m => m.Places);
            }

            return await merchants.Select(m => _mapper.Map<MerchantDTO>(m)).ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Merchant))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MerchantDTO>> GetMerchantByIdAsync(Guid id)
        {
            var merchant = await _merchantsContext.Merchants.Include(m => m.Places).SingleOrDefaultAsync(m => m.Id == id);

            if (merchant == null)
            {
                return NotFound(new ErrorDTO
                {
                    Code = ErrorCode.ItemNotFound,
                    Message = $"Merchant not found"
                });
            }

            return _mapper.Map<MerchantDTO>(merchant);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMerchantByIdAsync(Guid id)
        {
            var merchant = await _merchantsContext.Merchants.FindAsync(id);

            if (merchant == null)
            {
                return NotFound(new ErrorDTO
                {
                    Code = ErrorCode.ItemNotFound,
                    Message = $"Merchant not found"
                });
            }

            _merchantsContext.Remove(merchant);

            await _merchantsContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

