using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class MessagesRepository : IMessagesRepository
  {
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public MessagesRepository(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public void AddMessage(Message message)
    {
      _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
      _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessage(int id)
    {
      return await _context.Messages
        .Include(u => u.Sender)
        .Include(u => u.Recipient)
        .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
    {
      var query = _context.Messages.OrderByDescending(m => m.DateRead).AsQueryable();

      if (messageParams.Container == "outbox")
      {
        query = _context.Messages.OrderByDescending(m => m.DateSent).AsQueryable();
      }

      query = messageParams.Container switch
      {
        "inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false),
        "outbox" => query.Where(u => u.Sender.UserName == messageParams.Username && u.SenderDeleted == false),
        _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null),
      };

      var messages = query.Select(dtoMessage => new MessageDTO()
      {
        Content = dtoMessage.Content,
        DateRead = dtoMessage.DateRead,
        DateSent = dtoMessage.DateSent,
        Id = dtoMessage.Id,
        RecipientDeleted = dtoMessage.RecipientDeleted,
        RecipientId = dtoMessage.RecipientId,
        RecipientPhotoUrl = dtoMessage.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url,
        RecipientUsername = dtoMessage.RecipientUsername,
        SenderDeleted = dtoMessage.SenderDeleted,
        SenderId = dtoMessage.SenderId,
        SenderPhotoUrl = dtoMessage.Sender.Photos.FirstOrDefault(x => x.IsMain).Url,
        SenderUsername = dtoMessage.SenderUsername
      });

      var pagedMessages = await PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

      //******************
      //** This didn't work because we transformed our data type from our anticipated 
      //** IQueryable<MessageDTO> to an IEnumerable<MessageDTO> and we could no longer
      //** invoke the PagedListCTOR
      //*****************
      // foreach (var message in pagedMessages)
      // {
      //   var messageToAdd = new MessageDTO
      //   {
      //     Id = message.Id,
      //     SenderUsername = message.SenderUsername,
      //     SenderId = message.SenderId,
      //     RecipientUsername = message.RecipientUsername,
      //     RecipientId = message.RecipientId,
      //     Content = message.Content,
      //     DateSent = message.DateSent,
      //     DateRead = message.DateRead,
      //     SenderDeleted = message.SenderDeleted,
      //     RecipientDeleted = message.RecipientDeleted
      //   };
      //   messages.Add(messageToAdd);
      // };

      return pagedMessages;
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername)
    {
      var messages = await _context.Messages
        // .Include(u => u.Sender.Photos.FirstOrDefault(x => x.IsMain).Url)
        // .Include(u => u.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url)
        .Include(u => u.Sender).ThenInclude(p => p.Photos)
        .Include(u => u.Recipient).ThenInclude(p => p.Photos)
        .Where(m => m.Recipient.UserName == currentUsername
                && m.RecipientDeleted == false
                && m.Sender.UserName == recipientUsername
                || m.Recipient.UserName == recipientUsername
                && m.Sender.UserName == currentUsername
        )
        .OrderBy(m => m.DateSent)
        .ToListAsync();

      var unreadMessages = messages.Where(m => m.DateRead == null
          && m.Recipient.UserName == currentUsername).ToList();

      if (unreadMessages.Any())
      {
        foreach (var message in unreadMessages)
        {
          message.DateRead = DateTime.Now;
        }

        await _context.SaveChangesAsync();
      }

      // return messages.Select(messageDTO => new MessageDTO() {
      //   Content = messageDTO.Content,
      //   SenderPhotoUrl= messageDTO.Sender.Photos.FirstOrDefault(x => x.IsMain).Url,
      //   RecipientPhotoUrl= messageDTO.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url
      // }); ECT ECT

      return _mapper.Map<IEnumerable<MessageDTO>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public void Update(Message message)
    {
      _context.Entry(message).State = EntityState.Modified;
    }
  }
}