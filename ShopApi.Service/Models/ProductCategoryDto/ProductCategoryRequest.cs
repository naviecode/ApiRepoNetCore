﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models.ProductCategoryDto
{
    public class ProductCategoryRequest
    {
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
