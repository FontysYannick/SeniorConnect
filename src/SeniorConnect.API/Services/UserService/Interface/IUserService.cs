using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Services.UserService.Interface
{
    public interface IUserService
    {
        bool IsUserEmailExist(AbstractAuthRequest userRegisterRequest);
        Task<User?> FindUser(AbstractAuthRequest userLoginRequest);
        public Task<User?> FindUserById(int userId);
        public Task ChangeUserInformation(UserChangeInfoRequest userChangeInformationRequest);
    }
}
