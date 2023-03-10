using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.Role;
using TheCompleteProject.ModelsAndDto_s.DbModels.Role.Dto;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.Utility;
using TheCompleteProject.Utility.Response;

namespace TheCompleteProject.Service.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _accessor;
        //private readonly IExceptionLoggingService _exceptionLogging;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessor = accessor;
        }

        public async Task<GenericResponse> GetRoleById(int roleId)
        {
            var response = new GenericResponse();
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                response.WriteCustomErrormessage(AppConstant.Message.NoRecord.ToString());
            }
            response.Ok(role);
            return response;
        }

        public async Task<GenericResponse> GetAllRoles()
        {
            var response = new GenericResponse();
            var httpContext = _accessor.HttpContext;

            var roles = await _unitOfWork.RoleRepository.GetAsync();
            if (roles == null)
            {
                //response.WriteCustomErrormessage("No Records Found");
                response.WriteCustomErrormessage(AppConstant.Message.NoRecord.ToString());
                return response;
            }
            response.Ok(roles);
            return response;
        }

        public async Task<GenericResponse> AddRoles(AddRoleDto roleDto)
        {
            try
            {
                var response = new GenericResponse();
                Roles role = new Roles();
                _mapper.Map(roleDto, role);
                role.IsActive = true;
                role.CreatedDate = DateTime.Now;
                role.ModifiedDate = DateTime.Now;
                var isSuccess = (Roles)await _unitOfWork.RoleRepository.AddAsync(role);
                await _unitOfWork.SaveChangesAsync();

                response.Ok(isSuccess);
                return response;
            }
            catch (Exception ex)
            {
                //_exceptionLogging.LogExceptionToDB(ex, _accessor);
                throw ex;
            }
        }
        public async Task<GenericResponse> UpdateRole(UpdateRoleDto roleDto)
        {
            var response = new GenericResponse();
            try
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleDto.Id);
                if (role == null)
                {
                    response.WriteCustomErrormessage(AppConstant.Message.NoRecord.ToString());
                    return response;
                }
                _mapper.Map(roleDto, role);
                role.ModifiedDate = DateTime.Now;

                var isSuccess = (Roles)await _unitOfWork.RoleRepository.UpdateAsync(role);
                await _unitOfWork.SaveChangesAsync();

                response.Ok(isSuccess);
                return response;
            }
            catch (Exception ex)
            {
                //_exceptionLogging.LogExceptionToDB(ex, _accessor);
                response.WriteCustomErrormessage("Exception Occurred");
                return response;
            }
        }

        public async Task<GenericResponse> DeactiveRole(int roleId)
        {
            GenericResponse response = new GenericResponse();
            var role = await _unitOfWork.RoleRepository.GetDefaultAsync(x => x.Id == roleId);
            if (role == null)
            {
                response.WriteCustomErrormessage(AppConstant.Message.NoRecord.ToString());
                return response;
            }
            role.IsActive = false;
            var Deactive = (Roles)await _unitOfWork.RoleRepository.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            response.Ok(role);
            return response;
        }
    }
}
