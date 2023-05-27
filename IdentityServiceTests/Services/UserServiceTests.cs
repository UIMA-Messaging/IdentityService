using IdentityService.Contracts;
using IdentityService.Exceptions;
using IdentityService.Repositories.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IdentityService.Services.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        public Mock<IUserRepository> mockUserRepository;

        public UserService userService;

        [TestInitialize] 
        public void Initialize() 
        {
            mockUserRepository = new Mock<IUserRepository>();
            userService = new UserService(mockUserRepository.Object);
        }

        public static User GetUser() => new()
        {
            Id = Guid.NewGuid().ToString(),
            Username = "thisIsAUsername",
            // Rest not needed
        };

        [TestMethod]
        public async Task GetUserByUsernameTest_ReturnsUser()
        {
            var user = GetUser();

            mockUserRepository
                .Setup(mock => mock.GetUserByUsername(user.Username))
                .ReturnsAsync(user);

            var result = await userService.GetUserByUsername(user.Username);

            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void GetUserByUsernameTest_ThrowsUserNotFound()
        {
            var user = GetUser();

            mockUserRepository
                .Setup(mock => mock.GetUserByUsername(user.Username))
                .ReturnsAsync((User)default);

            var action = () => userService.GetUserByUsername(user.Username);

            Assert.ThrowsExceptionAsync<UserNotFound>(action);
        }

        [TestMethod]
        public async Task GetUsersByQueryTest_ReturnsEmptyUserArray()
        {
            var query = "thisIsAQuery";
            var count = 10;
            var offset = 0;

            mockUserRepository
                .Setup(mock => mock.GetUsersByQuery(query, count, offset))
                .ReturnsAsync(Array.Empty<User>());

            var results = await userService.GetUsersByQuery(query, count, offset);

            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any());
        }

        [TestMethod]
        public async Task GetUsersByQueryTest_ReturnsUserArray()
        {
            var query = "thisIsAQuery";
            var count = 10;
            var offset = 0;
            var user1 = GetUser();
            var user2 = GetUser();
            var user3 = GetUser();

            mockUserRepository
                .Setup(mock => mock.GetUsersByQuery(query, count, offset))
                .ReturnsAsync(new User[] { user1, user2, user3 });

            var results = await userService.GetUsersByQuery(query, count, offset);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Contains(user1) && results.Contains(user2) && results.Contains(user3));
        }
    }
}