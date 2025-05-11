using API.Features.ContactsTag.Create;
using API.Features.ContactsTag.Delete;
using API.Features.ContactsTag.GetAll;
using API.Features.ContactsTag.GetById;
using API.Features.ContactsTag.Update;
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
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllContactsTagQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetContactTagByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactTagCommand command)
    {
        if (id != command.Id) return BadRequest("Mismatched ID");
        var updated = await _mediator.Send(command);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _mediator.Send(new DeleteContactTagCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
