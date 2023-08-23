using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductRepository _productRepository;

        public CartController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy(int id)
        {
            List<ProductItems> cart = HttpContext.Session.Get<List<ProductItems>>("cart") ?? new List<ProductItems>();

            Product product = _productRepository.GetProductById(id);

            if (product != null)
            {
                int index = isExist(cart, id);

                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ProductItems { Product = product, Quantity = 1 });
                }

                HttpContext.Session.Set("cart", cart);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            List<ProductItems> cart = HttpContext.Session.Get<List<ProductItems>>("cart");
            int index = isExist(cart, id);

            if (index != -1)
            {
                cart.RemoveAt(index);
                HttpContext.Session.Set("cart", cart);
            }

            return RedirectToAction("Index");
        }

        private int isExist(List<ProductItems> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
