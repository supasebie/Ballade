using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class LikesRepository : ILikesRepository
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public LikesRepository(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
      return await _context.Likes.FindAsync(sourceUserId, likedUserId);
    }

    public async Task<PagedList<LikeDTO>> GetUserLikes(UserParams userParams, int userId)
    {
      if (string.IsNullOrEmpty(userParams.Predicate) == true)
      {
        return null;
      }
      var users = _context.Users.AsQueryable();
      var likes = _context.Likes.AsQueryable();

      if (userParams.Predicate == "liked")
      {
        likes = likes.Where(like => like.SourceUserId == userId);
        users = likes.Select(like => like.LikedUser);
      }

      if (userParams.Predicate == "likedBy")
      {
        likes = likes.Where(like => like.LikedUserId == userId);
        users = likes.Select(like => like.SourceUser);
      }

      users = OrderBy(users, userParams);

      return await PagedList<LikeDTO>.CreateAsync(users.ProjectTo<LikeDTO>(_mapper.ConfigurationProvider).AsNoTracking(),
                                              userParams.PageNumber, userParams.PageSize);
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
      return await _context.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public IQueryable<AppUser> OrderBy(IQueryable<AppUser> userQuery, UserParams userParams)
    {
      return userParams.OrderBy.ToString() switch
      {
        "Age" => userQuery.OrderByDescending(u => u.DateOfBirth),
        "Name" => userQuery.OrderBy(u => u.KnownAs),
        _ => userQuery.OrderBy(u => u.LastActive)
      };
    }
  }
}