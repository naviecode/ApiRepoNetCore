using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApi.Service.Abstractions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using ShopApi.Common;

namespace ShopApi.Service.Services
{
    
    public class UploadFileService : IUploadFileService
    {
        public UploadFileService()
        {
        }

        public IDictionary<int, string> UploadFile(string folder, IFormFile file)
        {
            // IDictionary<string, string>
            if (file != null && file.Length > 0)
            {
                // Cấu hình lưu trữ file trên ggdrive
                string credentialsPath = @"UploadConfig/credentials.json";
                string folderId = "17kCNuvfeM2JVN58uy2h6E8Znl82PyCdH";
                switch (folder)
                {
                    case "Product":
                        folderId = "140xTLh1NXcbr1FOHh5BwvbvsFk1uLahi";
                        break;
                    case "ProductCategory":
                        folderId = "15Ki-CqJrR6mvpzWGTXBzaHhSN01-K34-";
                        break;
                    case "User":
                        folderId = "1w5y2KCTeGRKWc03PVSLaHRxTWsh0B23x";
                        break;
                    default:
                        break;
                }

                //Thông tin file submit
                string fileName = file.FileName;

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadConfig/UploadTemp");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                else
                {
                    var files = Directory.GetFiles(uploadFolder);
                    foreach (string fileDelete in files)
                    {
                        File.Delete(fileDelete);
                    }
                }
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream).Wait();
                }

                //Thực hiên tải file lên google drive
                string reuslt = UploadFileToGoogleDrive(credentialsPath, folderId, filePath);  


                if(string.IsNullOrEmpty(reuslt))
                {
                    return new Dictionary<int, string> { { CommonConstants.ErrorSystem, "Tải File Không thành công" } };
                }

                return new Dictionary<int, string> { { CommonConstants.Success, reuslt } };
            }
            else
            {
                return new Dictionary<int, string> { { CommonConstants.Success, "File ảnh không hợp lệ" } };                
            }
        }
        public IDictionary<int, string> UpdateFile(string folder, IFormFile file, string id)
        {
            if (file != null && file.Length > 0)
            {
                // Cấu hình lưu trữ file trên ggdrive
                string credentialsPath = @"UploadConfig/credentials.json";

                //Thông tin file submit
                string fileName = file.FileName;

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadConfig/UploadTemp");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                else
                {
                    var files = Directory.GetFiles(uploadFolder);
                    foreach (string fileDelete in files)
                    {
                        File.Delete(fileDelete);
                    }
                }
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream).Wait();
                }

                //Thực hiện cập nhập file lên google drive
                string reuslt = UpadteFileToGoogleDrive(credentialsPath, filePath, id);


                if (string.IsNullOrEmpty(reuslt))
                {
                    return new Dictionary<int, string> { { CommonConstants.ErrorSystem, "Cập nhập File không thành công" } };
                }

                return new Dictionary<int, string> { { CommonConstants.Success, reuslt } };
            }
            else
            {
                return new Dictionary<int, string> { { CommonConstants.Success, "File ảnh không hợp lệ" } };
            }
        }

        public string UploadMultipleFiles(IFormFileCollection files)
        {
            //if (files != null && files.Count > 0)
            //{
            //    List<FileInfoVM> fileInfos = new List<FileInfoVM>();

            //    foreach (var file in files)
            //    {
            //        if (file != null && file.Length > 0)
            //        {
            //            // Lưu trữ file ảnh vào thư mục cụ thể
            //            string fileName = file.FileName;
            //            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);
            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                file.CopyToAsync(stream).Wait();
            //            }

            //            // Thêm thông tin về file ảnh vào danh sách
            //            fileInfos.Add(new FileInfoVM { FileName = fileName, FilePath = filePath });
            //        }
            //    }

            //    // Trả về thông tin về danh sách file ảnh đã được lưu trữ
            //    return Ok(fileInfos);
            //}
            //else
            //{
            //    return BadRequest("Danh sách file ảnh không hợp lệ");
            //}
            return "";
        }
        public string UploadFileToGoogleDrive(string credentialsPath, string folderId, string filePath)
        {
            GoogleCredential credential;
            using(var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.ScopeConstants.DriveFile
                });
            }
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Drive Upload"
            });

            var fileMetaDate = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath),
                Parents = new List<string> { folderId }
            };

            FilesResource.CreateMediaUpload request;
            using(var stream = new FileStream(filePath, FileMode.Open)) 
            {
                request = service.Files.Create(fileMetaDate, stream, "image/png");
                request.Fields = "id";
                request.Upload();
            }
            //Check url view https://drive.google.com/file/d/{response.id}/view
            var response = request.ResponseBody.Id ?? "";

            return response;

        }
        public string UpadteFileToGoogleDrive(string credentialsPath, string filePath, string idUpdate)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.ScopeConstants.DriveFile
                });
            }

            //Tạo Drive API service
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Drive Upadte File"
            });

            var fileMetaDate = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath)
            };

            FilesResource.UpdateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Update(fileMetaDate, idUpdate, stream, filePath);
                request.Fields = "id";
                request.Upload();
            }
            //Check url view https://drive.google.com/file/d/{response.id}/view
            var response = request.ResponseBody.Id ?? "";

            return response;
        }
    }
}
