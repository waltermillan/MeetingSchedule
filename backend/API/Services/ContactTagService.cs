using API.DTOs;
using Core.Interfaces;

namespace API.Services
{
    public class ContactTagService(
        IContactTagRepository contactTagRepository,
        IContactRepository contactRepository,
        ITagRepository tagRepository)
    {
        private readonly IContactTagRepository _contactTagRepository = contactTagRepository;
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<ContactTagDto> GetByIdAsync(Guid contactTagId)
        {
            var contactTag = await _contactTagRepository.GetByIdAsync(contactTagId)
                              ?? throw new KeyNotFoundException($"ContactTag with ID {contactTagId} not found.");

            var contact = await _contactRepository.GetByIdAsync(contactTag.ContactId)
                           ?? throw new KeyNotFoundException($"Contact with ID {contactTag.ContactId} not found.");

            var tag = await _tagRepository.GetByIdAsync(contactTag.TagId)
                       ?? throw new KeyNotFoundException($"Tag with ID {contactTag.TagId} not found.");

            return new ContactTagDto
            {
                Id = contactTag.Id,
                ContactId = contact.Id,
                Contact = contact.Name,
                TagId = tag.Id,
                Tag = tag.Name,
                CreatedAt = contactTag.CreatedAt,
                UpdatedAt = contactTag.UpdatedAt
            };
        }

        public async Task<IEnumerable<ContactTagDto>> GetAllAsync()
        {
            var contactTags = await _contactTagRepository.GetAllAsync();

            if (contactTags is null || !contactTags.Any())
                return [];

            var contactTagsDTO = new List<ContactTagDto>();

            foreach (var item in contactTags)
            {
                var contact = await _contactRepository.GetByIdAsync(item.ContactId);
                var tag = await _tagRepository.GetByIdAsync(item.TagId);

                if (contact is null || tag is null)
                    continue;

                contactTagsDTO.Add(new ContactTagDto
                {
                    Id = item.Id,
                    ContactId = contact.Id,
                    Contact = contact.Name,
                    TagId = tag.Id,
                    Tag = tag.Name,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }

            return contactTagsDTO;
        }
    }
}