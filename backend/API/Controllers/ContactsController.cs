using API.Features.Contacts.Create;
using API.Features.Contacts.Delete;
using API.Features.Contacts.GetAll;
using API.Features.Contacts.GetById;
using API.Features.Contacts.Update;
using API.Responses;
using Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    public class ContactsController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactCommand command)
        {
            try
            {
                var contactId = await mediator.Send(command);

                var data = new
                {
                    contactId
                };

                Log.Information($"Contact: {command.Name} created successfully.");
                return Ok(ApiResponseFactory.Success<object>(data, ContactMessages.CreationSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating contact. \nName: {command.Name}. \nException: {ex.InnerException}");

                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactMessages.CreationFailure, ex.Message)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contacts = await mediator.Send(new GetAllContactsQuery());
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting contacts. \nException: {ex.InnerException}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactMessages.RetrievalAllFailure, ex.Message)));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var contact = await mediator.Send(new GetByIdContactQuery(id));
                return contact is null ? NotFound() : Ok(contact);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting contact. ID Contact: {id} \nException: {ex.InnerException}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactMessages.RetrievalByIdFailure, ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest(ContactMessages.IdMismatch);

                var updated = await mediator.Send(command);

                var data = new
                {
                    updated
                };

                Log.Information($"Contact: {command.Name} updated successfully.");

                return Ok(ApiResponseFactory.Success<object>(data, ContactMessages.UpdateSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating contact. \nName: {command.Name}. \nException: {ex.InnerException}");

                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactMessages.UpdateFailure, ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await mediator.Send(new DeleteContactCommand(id));

                var data = new
                {
                    deleted
                };

                Log.Information($"ID Contact: {id} deleted successfully.");

                return Ok(ApiResponseFactory.Success<object>(data, ContactMessages.DeleteSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting contact. ID Contact: {id} \nException: {ex.InnerException}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactMessages.DeleteFailure, ex.Message)));
            }
        }
    }
}
