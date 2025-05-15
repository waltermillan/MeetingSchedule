using API.Features.Contacts.Create;
using API.Features.Contacts.Delete;
using API.Features.Contacts.GetAll;
using API.Features.Contacts.GetById;
using API.Features.Contacts.Update;
using API.Responses;
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

                var data = new
                {
                    contactId
                };

                return Ok(ApiResponseFactory.Success<object>(data, "Contact Created Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while creating the contact. Message: {0}", ex.Message)));
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
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving all contacts. Message: {0}", ex.Message)));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var contact = await _mediator.Send(new GetByIdContactQuery(id));
                return contact is null ? NotFound() : Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving the contact. Message: {0}", ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommand command)
        {
            try
            {
                if (id != command.Id) return BadRequest("Mismatched ID");
                var updated = await _mediator.Send(command);

                var data = new
                {
                    updated
                };

                return Ok(ApiResponseFactory.Success<object>(data, "Contact Updated Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while updating the contact. Message: {0}", ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteContactCommand(id));

                var data = new
                {
                    deleted
                };

                return Ok(ApiResponseFactory.Success<object>(data, "Contact deleted Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while deleting the contact. Message: {0}", ex.Message)));
            }
        }
    }
}
