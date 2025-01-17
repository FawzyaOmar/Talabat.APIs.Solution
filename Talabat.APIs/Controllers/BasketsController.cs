﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
   
    public class BasketsController : APIBsaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId) {

            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            return Basket is null ? new CustomerBasket(BasketId) : Ok(Basket);
        
        
        }
        [HttpPost]

        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto Basket) {

            var MappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(Basket);
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);

            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));

            return Ok(CreatedOrUpdatedBasket);


        }
        [HttpDelete]

        public async Task<ActionResult<bool>> DeleteBasket(string BasketId) {

            return await _basketRepository.DeleteBasketAsync(BasketId);
        
        
        }



    }
}
