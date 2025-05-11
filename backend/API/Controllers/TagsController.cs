using API.Features.Tags.Create;
using API.Features.Tags.Delete;
using API.Features.Tags.GetAll;
using API.Features.Tags.GetById;
using API.Features.Tags.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class TagsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
        {
            var tagId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = tagId }, tagId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _mediator.Send(new GetAllTagsQuery());
            return Ok(tags);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tag = await _mediator.Send(new GetTagByIdQuery(id));
            return tag is null ? NotFound() : Ok(tag);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTagCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched ID");
            var updated = await _mediator.Send(command);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteTagCommand(id));
            return deleted ? NoContent() : NotFound();
        }
    }
}