using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Data;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Service.UserService
{
    public class UserService
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
