using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests.Contacts.Queries
{
    public class GetByIdContactsHandlerTests
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
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Contacts", "GetByIdContactTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var contact = JsonSerializer.Deserialize<Contact>(json);
            return contact!;
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnContact()
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
            var found = await repo.GetByIdAsync(contact.Id);

            // Assert
            Assert.Equal(contact.Email, found.Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowIfNotFound()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new ContactRepository(context);

            var randomId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                repo.GetByIdAsync(randomId));

            Assert.Contains("Contact with ID", exception.Message);
        }
    }
}
