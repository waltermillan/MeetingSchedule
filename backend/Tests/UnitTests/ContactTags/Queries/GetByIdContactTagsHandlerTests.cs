using API.Features.ContactTags.GetById;
using API.Services;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System.Text.Json;

namespace Tests.UnitTests.ContactTags.Queries
{
    public class GetByIdContactTagsHandlerTests
    {
        // Helper to load test data from a JSON file
        private ContactTag LoadContactTagDataFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "ContactTags", "GetByIdContactTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<ContactTag>>(json);
            return data![0];
        }

        [Fact]
        public async Task Handle_ReturnsContactTagDto_WhenContactTagExists()
        {
            // Arrange
            var contactTag = LoadContactTagDataFromJson();

            var contact = new Contact { Id = contactTag.ContactId, Name = "Juan" };
            var tag = new Tag { Id = contactTag.TagId, Name = "Amigo" };

            var mockContactTagRepo = new Mock<IContactTagRepository>();
            var mockContactRepo = new Mock<IContactRepository>();
            var mockTagRepo = new Mock<ITagRepository>();

            mockContactTagRepo.Setup(r => r.GetByIdAsync(contactTag.Id)).ReturnsAsync(contactTag);
            mockContactRepo.Setup(r => r.GetByIdAsync(contact.Id)).ReturnsAsync(contact);
            mockTagRepo.Setup(r => r.GetByIdAsync(tag.Id)).ReturnsAsync(tag);

            var service = new ContactTagService(
                mockContactTagRepo.Object,
                mockContactRepo.Object,
                mockTagRepo.Object
            );

            var handler = new GetByIdContactTagHandler(service);

            // Act
            var result = await handler.Handle(new GetByIdContactTagQuery(contactTag.Id), default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactTag.Id, result.Id);
            Assert.Equal(contact.Id, result.ContactId);
            Assert.Equal(contact.Name, result.Contact);
            Assert.Equal(tag.Id, result.TagId);
            Assert.Equal(tag.Name, result.Tag);
        }
    }
}
