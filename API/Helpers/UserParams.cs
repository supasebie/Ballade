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
  public class UserParams : PaginationParams
  {
    public string CurrentUsername { get; set; }
    public string Predicate { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 1;
    public int MaxAge { get; set; } = 150;
    public OrderType OrderBy { get; set; } = OrderType.LastActive;
  }
}