using ECommerceProduct.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProduct.Models
{
    public class ProductImpl : IProductRepository
    {
        public ApplicationDbContext Context { get; set; }
        public ProductImpl(ApplicationDbContext Context)
        {
            this.Context = Context;
        }

        public Products Add(Products product)
        {
            Context.Products.Add(product);
            Context.SaveChanges();
            return product;
        }

        public Products delete(int Id)
        {
            Products product= Context.Products.Find(Id);
            if (product != null)
            {
                Context.Products.Remove(product);
            }
            return product;
        }

        public IEnumerable<Products> getAllProduct()
        {
           return Context.Products;
        }

        public Products getProduct(int Id)
        {
            return Context.Products.Find(Id);
        }

        public Products update(Products changeproducts)
        {
            var updates = Context.Products.Attach(changeproducts);
            updates.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Context.SaveChanges();
            return changeproducts;
        }

        }
    }

