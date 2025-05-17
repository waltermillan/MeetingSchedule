using API.DTOs;
using API.Features.ContactTags.GetAll;
using API.Services;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System.Text.Json;

namespace Tests.UnitTests.ContactTags.Queries
{
    public class GetAllContactTagsHandlerTests
    {
        // Helper to load test data from a JSON file
        private List<ContactTag> LoadContactTagDataFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "ContactTags", "GetAllContactTagsTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<ContactTag>>(json);
            return data!;
        }

        [Fact]
        public async Task Handle_ReturnsListOfContactTagDto()
        {
            // Arrange
            var fakeData = LoadContactTagDataFromJson();

            var contact1 = new Contact { Id = fakeData[0].ContactId, Name = "Juan" };
            var contact2 = new Contact { Id = fakeData[1].ContactId, Name = "Ana" };
            var tag1 = new Tag { Id = fakeData[0].TagId, Name = "Amigo" };
            var tag2 = new Tag { Id = fakeData[1].TagId, Name = "Trabajo" };

            var mockContactTagRepo = new Mock<IContactTagRepository>();
            var mockContactRepo = new Mock<IContactRepository>();
            var mockTagRepo = new Mock<ITagRepository>();

            mockContactTagRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeData);

            mockContactRepo.Setup(r => r.GetByIdAsync(contact1.Id)).ReturnsAsync(contact1);
            mockContactRepo.Setup(r => r.GetByIdAsync(contact2.Id)).ReturnsAsync(contact2);

            mockTagRepo.Setup(r => r.GetByIdAsync(tag1.Id)).ReturnsAsync(tag1);
            mockTagRepo.Setup(r => r.GetByIdAsync(tag2.Id)).ReturnsAsync(tag2);

            var service = new ContactTagService(
                mockContactTagRepo.Object,
                mockContactRepo.Object,
                mockTagRepo.Object
            );

            var handler = new GetAllContactTagsHandler(service);

            // Act
            var result = await handler.Handle(new GetAllContactTagsQuery(), default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeData.Count, result.Count());
            Assert.All(result, item => Assert.IsType<ContactTagDto>(item));
        }
    }
}
