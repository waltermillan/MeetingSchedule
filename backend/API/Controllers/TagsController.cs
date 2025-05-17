using API.Features.Tags.Create;
using API.Features.Tags.Delete;
using API.Features.Tags.GetAll;
using API.Features.Tags.GetById;
using API.Features.Tags.Update;
using API.Responses;
using Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TagsController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
        {
            try
            {
                var tagId = await mediator.Send(command);

                var data = new
                {
                    tagId
                };

                return Ok(ApiResponseFactory.Success<object>(data, TagMessages.CreationSuccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format(TagMessages.CreationFailure, ex.Message)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tags = await mediator.Send(new GetAllTagsQuery());
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format(TagMessages.RetrievalAllFailure, ex.Message)));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var tag = await mediator.Send(new GetByIdTagQuery(id));
                return tag is null ? NotFound() : Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format(TagMessages.RetrievalByIdFailure, ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTagCommand command)
        {
            try
            {
                if (id != command.Id) 
                    return BadRequest(ApiResponseFactory.Fail<object>(TagMessages.IdMismatch));

                var updated = await mediator.Send(command);

                var data = new
                {
                    updated
                };

                return Ok(ApiResponseFactory.Success<object>(data, TagMessages.UpdateSuccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format(TagMessages.UpdateFailure, ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await mediator.Send(new DeleteTagCommand(id));

                var data = new
                {
                    deleted
                };

                return Ok(ApiResponseFactory.Success<object>(data, TagMessages.DeleteSuccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format(TagMessages.DeleteFailure, ex.Message)));
            }
        }
    }
}
