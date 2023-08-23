using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProductRepository _productRepository;

        public ProductController(ProductDbContext dbContext, IWebHostEnvironment webHostEnvironment, ProductRepository productRepository)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            ViewBag.products = products;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
       
        public IActionResult Create(Product model)
        {
            try
            {
                Product product = new Product();
                product.Id = model.Id;
                product.Name = model.Name;
                product.Description = model.Description;

                product.Price = model.Price;

                if (model.ImageFile != null && model.ImageFile is IFormFile imageFile)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images2");
                    string uniqueFileName = $"{Guid.NewGuid().ToString()}_{imageFile.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    product.Image = uniqueFileName;
                    product.ImageFileName = uniqueFileName;
                }

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                var newlyAddedProduct = _dbContext.Products.Find(product.Id);


                ViewBag.NewProduct = newlyAddedProduct;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Edit(Product model)
        {
            
                try
                {
                    var existingProduct = _dbContext.Products.Find(model.Id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = model.Name;
                    existingProduct.Description = model.Description;
                    existingProduct.Price = model.Price;

                    _dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while editing the product.");
                }
            

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var productToDelete = _dbContext.Products.Find(id);

                if (productToDelete == null)
                {
                    return NotFound();
                }

                _dbContext.Products.Remove(productToDelete);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle any deletion errors
                return RedirectToAction("Index");
            }
        }
    }

}
