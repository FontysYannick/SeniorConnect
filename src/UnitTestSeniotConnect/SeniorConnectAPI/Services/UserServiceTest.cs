
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Service.UserService;
using SeniorConnect.API.Models.Users;

namespace UnitTestSeniorConnect.SeniorConnectAPI.Services
{
    public class UserServiceTest
    {
        private Fixture _fixture;

        public UserServiceTest()
        {
            _fixture = new Fixture();
        }

        private Fixture UserFixtureWithOutReference()
        {
            var userFixture = new Fixture();
            userFixture.Customize<User>(user => user
            .Without(u => u.ActivityUsers)
            .Without(u => u.Activities)
            );

            userFixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return userFixture;
        }

        public async Task<DataContext> GetDataContextMock()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: $"SeniorConnectDbTest_{System.Guid.NewGuid()}")
                .Options;

            var dataContext = new DataContext(options);

            dataContext.Database.EnsureCreated();

            return dataContext;
        }

        [Fact]
        public async Task UserServiceIsUserEmailExistReturnTrue()
        {
            //Arrange
            var dataContext = await GetDataContextMock();
            var user = UserFixtureWithOutReference().Create<User>();
            var fakeUserRegisterRequest = new Fixture();

            fakeUserRegisterRequest.Customize<UserRegisterRequest>(
                u => u.With(u => u.Email , user.Email)
            );

            var userRequest = fakeUserRegisterRequest.Create<UserRegisterRequest>();

            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();

            var userService = new UserService(dataContext);
            var result = userService.IsUserEmailExist(userRequest);

            //Assert
            Assert.True(result);
            Assert.Equal(user.Email, userRequest.Email);
        }

        [Fact]
        public async Task UserServiceIsUserEmailExistReturnFalse()
        {
            //Arrange
            var dataContext = await GetDataContextMock();
            var userRequest = _fixture.Create<UserRegisterRequest>();

            var userService = new UserService(dataContext);
            var result = userService.IsUserEmailExist(userRequest);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void UserServiceFindUserWillReturnNull()
        {
            //Arrange
            var dataContext = await GetDataContextMock();
            var userRequest = _fixture.Create<UserLoginRequest>();

            var userService = new UserService(dataContext);
            var result = await userService.FindUser(userRequest);

            //Assert
            Assert.Null(result);
        }
    }
}
