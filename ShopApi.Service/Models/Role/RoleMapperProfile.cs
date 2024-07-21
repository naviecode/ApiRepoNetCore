using AutoMapper;
using ShopApi.Model.Models;
using ShopApi.Service.Models.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models.Role
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile() 
        {
            CreateMap<Roles, RoleResponse>();

            CreateMap<RoleResponse, Roles>();
            CreateMap<RoleRequest, Roles>();
            CreateMap<RoleInput, Roles>();
        }
    }
}
