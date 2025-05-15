using API.Features.ContactsTag.Create;
using API.Features.ContactsTag.Delete;
using API.Features.ContactsTag.GetAll;
using API.Features.ContactsTag.GetById;
using API.Features.ContactsTag.Update;
using API.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ContactTagsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ContactTagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactTagCommand command)
    {
        try
        {
            var contactTagId = await _mediator.Send(command);

            var data = new
            {
                contactTagId
            };

            return Ok(ApiResponseFactory.Success<object>(data, "Contact-Tag Created Successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while creating the contact-tag. Message: {0}", ex.Message)));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetAllContactsTagQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving all contact-tags. Message: {0}", ex.Message)));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetByIdContactTagQuery(id));
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving the contact tag. Message: {0}", ex.Message)));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactTagCommand command)
    {
        try
        {
            if (id != command.Id) return BadRequest("Mismatched ID");
            var updated = await _mediator.Send(command);

            var data = new
            {
                updated
            };

            return Ok(ApiResponseFactory.Success<object>(data, "Contact-Tag Updated Successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while updating the contact tag. Message: {0}", ex.Message)));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteContactTagCommand(id));

            var data = new
            {
                deleted
            };

            return Ok(ApiResponseFactory.Success<object>(data, "Contact-Tag Deleted Successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while deleting the contact tag. Message: {0}", ex.Message)));
        }
    }
}
