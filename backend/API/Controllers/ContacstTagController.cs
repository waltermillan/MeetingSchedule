using API.Features.ContactsTag.Create;
using API.Features.ContactsTag.Delete;
using API.Features.ContactsTag.GetAll;
using API.Features.ContactsTag.GetById;
using API.Features.ContactsTag.Update;
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
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the contact tag.", Detailed = ex.Message });
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
            return StatusCode(500, new { Message = "An error occurred while retrieving all contact tags.", Detailed = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetContactTagByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the contact tag.", Detailed = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactTagCommand command)
    {
        try
        {
            if (id != command.Id) return BadRequest("Mismatched ID");
            var updated = await _mediator.Send(command);
            return updated ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the contact tag.", Detailed = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteContactTagCommand(id));
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the contact tag.", Detailed = ex.Message });
        }
    }
}
