using API.DTOs;
using Core.Interfaces;

namespace API.Services;

public class ContactTagService
{
    private readonly IContactTagRepository _contactTagRepository;
    private readonly IContactRepository _contactRepository;
    private readonly ITagRepository _tagRepository;

    public ContactTagService(IContactTagRepository contactTagRepository, IContactRepository contactRepository, ITagRepository tagRepository)
    {
        _contactTagRepository = contactTagRepository;
        _contactRepository = contactRepository;
        _tagRepository = tagRepository;
    }

    public async Task<ContactTagDTO> GetByIdAsync(Guid contactTagId)
    {
        var contactTag = await _contactTagRepository.GetByIdAsync(contactTagId);
        var contact = await _contactRepository.GetByIdAsync(contactTag.ContactId);
        var tag = await _tagRepository.GetByIdAsync(contactTag.TagId);

        if (contact is null)
            return null;

        var contactTagDTO = new ContactTagDTO
        {
            Id = contactTag.Id,
            ContactId = contact.Id,
            Contact = contact.Name,
            TagId = tag.Id,
            Tag = tag.Name
        };

        return contactTagDTO;
    }

    public async Task<IEnumerable<ContactTagDTO>> GetAllAsync()
    {
        var contactTags = await _contactTagRepository.GetAllAsync();

        if (contactTags is null || !contactTags.Any())
            return [];

        var contactTagsDTO = new List<ContactTagDTO>();

        foreach (var item in contactTags)
        {
            var contact = await _contactRepository.GetByIdAsync(item.ContactId);
            var tag = await _tagRepository.GetByIdAsync(item.TagId);
            var contactTagDTO = new ContactTagDTO
            {
                Id = item.Id,
                ContactId = contact.Id,
                Contact = contact.Name,
                TagId = tag.Id,
                Tag = tag.Name
            };

            contactTagsDTO.Add(contactTagDTO);
        }

        return contactTagsDTO;
    }
}
