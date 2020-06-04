using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Internal;
using ECommerceProduct.Models;
using ECommerceProduct.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using ECommerceProduct.Utility;
using ECommerceProduct.Data;

namespace ECommerceProduct.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnv;
        public ApplicationDbContext Context { get; set; }
        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }
        public ProductsController(ApplicationDbContext _context, HostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
            _db = _context;
            ProductsVM = new ProductsViewModel()
            {
                ProductType = _db.ProductType.ToList(),
                SpecialTags = _db.SpecialTags.ToList(),
                Products = new Products()
            };
            }

        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.SpecialTags);
            return View(await products.ToListAsync());
        }
        //Get Products Create
        public IActionResult Create()
        {
            return View(ProductsVM);
        }
        //HttPost Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(ProductsVM);
            }
            _db.Products.Add(ProductsVM.Products);
            await _db.SaveChangesAsync();
            //Images to be saved
            string webrootpath = _hostingEnv.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var ProductsFromDB = _db.Products.Find(ProductsVM.Products.Id);
            
            if (files.Count != 0)
            {
                //Image has beeb uploaded
                var uploads = Path.Combine(webrootpath, StaticDetails.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                ProductsFromDB.Image = @"\" + StaticDetails.ImageFolder + @"\" + ProductsVM.Products.Id + extension;
            }
            else
            {
                //When user doesnot upload Image
                var uploads = Path.Combine(webrootpath, StaticDetails.ImageFolder + @"\" + StaticDetails.DefualtProductImage);
                System.IO.File.Copy(uploads, webrootpath + @"\" + StaticDetails.ImageFolder + @"\" + ProductsVM.Products.Id + ".png");
                ProductsFromDB.Image = @"\" + StaticDetails.ImageFolder + @"\" + ProductsVM.Products.Id + ".png";               
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
         //Get :Edit 
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)

            {
                return NotFound();
            }
            ProductsVM.Products = await _db.Products.Include(m => m.ProductType).Include(m => m.SpecialTags).SingleOrDefaultAsync(m => m.Id == Id);
            if (ProductsVM.Products == null)
            {
                return NotFound();
            }
            else
            {
                return View(ProductsVM);
            }
        }
        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id)
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _hostingEnv.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var productFromDB = _db.Products.Where(m => m.Id == ProductsVM.Products.Id).FirstOrDefault();
                if(files.Count>0 && files[0]!=null)
                {
                    //if User uploads a new Image
                    var uploads = Path.Combine(webRootPath, StaticDetails.ImageFolder);
                    var newExtention = Path.GetExtension(files[0].FileName);
                    var oldExtention = Path.GetExtension(productFromDB.Image);
                    if (System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Products.Id + oldExtention)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Products.Id + oldExtention));
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + newExtention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    ProductsVM.Products.Image = @"\" + StaticDetails.ImageFolder + @"\" + ProductsVM.Products.Id + newExtention;
                }
                if(ProductsVM.Products.Image!=null)
                {
                    productFromDB.Image = ProductsVM.Products.Image;
                }
                productFromDB.Name = ProductsVM.Products.Name;
                productFromDB.Price = ProductsVM.Products.Price;
                productFromDB.Available = ProductsVM.Products.Available;
                productFromDB.ProductTypeId = ProductsVM.Products.ProductTypeId;
                productFromDB.SpecialTagsId = ProductsVM.Products.SpecialTagsId;
                productFromDB.ShadeColor = ProductsVM.Products.ShadeColor;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ProductsVM);
        }
        //Get :Details Action Method 
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)

            {
                return NotFound();
            }
            ProductsVM.Products = await _db.Products.Include(m => m.ProductType).Include(m => m.SpecialTags).SingleOrDefaultAsync(m => m.Id == Id);
            if (ProductsVM.Products == null)
            {
                return NotFound();
            }
            else
            {
                return View(ProductsVM);
            }
        }
        //Get :Delete Action Method 
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)

            {
                return NotFound();
            }
            ProductsVM.Products = await _db.Products.Include(m => m.ProductType).Include(m => m.SpecialTags).SingleOrDefaultAsync(m => m.Id == Id);
            if (ProductsVM.Products == null)
            {
                return NotFound();
            }
            else
            {
                return View(ProductsVM);
            }
        }

        //HttpPost Delete Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            string webrootpath = _hostingEnv.WebRootPath;
            Products products = await _db.Products.FindAsync(Id);
            if(products==null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webrootpath, StaticDetails.ImageFolder);
                var extension = Path.GetExtension(products.Image);
                if(System.IO.File.Exists(Path.Combine(uploads,products.Id+extension)))
               {
                    System.IO.File.Delete(Path.Combine(uploads, products.Id + extension));
                   }
                _db.Products.Remove(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
        }
    }
}