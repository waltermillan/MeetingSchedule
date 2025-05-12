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
            try
            {
                var contactId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = contactId }, contactId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the contact.", Detailed = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contacts = await _mediator.Send(new GetAllContactsQuery());
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving all contacts.", Detailed = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var contact = await _mediator.Send(new GetContactByIdQuery(id));
                return contact is null ? NotFound() : Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the contact.", Detailed = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommand command)
        {
            try
            {
                if (id != command.Id) return BadRequest("Mismatched ID");
                var updated = await _mediator.Send(command);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the contact.", Detailed = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteContactCommand(id));
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the contact.", Detailed = ex.Message });
            }
        }
    }
}
