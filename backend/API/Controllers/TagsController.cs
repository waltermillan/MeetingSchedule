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
            try
            {
                var tagId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = tagId }, tagId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the tag.", Detailed = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tags = await _mediator.Send(new GetAllTagsQuery());
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving all tags.", Detailed = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var tag = await _mediator.Send(new GetTagByIdQuery(id));
                return tag is null ? NotFound() : Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the tag.", Detailed = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTagCommand command)
        {
            try
            {
                if (id != command.Id) return BadRequest("Mismatched ID");
                var updated = await _mediator.Send(command);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the tag.", Detailed = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteTagCommand(id));
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the tag.", Detailed = ex.Message });
            }
        }
    }
}
