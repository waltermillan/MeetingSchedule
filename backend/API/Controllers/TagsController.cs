using API.Features.Tags.Create;
using API.Features.Tags.Delete;
using API.Features.Tags.GetAll;
using API.Features.Tags.GetById;
using API.Features.Tags.Update;
using API.Responses;
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

                var data = new
                {
                    tagId
                };

                return Ok(ApiResponseFactory.Success<object>(data, "Tag Created Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while creating the tag. Message: {0}", ex.Message)));
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
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving all tags. Message: {0}", ex.Message)));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var tag = await _mediator.Send(new GetByIdTagQuery(id));
                return tag is null ? NotFound() : Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving the tag. Message: {0}", ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTagCommand command)
        {
            try
            {
                if (id != command.Id) 
                    return BadRequest(ApiResponseFactory.Fail<object>("Mismatched ID"));

                var updated = await _mediator.Send(command);

                var data = new
                {
                    updated
                };

                return Ok(ApiResponseFactory.Success<object>(data, "Tags Updated Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while updating the tag. Message: {0}", ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteTagCommand(id));

                var data = new
                {
                    deleted
                };

                return Ok(ApiResponseFactory.Success<object>(data, "Tag Deleted Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while deleting the tag. Message: {0}", ex.Message)));
            }
        }
    }
}
