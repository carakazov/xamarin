using Backend.Model;
using Backend.RestDto;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly ApplicationContext.ApplicationContext context;

    public HistoryController(ApplicationContext.ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult GetMyHistory([FromHeader] string authorization)
    {
        string login = TokenUtils.CheckToken(authorization);
        if (login == "Error")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        UserEntity userEntity = context.Users.Include(user => user.History).First(user => user.Login == login);
        if (userEntity.Status == "Banned")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        List<HistoryDto> result = new List<HistoryDto>();
        userEntity.History.ForEach(history => result.Add(new HistoryDto()
        {
            Image = history.Image,
            Result = history.Result
        }));

        return StatusCode(StatusCodes.Status200OK, result);
    }
}