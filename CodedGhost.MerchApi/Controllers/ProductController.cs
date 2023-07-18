﻿using CodedGhost.MerchApi.Interfaces.Services;
using CodedGhost.MerchApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CodedGhost.MerchApi.Controllers;

[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("")]
    public async Task<IActionResult> GetProducts([FromBody] ProductsRequest request)
    {
        if (request?.CategoryIds == null) return BadRequest();

        var products = await _productService.GetProducts(request.CategoryIds);

        return Json(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        if (id == 0) return BadRequest();

        var product = await _productService.GetProduct(id);

        return Json(product);
    }
}