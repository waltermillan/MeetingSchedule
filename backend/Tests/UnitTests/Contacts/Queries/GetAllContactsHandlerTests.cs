using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace Tests.UnitTests.Contacts.Queries
{
    public class GetAllContactsHandlerTests
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

        private List<Contact> LoadContactsFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Contacts", "GetAllContactsTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(json);
            return contacts!;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllContacts()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new ContactRepository(context);

            var contacts = LoadContactsFromJson();

            foreach (var c in contacts)
            {
                c.CreatedAt = DateTime.UtcNow;
                c.UpdatedAt = DateTime.UtcNow;
            }

            context.Contacts.AddRange(contacts);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Alice");
            Assert.Contains(result, c => c.Email == "bob@example.com");
        }
    }
}
