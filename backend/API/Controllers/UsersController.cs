﻿using API.DTOs;
using API.Features.Users.Create;
using API.Features.Users.Delete;
using API.Features.Users.GetAll;
using API.Features.Users.GetById;
using API.Features.Users.Update;
using API.Responses;
using API.Services;
using Core.Constants;
using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    public class UsersController(IMediator mediator, UserService userService, IPasswordHasher passwordHasher) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;
        private readonly UserService _userService = userService;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userName = request.UserName?.ToUpper();
            if (string.IsNullOrEmpty(userName))
            {
                Log.Error($"UserName is null or empty.");
                return BadRequest(UserMessages.LoginUserNameError);
            }

            var password = request.Password;

            try
            {
                var user = await _userService.GetByNameAsync(userName);

                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(user?.Password))
                {
                    Log.Error($"Unauthorized. Invalid user name or password. \nUserName: {userName}");
                    return Unauthorized(new { Code = 401, Message = UserMessages.LoginFailure });
                }

                if (!_passwordHasher.VerifyPassword(password, user.Password))
                {
                    Log.Error($"Unauthorized. Invalid password. \nUserName: {userName}");
                    return Unauthorized(new { Code = 401, Message = UserMessages.LoginFailure });
                }

                var data = new
                {
                    user.Id,
                    user.Name,
                    user.UserName
                };

                Log.Information($"User: {userName} authenticated successfully.");

                return Ok(ApiResponseFactory.Success<object>(data, UserMessages.LoginSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Authentication error. \nUserName: {userName}. \nError: {ex.Message}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(UserMessages.AuthError, ex.Message)));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            try
            {
                var tagId = await _mediator.Send(command);

                var data = CreatedAtAction(nameof(GetById), new { id = tagId }, tagId);

                Log.Information($"User: {command.UserName} created successfully.");

                return Ok(ApiResponseFactory.Success<object>(data, UserMessages.CreationSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating user. \nUserName: {command.UserName}. \nError: {ex.Message}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(UserMessages.CreationFailure, ex.Message)));
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
                Log.Error($"Error getting users. \nError: {ex.Message}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(UserMessages.RetrievalAllFailure, ex.Message)));
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
                Log.Error($"Error getting user. ID User: {id}. \nError: {ex.Message}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(UserMessages.RetrievalByIdFailure, ex.Message)));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest(UserMessages.IdMismatch);

                var updated = await _mediator.Send(command);

                Log.Information($"nUserName: {command.UserName} updated successfully.");

                return Ok(ApiResponseFactory.Success<object>(updated, UserMessages.UpdateSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating user. \nUserName: {command.UserName}. \nError: {ex.Message}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(UserMessages.UpdateFailure, ex.Message)));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteUserCommand(id));

                Log.Information($"ID User: {id} deleted successfully.");

                return Ok(ApiResponseFactory.Success<object>(deleted, UserMessages.DeleteSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting user. \nID User: {id}. \nError: {ex.Message}");
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    string.Format(UserMessages.DeleteFailure, ex.Message)));
            }
        }
    }
}
