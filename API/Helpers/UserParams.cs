using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
  public enum OrderType{
    LastActive,
    Name,
    Age
  }
  public class UserParams
  {
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 0;
    private int _pageSize = 10;

    public int PageSize
    {
      get => _pageSize;
      set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string CurrentUsername { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 1;
    public int MaxAge { get; set; } = 150;
    public OrderType OrderBy { get; set; } = OrderType.LastActive;
  }
}