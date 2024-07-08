using Microsoft.AspNetCore.Http;
using ShopApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Abstractions
{
    public interface IUploadFileService
    {
        IDictionary<int, string> UploadFile(string folder, IFormFile file);
        IDictionary<int, string> UpdateFile(string folder, IFormFile file, string id);
        string UploadMultipleFiles(IFormFileCollection files);
    }
}
