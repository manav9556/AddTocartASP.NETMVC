using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy(int id)
        {
            ProductModel productModel = new ProductModel();
            List<ProductItems> cart = HttpContext.Session.Get<List<ProductItems>>("cart") ?? new List<ProductItems>();
            string ids=id.ToString();
            int index = isExist(cart, ids);
            if (index != -1)
            {
                cart[index].Quantity++;
            }
            else
            {
                cart.Add(new ProductItems { Product = productModel.Find(id), Quantity = 1 });
            }

            HttpContext.Session.Set("cart", cart);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(string id)
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

        private int isExist(List<ProductItems> cart, string id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
