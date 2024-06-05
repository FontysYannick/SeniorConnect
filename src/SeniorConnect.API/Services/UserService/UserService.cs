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

        public async Task<User?> FindUserById(int userId)
        {
            return await _dataContext.Users.FindAsync(userId);
        }

        public async Task ChangeUserInformation(UserChangeInfoRequest userChangeInformationRequest)
        {

            var user  = await FindUserById(userChangeInformationRequest.userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FirstName = userChangeInformationRequest.FirstName;
            user.LastName = userChangeInformationRequest.LastName;
            user.Preposition = userChangeInformationRequest.Preposition;
            user.Phonenumber = userChangeInformationRequest.PhoneNumber;

            await _dataContext.SaveChangesAsync();
        }
    }
}
