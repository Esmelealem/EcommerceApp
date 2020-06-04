using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceProduct.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ECommerceProduct.Extensions;
using ECommerceProduct.Data;

namespace ECommerceProduct.Controllers
{
    [Area("Customers")]
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var pruductList = await _db.Products.Include(m => m.ProductType).Include(m => m.SpecialTags).ToListAsync();
            return View(pruductList);
        }
        //Get:Detail Action method
        public async Task<IActionResult> Details(int Id)
        {
            var product = await _db.Products.Include(m => m.ProductType).Include(m => m.SpecialTags).Where(m=>m.Id==Id).FirstOrDefaultAsync();
            return View(product);
        }

        //HttpPost:Detail Action method
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPost(int Id)
        {
            List<int> listShopCart =  HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(listShopCart==null)
            {
                listShopCart =  new List<int>();
            }
             listShopCart.Add(Id);
            HttpContext.Session.Set("ssShoppingCart", listShopCart);
            return RedirectToAction("Index", "Home",  new { Area = "Customers" });
        }

        public IActionResult Remove(int Id)
        {
            List<int> listShopCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(listShopCart.Count>0)
            {
                if(listShopCart.Contains(Id))
                {
                    listShopCart.Remove(Id);
                }
            }
            HttpContext.Session.Set("ssShoppingCart", listShopCart);
            return RedirectToAction(nameof(Index));

        }
    }
}
