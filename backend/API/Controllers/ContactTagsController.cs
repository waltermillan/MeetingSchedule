using API.Features.ContactTags.Create;
using API.Features.ContactTags.Delete;
using API.Features.ContactTags.GetAll;
using API.Features.ContactTags.GetById;
using API.Features.ContactTags.Update;
using API.Responses;
using Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContactTagsController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactTagCommand command)
        {
            try
            {
                var contactTagId = await mediator.Send(command);

                var data = new
                {
                    contactTagId
                };

                return Ok(ApiResponseFactory.Success<object>(data, ContactTagMessages.CreationSuccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactTagMessages.CreationFailure, ex.Message)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await mediator.Send(new GetAllContactTagsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactTagMessages.RetrievalAllFailure, ex.Message)));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await mediator.Send(new GetByIdContactTagQuery(id));
                return result is null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactTagMessages.RetrievalByIdFailure, ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactTagCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest(ContactTagMessages.IdMismatch);

                var updated = await mediator.Send(command);

                var data = new
                {
                    updated
                };

                return Ok(ApiResponseFactory.Success<object>(data, ContactTagMessages.UpdateSuccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactTagMessages.UpdateFailure, ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await mediator.Send(new DeleteContactTagCommand(id));

                var data = new
                {
                    deleted
                };

                return Ok(ApiResponseFactory.Success<object>(data, ContactTagMessages.DeleteSuccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(ContactTagMessages.DeleteFailure, ex.Message)));
            }
        }
    }
}