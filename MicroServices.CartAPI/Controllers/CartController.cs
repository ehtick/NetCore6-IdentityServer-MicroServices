﻿using MicroServices.CartAPI.Data.ValueObjects;
using MicroServices.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> FindById(string id)
        {
            CartVO cart = await _repository.FindCartByUserIdAsync(id);
            if (cart is null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
        {
            CartVO cart = await _repository.SaveOrUpdateCartAsync(vo);
            if (cart is null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            CartVO cart = await _repository.SaveOrUpdateCartAsync(vo);
            if (cart is null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            bool status = await _repository.RemoveFromCartAsync(id);
            if (status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO vo)
        {
            bool status = await _repository.ApplyCouponAsync(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            bool status = await _repository.RemoveCouponAsync(userId);
            if (!status) return NotFound();
            return Ok(status);
        }
    }
}
