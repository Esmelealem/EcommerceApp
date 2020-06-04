using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProduct.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Products>().HasData(new Products
            //{
            //    productId = 1,
            //    productName = "Apple",
            //    productType = "fruit"
            //},
            //new Product
            //{
            //    productId = 2,
            //    productName = "Mango",
            //    productType = "fruit"
            //}

            //    );
        }
    }
}
