using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models
{
    public class ProductFormFile
    {
        public string ProductData { get; set; }
        public IFormFile File { get; set; }
    }
}
