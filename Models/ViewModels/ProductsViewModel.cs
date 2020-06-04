using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProduct.Models.ViewModels
{
    public class ProductsViewModel
    {
        public Products Products { get; set; }
        public IEnumerable<ProductType>ProductType { get; set; }
        public IEnumerable<SpecialTags>SpecialTags { get; set; }
    }
}
