using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Authorize]
  public class LikesController : BaseApiController
  {
    private readonly ILikesRepository _likesRepository;
    private readonly IUserRepository _userRepository;
    public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
    {
      _userRepository = userRepository;
      _likesRepository = likesRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
      var sourceUserId = User.GetUserId();
      var likedUser = await _userRepository.GetUserByUsernameAsync(username);
      var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

      if (likedUser == null) return NotFound();

      if (sourceUser.UserName == username) return BadRequest("We like you too!");

      var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

      if (userLike != null) return BadRequest("You already like this user");

      userLike = new UserLike
      {
        SourceUserId = sourceUserId,
        LikedUserId = likedUser.Id
      };

      sourceUser.LikedUsers.Add(userLike);

      if (await _userRepository.SaveAllAsync())
      {
        return Ok();
      }

      return BadRequest("System error");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes([FromQuery] UserParams userParams)
    {
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      userParams.CurrentUsername = user.UserName;

      var users = await _likesRepository.GetUserLikes(userParams, User.GetUserId());

      Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

      if (users != null)
        return Ok(users);

      return BadRequest("Bad request");
    }
  }
}