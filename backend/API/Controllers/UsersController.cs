using API.DTOs;
using API.Features.Users.Create;
using API.Features.Users.Delete;
using API.Features.Users.GetAll;
using API.Features.Users.GetById;
using API.Features.Users.Update;
using API.Responses;
using API.Services;
using Azure.Core;
using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly UserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public UsersController(IMediator mediator, UserService userService, IPasswordHasher passwordHasher)
        {
            _mediator = mediator;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userName = request.UserName.ToUpper();
            var password = request.Password;

            try
            {
                var user = await _userService.GetByNameAsync(userName);

                if (user is null || !_passwordHasher.VerifyPassword(password, user.Password))
                {
                    return Unauthorized(new { Code = 401, Message = "Invalid UserName Or Password." });
                }

                var data = new
                {
                    user.Id,
                    user.Name,
                    user.UserName
                };

                return Ok(ApiResponseFactory.Success<object>(data, "User Authenticated Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("Authentication Error. Message: {0}", ex.Message)));
            }
        }

            [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            try
            {
                var tagId = await _mediator.Send(command);
                
                var data = CreatedAtAction(nameof(GetById), new { id = tagId }, tagId);

                return Ok(ApiResponseFactory.Success<object>(data, "User Created Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while creating the user. Message: {0}", ex.Message)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving all users. Message: {0}", ex.Message)));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _mediator.Send(new GetByIdUserQuery(id));
                return user is null ? NotFound() : Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while retrieving the user. Message: {0}", ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id) return BadRequest("Mismatched ID");
                var updated = await _mediator.Send(command);

                return Ok(ApiResponseFactory.Success<object>(updated, "User Updated Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while updating the user. Message: {0}", ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteUserCommand(id));

                return Ok(ApiResponseFactory.Success<object>(deleted, "User Deleted Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(string.Format("An error occurred while deleting the user. Message: {0}", ex.Message)));
            }
        }
    }
}
