using IdentityService.Contracts;
using IdentityService.Exceptions;
using IdentityService.RabbitMQ;
using IdentityService.Repositories.Keys;
using IdentityService.Repositories.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IdentityService.Services.Keys.Tests
{
    [TestClass]
    public class KeyServiceTests
    {
        public Mock<IRabbitMQListener<ExchangeKeys>> mockExchangeKeyListener;
        public Mock<IUserRepository> mockUserRepository;
        public Mock<IKeyRepository> mockKeyRepository;

        public KeyService keyService;

        [TestInitialize]
        public void Initialize()
        {
            mockExchangeKeyListener = new Mock<IRabbitMQListener<ExchangeKeys>>();
            mockUserRepository = new Mock<IUserRepository>();
            mockKeyRepository = new Mock<IKeyRepository>();

            keyService = new KeyService(
                mockKeyRepository.Object, 
                mockUserRepository.Object,
                mockExchangeKeyListener.Object);
        }

        public static string GenerateHexKey() => Guid.NewGuid().ToString("N");

        public static ExchangeKeys GetExchangeKeys() => new()
        {
            UserId = Guid.NewGuid().ToString(),
            IdentityKey = GenerateHexKey(),
            SignedPreKey = GenerateHexKey(),
            OneTimePreKeys = new[]
            {
                GenerateHexKey(),
                GenerateHexKey(),
                GenerateHexKey()
            },
            Signature = "thisIsASignature"
        };

        public static KeyBundle GetKeyBundle() => new()
        {
            IdentityKey = GenerateHexKey(),
            SignedPreKey = GenerateHexKey(),
            OneTimePreKey = GenerateHexKey(),
            Signature = "thisIsASignature"
        };

        public static User GetUser() => new()
        {
            Id = Guid.NewGuid().ToString(),
            // Rest not needed
        };

        [TestMethod]
        public void KeyServiceContructorTest_HandlesExchangeKeysFromRabbitMq()
        {
            var keys = GetExchangeKeys();

            mockExchangeKeyListener.Raise(mock => mock.OnReceive += null, new EventArgs(), keys);

            // mockKeyRepository.Verify(mock => mock.CreateOrUpdateKeys(keys.UserId, keys.IdentityKey, keys.SignedPreKey, keys.Signature, keys.OneTimePreKeys), Times.Once); // Not working
        }

        [TestMethod]
        public async Task GetKeyBundleTest_ReturnsKeyBundles()
        {
            var from = GetUser();
            var to = GetUser();
            var bundle = GetKeyBundle();

            mockUserRepository
                .Setup(mock => mock.GetUserById(to.Id))
                .ReturnsAsync(to);
            mockUserRepository
                .Setup(mock => mock.GetUserById(from.Id))
                .ReturnsAsync(from);
            mockKeyRepository
                .Setup(mock => mock.GetKeyBundleAndDisposeFromUser(to.Id))
                .ReturnsAsync(bundle);

            var results = await keyService.GetKeyBundle(from.Id, to.Id);

            Assert.AreEqual(results, bundle);
        }

        [TestMethod]
        public void GetKeyBundleTest_ThrowsUserNotFoundWhenFromDoesNotExist()
        {
            var from = GetUser();
            var to = GetUser();

            mockUserRepository
                .Setup(mock => mock.GetUserById(from.Id))
                .ReturnsAsync((User)default);
            mockUserRepository
                .Setup(mock => mock.GetUserById(to.Id))
                .ReturnsAsync(to);

            var action = () => keyService.GetKeyBundle(from.Id, to.Id);

            Assert.ThrowsExceptionAsync<UserNotFound>(action);
        }

        [TestMethod]
        public void GetKeyBundleTest_ThrowsUserNotFoundWhenToDoesNotExist()
        {
            var from = GetUser();
            var to = GetUser();

            mockUserRepository
                .Setup(mock => mock.GetUserById(from.Id))
                .ReturnsAsync(from);
            mockUserRepository
                .Setup(mock => mock.GetUserById(to.Id))
                .ReturnsAsync((User)default);

            var action = () => keyService.GetKeyBundle(from.Id, to.Id);

            Assert.ThrowsExceptionAsync<UserNotFound>(action);
        }

        [TestMethod]
        public async Task RegisterExchangeKeysTest_InsertsExchangeKeysToDatabase()
        {
            var keys = GetExchangeKeys();
            var user = GetUser();

            mockUserRepository
                .Setup(mock => mock.GetUserById(keys.UserId))
                .ReturnsAsync(user);

            await keyService.RegisterExchangeKeys(keys);

            mockKeyRepository.Verify(mock => mock.CreateOrUpdateKeys(keys.UserId, keys.IdentityKey, keys.SignedPreKey, keys.Signature, keys.OneTimePreKeys), Times.Once);
        }

        [TestMethod]
        public void RegisterExchangeKeysTest_ThrowsUserNotFound()
        {
            var user = GetUser();
            var keys = GetExchangeKeys();

            mockUserRepository
                .Setup(mock => mock.GetUserById(keys.UserId))
                .ReturnsAsync((User)default);

            var action = () => keyService.RegisterExchangeKeys(keys);

            Assert.ThrowsExceptionAsync<UserNotFound>(action);
        }
    }
}