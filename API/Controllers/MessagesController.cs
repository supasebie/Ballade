using System;
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
  public class MessagesController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    private readonly IMessagesRepository _messageRepository;
    public MessagesController(IUserRepository userRepository, IMessagesRepository messageRepository)
    {
      _messageRepository = messageRepository;
      _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult> CreateMessage([FromBody] MessageRequestDTO requestDTO)
    {
      var senderUsername = User.GetUsername();

      if (senderUsername == requestDTO.RecipientUsername) return BadRequest("You can't message yourself");

      var sender = await _userRepository.GetUserByUsernameAsync(senderUsername);

      var recipient = await _userRepository.GetUserByUsernameAsync(requestDTO.RecipientUsername);

      if (recipient == null) return NotFound("Recipient not found");

      var message = new Message
      {
        Content = requestDTO.Content,
        Sender = sender,
        SenderUsername = sender.UserName,
        Recipient = recipient,
        RecipientUsername = recipient.UserName,
      };

      _messageRepository.AddMessage(message);

      if (await _messageRepository.SaveAllAsync()) return Ok(new MessageDTO
      {
        Id = message.Id,
        Content = requestDTO.Content,
        SenderId = sender.Id,
        SenderUsername = sender.UserName,
        SenderPhotoUrl = sender.Photos.FirstOrDefault(x => x.IsMain).Url,
        RecipientId = recipient.Id,
        RecipientUsername = recipient.UserName,
        RecipientPhotoUrl = recipient.Photos.FirstOrDefault(x => x.IsMain).Url,
        DateSent = message.DateSent
      });

      return BadRequest("Unable to send message to user");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages([FromQuery] MessageParams messageParams)
    {
      messageParams.Username = User.GetUsername();

      var messages = await _messageRepository.GetMessagesForUser(messageParams);

      Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

      return messages;
    }

    [HttpGet("thread/{recipientUsername}")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string recipientUsername)
    {
      var messages = await _messageRepository.GetMessageThread(User.GetUsername(), recipientUsername);

      return Ok(messages);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DelteMessage(int id)
    {
      var user = User.GetUsername();

      var message = await _messageRepository.GetMessage(id);

      if (message.Sender.UserName != user && message.Recipient.UserName != user)
      {
        return Unauthorized();
      }

      if (message == null) { return BadRequest(); }

      if (message.Sender.UserName == user)
      {
        message.SenderDeleted = true;
      }
      else
      {
        message.RecipientDeleted = true;
      }

      if (message.SenderDeleted && message.RecipientDeleted)
      {
        _messageRepository.DeleteMessage(message);
      }

      if (await _messageRepository.SaveAllAsync())
      {
        return Ok();
      }
      return BadRequest();
    }
  }
}