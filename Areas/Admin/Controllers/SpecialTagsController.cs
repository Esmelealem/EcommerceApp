using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceProduct.Models;
using ECommerceProduct.Data;

namespace ECommerceProduct.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationDbContext Context { get; set; }
        public SpecialTagsController(ApplicationDbContext _context)
        {
            _db = _context;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }
        //Get Create Action Method
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.Add(specialTag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }
    }
}