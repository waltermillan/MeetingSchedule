using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Users.Commands
{
    public class UpdateUserHandlerTests
    {
        // Helper to load test data from a JSON file
        private List<User> LoadUserDataFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Users", "UpdateUserTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<User>>(json);
            return data!;
        }

        // Helper to create an in-memory DbContext
        private static async Task<Context> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new Context(options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        [Fact]
        public async Task Update_ShouldModifyUser()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new UserRepository(context);

            var users = LoadUserDataFromJson();
            var user = users[0];

            repo.Add(user);
            await context.SaveChangesAsync();

            var updatedUser = users[1];
            user.Name = updatedUser.Name;
            user.UserName = updatedUser.UserName;
            user.Password = updatedUser.Password;

            // Act
            repo.Update(user);
            await context.SaveChangesAsync();

            // Assert
            var updatedUserInDb = await context.Users.FindAsync(user.Id);
            Assert.NotNull(updatedUserInDb);
            Assert.Equal(updatedUser.Name, updatedUserInDb.Name);
            Assert.Equal(updatedUser.UserName, updatedUserInDb.UserName);
            Assert.Equal(updatedUser.Password, updatedUserInDb.Password);
        }
    }
}
