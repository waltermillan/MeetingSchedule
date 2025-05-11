using API.Features.Contacts.Create;
using API.Features.Contacts.Delete;
using API.Features.Contacts.GetAll;
using API.Features.Contacts.GetById;
using API.Features.Contacts.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContactsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactCommand command)
        {
            var contactId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = contactId }, contactId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _mediator.Send(new GetAllContactsQuery());
            return Ok(contacts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contact = await _mediator.Send(new GetContactByIdQuery(id));
            if (contact is null) return NotFound();
            return Ok(contact);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched ID");
            var updated = await _mediator.Send(command);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteContactCommand(id));
            return deleted ? NoContent() : NotFound();
        }
    }
}