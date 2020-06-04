using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceProduct.Models;
using ECommerceProduct.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceProduct.Extensions;
using ECommerceProduct.Data;

namespace ECommerceProduct.Areas.Customers.Controllers
{
   
    [Area("Customers")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Products>()
            };  

        }
        public async Task<IActionResult> Index()
        {
            List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(listShoppingCart.Count>0)
            {
                foreach(int cartItem in listShoppingCart)
                {
                    Products prod = _db.Products.Include(p=>p.SpecialTags).Include(p=>p.ProductType).Where(p => p.Id == cartItem).FirstOrDefault();//eager loading
                    ShoppingCartVM.Products.Add(prod);
                }
                  }
            return View(ShoppingCartVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> listCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            ShoppingCartVM.Appointments.ApointmentDate
             .AddHours(ShoppingCartVM.Appointments.ApointmentTime.Hour)
             .AddMinutes(ShoppingCartVM.Appointments.ApointmentTime.Minute);

            Appointments appointments = ShoppingCartVM.Appointments;
            _db.Appointments.Add(appointments);
            _db.SaveChanges();
            int appointmentId = appointments.Id;
            foreach(int productId in listCartItems)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = productId
                };
                _db.ProductsSelectedForAppointment.Add(productsSelectedForAppointment);
            }
            _db.SaveChanges();
            listCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", listCartItems);
            return RedirectToAction("AppointmentConfirmation","ShoppingCart",new { Id = appointmentId });            
        }
        public IActionResult Remove(int Id)
        {
            List<int> listCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(listCartItems.Count>0)
            {
                if(listCartItems.Contains(Id))
                {
                    listCartItems.Remove(Id);
                }
            }//if
            HttpContext.Session.Set("ssShoppingCart", listCartItems);
            return RedirectToAction(nameof(Index));
        }
        //Get : Action method for Confirmation
        public  IActionResult AppointmentConfirmation(int Id)
        {
            ShoppingCartVM.Appointments = _db.Appointments.Where(p => p.Id == Id).FirstOrDefault();
            List<ProductsSelectedForAppointment> productListObject = _db.ProductsSelectedForAppointment.Where(p => p.AppointmentId == Id).ToList();
        foreach(ProductsSelectedForAppointment productApointObject in productListObject)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.ProductType).Include(p => p.SpecialTags).Where(p => p.Id == productApointObject.ProductId).FirstOrDefault());
            }
            return View(ShoppingCartVM);
        }

    }
}