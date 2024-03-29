using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _mapper = mapper;
      _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserRegisterDTO userRegisterDTO)
    {
      if (await UserExists(userRegisterDTO.Username)) return BadRequest("Error creating user");

      var user = _mapper.Map<AppUser>(userRegisterDTO);

      user.UserName = userRegisterDTO.Username.ToLower();

      var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);

      if(!result.Succeeded) return BadRequest(result.Errors);

      var roleResult = await _userManager.AddToRoleAsync(user, "Member");

      if(!roleResult.Succeeded) return BadRequest(result.Errors);

      return new UserDto
      {
        Username = user.UserName,
        Token = await _tokenService.CreateToken(user),
        KnownAs = user.KnownAs,
        Gender = user.Gender
      };
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _userManager.Users
        .Include(x => x.Photos)
        .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

      if (user == null) return Unauthorized("Invalid user");

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
      
      if(!result.Succeeded) return Unauthorized();
    
      return new UserDto
      {
        Username = user.UserName,
        Token = await _tokenService.CreateToken(user),
        PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
        KnownAs = user.KnownAs,
        Gender = user.Gender
      };
    }

    private async Task<bool> UserExists(string username)
    {
      return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
  }
}