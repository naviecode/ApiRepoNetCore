using AutoMapper;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.ProductCategoryDto;
using ShopApi.Service.Models.Role;
using ShopApi.Service.Models.UserDto;

namespace ShopApi.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public RoleService(IRoleRepository roleRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IUserContextService userContextService)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public ResponseActionDto<RoleResponse> Add(RoleInput input)
        {
            try
            {
                var dataConvert = _mapper.Map<Roles>(input);
                dataConvert.CreatedBy = this._userContextService.GetUserName();
                dataConvert.CreatedDate = DateTime.Now;

                var resultAdd = _roleRepository.Add(dataConvert);

                _unitOfWork.Commit();
                return new ResponseActionDto<RoleResponse>(null, CommonConstants.Success, "Thêm mới thành công", resultAdd.ID.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<RoleResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<RoleResponse> Delete(int id)
        {
            try
            {
                var productCategoryId = _roleRepository.GetSingleById(id);
                if (productCategoryId == null)
                {
                    return new ResponseActionDto<RoleResponse>(null, Common.CommonConstants.Error, "Không tìm thấy sản phẩm", "");
                }
                var data = _roleRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<RoleResponse>(null, Common.CommonConstants.Success, "Xóa thành công","");
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<RoleResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseDataDto<RoleResponse> GetAll()
        {
            var data = _roleRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<RoleResponse>(_mapper.Map<List<Roles>, List<RoleResponse>>(data), totalItem);
        }
        public ResponseDataDto<RoleResponse> GetAllByFilter(RoleRequest filter)
        {
            var data = _roleRepository.GetAll()
                .Where(x => (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) && (filter.Status == null || x.Status == filter.Status)
                ).ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<RoleResponse>(_mapper.Map<List<Roles>, List<RoleResponse>>(data), totalItem);
        }
        public ResponseDataDto<RoleResponse> GetRoleCombobox()
        {
            var data = _roleRepository.GetAll().Where(x => x.Status == true).ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<RoleResponse>(_mapper.Map<List<Roles>, List<RoleResponse>>(data), totalItem);
        }
        public ResponseActionDto<RoleResponse> GetById(int id)
        {
            var data = _roleRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<RoleResponse>(null, CommonConstants.Error, "Không tìm thấy quyền", "");
            }
            return new ResponseActionDto<RoleResponse>(_mapper.Map<Roles, RoleResponse>(data), CommonConstants.Success, "", "");
        }

        public ResponseActionDto<RoleResponse> Update(RoleInput input)
        {
            try
            {
                var dataConvert = _mapper.Map<Roles>(input);
                dataConvert.UpdatedBy = this._userContextService.GetUserName();
                dataConvert.UpdatedDate = DateTime.Now;
                _roleRepository.Update(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<RoleResponse>(null, Common.CommonConstants.Success, "Cập nhập thành công", input.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<RoleResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }
    }
}
