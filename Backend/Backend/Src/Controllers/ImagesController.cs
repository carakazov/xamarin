using Backend.Model;
using Backend.RestDto;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly ApplicationContext.ApplicationContext context;

    public ImagesController(ApplicationContext.ApplicationContext context)
    {
        this.context = context;
    }
    
    [HttpPost]
    public IActionResult RecognizeImage([FromBody] RecognitionRequest request, [FromHeader] string authorization)
    {
        string login = TokenUtils.CheckToken(authorization);
        if (login == "Error")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        UserEntity userEntity = context.Users.First(user => user.Login == login);
        if (userEntity.Status == "Banned")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        string result = ImageRecognitionUtil.RecognizeImage(request.Base64Image);
        HistoryEntity historyEntity = new HistoryEntity()
        {
            Image = request.Base64Image,
            Result = result,
            User = userEntity
        };
        context.History.Add(historyEntity);
        context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK, new RecognitionResponse() { Result = result });
    }
}