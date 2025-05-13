using System.Threading;
using System.Threading.Tasks;
using ClientsService.Domain.Models;
using ClientsService.Persistence.Data;
using ClientsService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientsService.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private ClientsDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ClientsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ClientsDbContext(options);
        }

        private User CreateUser(string username, string email)
        {
            return new User
            {
                Username = username,
                Email = email,
                PasswordHash = "dummy", // если поле обязательно
                Role = "User"           // если поле обязательно
            };
        }

        [Fact]
        public async Task GetUserByUserNameAsync_ReturnsCorrectUser()
        {
            // Arrange
            var context = CreateDbContext();
            var repo = new UserRepository(context);

            var user1 = CreateUser("john_doe", "john@example.com");
            var user2 = CreateUser("alice", "alice@example.com");
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetUserByUserNameAsync("john_doe");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john_doe", result!.Username);
            Assert.Equal("john@example.com", result.Email);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_ReturnsNullIfNotFound()
        {
            var context = CreateDbContext();
            var repo = new UserRepository(context);

            var result = await repo.GetUserByUserNameAsync("does_not_exist");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ReturnsCorrectUser()
        {
            var context = CreateDbContext();
            var repo = new UserRepository(context);

            var user = CreateUser("bob", "bob@example.com");
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await repo.GetUserByEmailAsync("bob@example.com");

            Assert.NotNull(result);
            Assert.Equal("bob", result!.Username);
            Assert.Equal("bob@example.com", result.Email);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ReturnsNullIfNotFound()
        {
            var context = CreateDbContext();
            var repo = new UserRepository(context);

            var result = await repo.GetUserByEmailAsync("missing@example.com");

            Assert.Null(result);
        }
    }
}
