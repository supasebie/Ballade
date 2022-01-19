using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
  public class UserDto
  {
    public string Username { get; set; }
    public string Token { get; set; }
    public string PhotoUrl { get; set; }
    public string KnownAs { get; set; }
  }

  public class UserRegisterDTO
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    public string KnownAs { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
  }
}