using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Services.ActivityService;
using SeniorConnect.API.Models.Activity;

namespace SeniorConnect.Tests
{
    public class ActivityServiceTest
    {
        private readonly Mock<DataContext> _contextMock;
        private readonly ActivityService _activityService;

        public ActivityServiceTest()
        {
            _contextMock = new Mock<DataContext>();
            _activityService = new ActivityService(_contextMock.Object);
        }

        [Fact]
        public void GetActivities_ShouldReturnFutureActivities()
        {
            // Arrange
            var activities = new List<Activity>
            {
                new Activity { ActivityId = 1, Title = "Past Activity", Date = DateTime.Now.AddDays(-1) },
                new Activity { ActivityId = 2, Title = "Future Activity", Date = DateTime.Now.AddDays(1) }
            }.AsQueryable();

            var dbSetMock = new Mock<DbSet<Activity>>();
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.Provider).Returns(activities.Provider);
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.Expression).Returns(activities.Expression);
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.ElementType).Returns(activities.ElementType);
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.GetEnumerator()).Returns(activities.GetEnumerator());

            _contextMock.Setup(c => c.Activities).Returns(dbSetMock.Object);

            // Act
            var result = _activityService.GetActivities();

            // Assert
            Assert.Single(result);
            Assert.Equal("Future Activity", result.First().Title);
        }

        [Fact]
        public void GetSingleActivity_ShouldReturnActivityWithOrganizer()
        {
            // Arrange
            var activities = new List<Activity>
            {
                new Activity { ActivityId = 1, Title = "Test Activity", Organizer = new User { UserId = 1, Email = "test@example.com" } }
            }.AsQueryable();

            var dbSetMock = new Mock<DbSet<Activity>>();
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.Provider).Returns(activities.Provider);
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.Expression).Returns(activities.Expression);
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.ElementType).Returns(activities.ElementType);
            dbSetMock.As<IQueryable<Activity>>().Setup(m => m.GetEnumerator()).Returns(activities.GetEnumerator());

            _contextMock.Setup(c => c.Activities).Returns(dbSetMock.Object);

            // Act
            var result = _activityService.GetSingleActivity(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Activity", result.Title);
            Assert.NotNull(result.Organizer);
            Assert.Equal("test@example.com", result.Organizer.Email);
        }

        [Fact]
        public void SetActivity_ShouldAddNewActivity()
        {
            // Arrange
            var activity = new AbstractActivity
            {
                Title = "New Activity",
                OrganizerId = 1,
                Description = "Description",
                Image = "Image",
                Date = DateTime.Now.AddDays(1),
                Place = "Place",
                MaxParticipants = 10,
                Awards = "Awards"
            };

            var dbSetMock = new Mock<DbSet<Activity>>();
            _contextMock.Setup(c => c.Activities).Returns(dbSetMock.Object);

            // Act
            _activityService.SetActivity(activity);

            // Assert
            dbSetMock.Verify(d => d.Add(It.IsAny<Activity>()), Times.Once);
            _contextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void AddUserToActivity_ShouldAddUserActivity()
        {
            // Arrange
            var userActivity = new AbstractUserActivty
            {
                UserId = 1,
                ActivityId = 1
            };

            var dbSetMock = new Mock<DbSet<ActivityUsers>>();
            _contextMock.Setup(c => c.ActivityUsers).Returns(dbSetMock.Object);

            // Act
            _activityService.AddUserToActivity(userActivity);

            // Assert
            dbSetMock.Verify(d => d.Add(It.IsAny<ActivityUsers>()), Times.Once);
            _contextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void GetUserToActivity_ShouldReturnUserActivities()
        {
            // Arrange
            var userId = 1;
            var activities = new List<ActivityUsers>
            {
                new ActivityUsers { UserId = userId, Activity = new Activity { Title = "Future Activity", Date = DateTime.Now.AddDays(1) }},
                new ActivityUsers { UserId = userId, Activity = new Activity { Title = "Past Activity", Date = DateTime.Now.AddDays(-1) }}
            }.AsQueryable();

            var dbSetMock = new Mock<DbSet<ActivityUsers>>();
            dbSetMock.As<IQueryable<ActivityUsers>>().Setup(m => m.Provider).Returns(activities.Provider);
            dbSetMock.As<IQueryable<ActivityUsers>>().Setup(m => m.Expression).Returns(activities.Expression);
            dbSetMock.As<IQueryable<ActivityUsers>>().Setup(m => m.ElementType).Returns(activities.ElementType);
            dbSetMock.As<IQueryable<ActivityUsers>>().Setup(m => m.GetEnumerator()).Returns(activities.GetEnumerator());

            _contextMock.Setup(c => c.ActivityUsers).Returns(dbSetMock.Object);

            // Act
            var result = _activityService.GetUserToActivity(userId);

            // Assert
            Assert.Single(result);
            Assert.Equal("Future Activity", result.First().Title);
        }
    }
}
