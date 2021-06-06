using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BuggyController : BaseApiController
  {
    private readonly DataContext _context;
    public BuggyController(DataContext context)
    {
      _context = context;
    }

    [HttpGet("auth")]
    [Authorize]
    public ActionResult<string> GetSecret()
    {
      return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
      var thing = _context.Users.Find(-1);
      if (thing == null) return NotFound();

      return Ok(thing);
    }
    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
      var thing = _context.Users.Find(-1);

      var thingToReturn = thing.ToString();

      return Ok(thing);
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
      return BadRequest();
    }

  }
}