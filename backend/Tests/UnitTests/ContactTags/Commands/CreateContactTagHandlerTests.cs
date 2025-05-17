using API.Features.ContactTags.Create;
using API.Features.ContactTags.CreateContactTag;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System.Text.Json;

namespace Tests.UnitTests.ContactTags.Commands
{
    public class CreateContactTagHandlerTests
    {
        private CreateContactTagCommand LoadCommandFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "ContactTags", "CreateContactTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var command = JsonSerializer.Deserialize<CreateContactTagCommand>(json, options);

            return command!;
        }

        [Fact]
        public async Task Handle_ShouldCreateContactTag_AndReturnId()
        {
            // Arrange
            var command = LoadCommandFromJson();

            var fakeRepo = new Mock<IContactTagRepository>();
            var fakeUow = new Mock<IUnitOfWork>();

            fakeUow.Setup(u => u.ContactTags).Returns(fakeRepo.Object);
            fakeUow.Setup(u => u.SaveAsync(default)).ReturnsAsync(1);

            var handler = new CreateContactTagHandler(fakeUow.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.IsType<Guid>(result);
            fakeRepo.Verify(r => r.Add(It.Is<ContactTag>(
                x => x.ContactId == command.ContactId && x.TagId == command.TagId)), Times.Once);

            fakeUow.Verify(u => u.SaveAsync(default), Times.Once);
        }
    }
}
