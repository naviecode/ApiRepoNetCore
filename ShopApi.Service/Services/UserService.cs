using AutoMapper;
using Newtonsoft.Json;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.ProductCategoryDto;
using ShopApi.Service.Models.UserDto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopApi.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, 
            IUnitOfWork unitOfWork, 
            IUploadFileService uploadFileService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _uploadFileService = uploadFileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ResponseDataDto<UserResponse> GetAll()
        {
            var data = _userRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<UserResponse>(_mapper.Map<List<User>, List<UserResponse>>(data), totalItem);
        }
        public ResponseDataDto<UserResponse> GetAllByFilter(UserRequest filter)
        {
            var data = _userRepository.GetAll()
                .Where(x => (string.IsNullOrEmpty(filter.UserName) || x.UserName.Contains(filter.UserName))
                ).ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<UserResponse>(_mapper.Map<List<User>, List<UserResponse>>(data), totalItem);
        }
        public ResponseActionDto<UserResponse> GetById(int id)
        {
            var data = _userRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Không tìm thấy sản phẩm", "");
            }
            return new ResponseActionDto<UserResponse>(_mapper.Map<User, UserResponse>(data), CommonConstants.Success, "", "");
        }
        public ResponseActionDto<UserResponse> Add(UserFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var dataConvert = JsonConvert.DeserializeObject<UserCreate>(input.UserData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();
            try
            {
                if (dataConvert == null)
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Thêm mới thất bại", "ConvertJson not working");
                }
                //Nếu không có File post lên thì kh thực hiện service upload 
                if (input.File != null)
                {
                    resUpload = _uploadFileService.UploadFile(CommonConstants.UserFolder, input.File);
                    if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                    {
                        return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                    }
                    dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                }
                var resultAdd = _userRepository.Add(_mapper.Map<UserCreate, User>(dataConvert));
                _unitOfWork.Commit();
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Success, "Thêm mới thành công", resultAdd.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<UserResponse> Update(UserFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var dataConvert = JsonConvert.DeserializeObject<UserUpdate>(input.UserData, settings);
            var userGetById = _userRepository.GetSingleById(dataConvert.ID);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();
            try
            {
                if (dataConvert == null)
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Cập nhập thất bại", "ConvertJson not working");
                }
                if (input.File != null)
                {
                    //Nếu đã có file 
                    if (dataConvert.ImageKey != null)
                    {
                        resUpload = _uploadFileService.UpdateFile(CommonConstants.UserFolder, input.File, dataConvert.ImageKey);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        //Upload lại khi có keyImage trả về
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                    else //Nếu chưa có file
                    {
                        resUpload = _uploadFileService.UploadFile(CommonConstants.UserFolder, input.File);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                }
                dataConvert.Password = userGetById.Password;
                _userRepository.Update(_mapper.Map(_mapper.Map<User>(dataConvert), userGetById) );
                _unitOfWork.Commit();
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Success, "Cập nhập thành công", dataConvert.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<UserResponse> Delete(int id)
        {
            try
            {
                var productCategoryId = _userRepository.GetSingleById(id);
                if (productCategoryId == null)
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Không tìm thấy user", "");
                }
                var data = _userRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Success, "Xóa thành công", null);
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<UserResponse> Register(UserRegister register)
        {
            try
            {
                var user = _userRepository.GetSingleById(register.Id);
                if(user == null)
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Không tìm thấy người dùng", "");
                }
                if(!string.IsNullOrEmpty(user.Password))
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Tài khoản đã được đăng ý", "");

                }

                user.Password = register.PassWord;

                _userRepository.Update(user);
                _unitOfWork.Commit();

                return new ResponseActionDto<UserResponse>(null, CommonConstants.Success, "Đăng ký thành công","");
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<UserResponse> ChangePassword(UserRegister register)
        {
            try
            {
                var user = _userRepository.GetSingleById(register.Id);
                if (user == null)
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Không tìm thấy người dùng", "");
                }

                if(user.Password != register.PassWord)
                {
                    return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Mật khẩu cũ không khớp", "");

                }

                user.Password = register.NewPassword;

                _userRepository.Update(user);
                _unitOfWork.Commit();

                return new ResponseActionDto<UserResponse>(null, CommonConstants.Success, "Đổi mật khẩu thành công", "");
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<UserResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }
    }
}
