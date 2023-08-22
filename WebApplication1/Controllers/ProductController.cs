using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        public ActionResult Index()
        {
            ProductModel productModel = new ProductModel();
            ViewBag.products = productModel.FindAll();
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
                }

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                var newlyAddedProduct = _dbContext.Products.Find(product.Id); 

                // Pass the newly added product to the view
                ViewBag.NewProduct = newlyAddedProduct;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        


        [HttpGet]
        public IActionResult Edit(Product Model)
        {
            var product = Model.Id;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       

        [HttpGet]
        public IActionResult Delete(Product Model)
        {
            var product = Model.Id;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        

    }
}
