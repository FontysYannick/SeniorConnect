using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Data;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Services.UserService.Interface;

namespace SeniorConnect.API.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;

        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsUserEmailExist(AbstractAuthRequest userRegisterRequest)
        {
            return _dataContext.Users.Any(user => user.Email == userRegisterRequest.Email);
        }

        public async Task<User?> FindUser(AbstractAuthRequest userLoginRequest)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(user => user.Email == userLoginRequest.Email);
        }
    }
}
