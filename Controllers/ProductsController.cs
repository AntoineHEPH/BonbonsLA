using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Condorcet.B2.AspnetCore.MVC.Application.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: ProjectsController
        public async Task<ActionResult> Index()
        {
            var products = await _productRepository.GetAll();
            return View(new ProductIndexViewModel()
            {
                Products = products
            });
        }

        [Authorize(Policy = "CreateProductPolicy")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Policy = "CreateProductPolicy")]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _productRepository.Insert(new Product
            {
                Name = model.Name,
                Description = model.Description,
                Type = (int)model.Type,
                Prix = model.Prix
            });
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                return NotFound();
            return View(new ProductFormViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Type = (ProductType)product.Type,
                Prix = product.Prix
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _productRepository.Update(model.Id.Value, new Product
            {
                Name = model.Name,
                Description = model.Description,
                Type = (int)model.Type,
                Prix = model.Prix,
            });
            return RedirectToAction(nameof(Index));
        }

    }
}