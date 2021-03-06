﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProduct.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }
        public string ShadeColor { get; set; }

        [Display(Name="Product Type")]
        public int ProductTypeId { get; set; }

       [ForeignKey("ProductTypeId")]
        public virtual ProductType ProductType { get; set; }

        [Display(Name = "Special Tag")]
        public int SpecialTagsId { get; set; }

        [ForeignKey("SpecialTagsId")]
        public virtual SpecialTags SpecialTags { get; set; }
    }
}
