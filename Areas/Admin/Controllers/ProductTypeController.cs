using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceProduct.Data;
using ECommerceProduct.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProduct.Controllers
{
    [Area("Admin")] 
    public class ProductTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationDbContext Context { get; set; }
        public ProductTypeController(ApplicationDbContext _context)
        {
            _db = _context;
        }
        public IActionResult Index()
        {
            return View(_db.ProductType.ToList());
                
        }
        //Get Create Action Method
        public IActionResult Create()
        {
            return View();
        }
        //Post: Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType product)
        {
            if(ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //Get Edit Action Method
        public async Task<IActionResult> Edit(int? Id)
        {
            if(Id==null)
            {
                return NotFound();
            }
           
            var product = await _db.ProductType.FindAsync(Id);
            if (product==null)
            {
                return NotFound();
            }
                return View(product);
        }

        //HttpPost Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id,ProductType product)
        {
            if (Id!= product.Id)
            {
                return NotFound();
            }
           if (ModelState.IsValid)
            {
                _db.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Index");
            }
            return View(product);
        }
        //Get: Details Action Method
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var product = await _db.ProductType.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Get: Delete Action Method
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var product = await _db.ProductType.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //HttpPost Delete Action Method
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
                var product = await _db.ProductType.FindAsync(Id);
                _db.ProductType.Remove(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));            
        }
    }
}