using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Contacts.Commands
{
    public class DeleteContactHandlerTests
    {
        private static async Task<Context> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new Context(options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        private Contact LoadContactFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Contacts", "DeleteContactTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var contact = JsonSerializer.Deserialize<Contact>(json);
            return contact!;
        }

        [Fact]
        public async Task Remove_ShouldDeleteContact()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new ContactRepository(context);

            var contact = LoadContactFromJson();
            contact.CreatedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;

            repo.Add(contact);
            await context.SaveChangesAsync();

            // Act
            repo.Remove(contact);
            await context.SaveChangesAsync();

            var deleted = await context.Contacts.FindAsync(contact.Id);

            // Assert
            Assert.Null(deleted);
        }
    }
}
