using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceProduct.Data;
using ECommerceProduct.Models;

namespace ECommerceProduct.Areas.Admin.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            return View(_db.ApplicationUser.ToList());
        }
        //Get Edit
        public async Task<IActionResult> Edit(string id)
        {
            if(id==null || id.Trim().Length==0)
            {
                return NotFound();
            }
            else
            {
                var userFromDB = await _db.ApplicationUser.FindAsync(id);
                if(userFromDB==null)
                {
                    return NotFound();
                }
                return View(userFromDB);
            }
        }
       
        //Post Edit'
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                ApplicationUser userFromDB = _db.ApplicationUser.Where(u => u.Id==id).FirstOrDefault();
                userFromDB.Name = applicationUser.Name;
                userFromDB.PhoneNumber = applicationUser.PhoneNumber;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser); 
        }
    }
}