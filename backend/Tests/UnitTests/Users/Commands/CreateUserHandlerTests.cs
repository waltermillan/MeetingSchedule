using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Users.Commands
{
    public class CreateUserHandlerTests
    {
        // Helper to load test data from a JSON file
        private User LoadUserDataFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Users", "CreateUserTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<User>>(json);
            return data![0];
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
        public async Task Add_ShouldAddNewUser()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new UserRepository(context);
            var user = LoadUserDataFromJson();

            // Act
            repo.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var dbUser = await context.Users.FirstOrDefaultAsync(t => t.Name == "Francis");
            Assert.NotNull(dbUser);
            Assert.Equal("Francis", dbUser.Name);
            Assert.Equal("Blosson", dbUser.UserName);
            Assert.Equal("Blosson1243", dbUser.Password);
        }
    }
}
