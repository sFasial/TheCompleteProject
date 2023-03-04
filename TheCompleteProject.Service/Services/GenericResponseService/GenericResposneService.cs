using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.Authentication;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.Repository.Repositories.Jwt;
using TheCompleteProject.Service.Services.Jwt;
using TheCompleteProject.Utility;
using TheCompleteProject.Utility.Response;

namespace TheCompleteProject.Service.Services.GenericResponseService
{
    public class GenericResposneService : IGenericResponse
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtRefreshTokenService _jwtRefreshTokenService;

        public GenericResposneService(
                                        IUnitOfWork unitOfWork
                                      , IMapper mapper
                                      , IJwtRefreshTokenService jwtRefreshTokenService
                                     )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtRefreshTokenService = jwtRefreshTokenService;
        }

        public async Task<GenericResponse> Login(LoginRequestDto loginDto)
        {
            var mappedUser = new Users();
            _mapper.Map(loginDto, mappedUser);
            var response = new GenericResponse();
            var user = await _jwtRefreshTokenService.IsValidUserAsync(mappedUser);
            if (user == null)
            {
                response.WriteCustomErrormessage(AppConstant.NoUserFound);
            }
            response.Ok(user);
            return response;
        }
    }
}
