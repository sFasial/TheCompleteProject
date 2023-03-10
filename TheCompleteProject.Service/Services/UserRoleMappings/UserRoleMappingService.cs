using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings.Dto;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.Utility;
using TheCompleteProject.Utility.Response;

namespace TheCompleteProject.Service.Services.UserRoleMappings
{
    public class UserRoleMappingService : IUserRoleMappingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _accessor;
        public UserRoleMappingService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessor = accessor;
        }

        public IQueryable<object> GetUserRoleMapping()
        {
            var userRoleMappings = _unitOfWork.UserRoleMappingRepository.GetUserRoleMapping();
            return userRoleMappings;
        }
        public GenericResponse AddUserRoleMapping(AddUserRoleMapping model)
        {
            GenericResponse response = new GenericResponse();
            UserRoleMapping _userRoleMapping = new UserRoleMapping();
            _mapper.Map(model, _userRoleMapping);
            _unitOfWork.UserRoleMappingRepository.Add(_userRoleMapping);
            Task.Run(() => _unitOfWork.SaveChangesAsync());

            response.Ok(_userRoleMapping);
            return response;
        }

        public async Task<GenericResponse> UpdateUserRoleMappingAsync(UserRoleMappingDto model, int id)
        {
            GenericResponse response = new GenericResponse();
            var userRoleMapping = await _unitOfWork.UserRoleMappingRepository.GetByIdAsync(id);
            if (userRoleMapping == null)
            {
                response.WriteCustomErrormessage(AppConstant.Message.NoRecord.ToString());
                return response;
            }
            _mapper.Map(model, userRoleMapping);
            userRoleMapping.ModifiedDate = DateTime.Now;
            _unitOfWork.UserRoleMappingRepository.Update(userRoleMapping);
            await _unitOfWork.SaveChangesAsync();

            response.Ok(userRoleMapping);
            return response;
        }

        public async Task<GenericResponse> DeactiveUserRoleMapping(int id)
        {
            GenericResponse response = new GenericResponse();
            var userRoleMapping = await _unitOfWork.UserRoleMappingRepository.GetByIdAsync(id);
            if (userRoleMapping == null)
            {
                response.WriteCustomErrormessage(AppConstant.Message.NoRecord.ToString());
                return response;
            }
            userRoleMapping.IsActive = false;
            _unitOfWork.UserRoleMappingRepository.Update(userRoleMapping);
            await _unitOfWork.SaveChangesAsync();

            response.Ok(userRoleMapping);
            return response;
        }

        public List<UserRoleMappingDto> GetAllRolesByUserId(int userId)
        {
            var userRoles = _unitOfWork.UserRoleMappingRepository.GetAllRolesByUserId(userId);
            return userRoles;
        }
    }
}
