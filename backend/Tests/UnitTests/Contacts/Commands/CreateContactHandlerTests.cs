using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Contacts.Commands
{
    public class CreateContactHandlerTests
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

            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Contacts", "ContactTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var contact = JsonSerializer.Deserialize<Contact>(json);
            return contact!;
        }



        [Fact]
        public async Task Add_ShouldAddNewContact()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new ContactRepository(context);

            var contact = LoadContactFromJson();
            contact.CreatedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;

            // Act
            repo.Add(contact);
            await context.SaveChangesAsync();

            var dbContact = await context.Contacts.FirstOrDefaultAsync(c => c.Email == contact.Email);

            // Assert
            Assert.NotNull(dbContact);
            Assert.Equal(contact.Name, dbContact.Name);
        }
    }
}
