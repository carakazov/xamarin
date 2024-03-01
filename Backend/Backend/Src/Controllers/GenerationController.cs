using Backend.Model;
using Backend.RestDto;
using Backend.Src.RestDto;
using Backend.Src.Utils;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class GenerationController : ControllerBase
{
    private readonly ApplicationContext.ApplicationContext context;

    public GenerationController(ApplicationContext.ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult GetMyGeneration([FromHeader] string authorization)
    {
        string login = TokenUtils.CheckToken(authorization);
        if (login == "Error")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        UserEntity userEntity = context.Users.Include(user => user.Generation).First(user => user.Login == login);
        if (userEntity.Status == "Banned")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        List<GenerationDto> result = new List<GenerationDto>();
        userEntity.Generation.ForEach(generation => result.Add(new GenerationDto()
        {
            Id = generation.Id,
            Image = generation.Image,
            Promt = generation.Promt,
            Rating = generation.Rating,
            IsPublic = generation.IsPublic
        }));

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    public IActionResult GenerateImage([FromBody] GenerationRequest request, [FromHeader] string authorization)
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

        string result = ImageGeneration.Generate(request.Promt);
        GenerationEntity generationEntity = new GenerationEntity()
        {
            Image = result,
            Promt = request.Promt,
            User = userEntity,
            Rating = 0,
            IsPublic = false

        };
        context.Generation.Add(generationEntity);
        context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK, new GenerationResponse() { Base64Image = result, Id = context.Generation.Count() });

    }

    [HttpPost("RateImg")]
    public IActionResult RateImage([FromBody] RateImgDto request, [FromHeader] string authorization)
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

        GenerationEntity generation = context.Generation.First(user => user.Id == request.Id );
        generation.Rating = request.Rate;
        context.Generation.Update(generation);
        context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK);
    }

    [HttpPost("PublicView")]
    public IActionResult PublicView([FromBody] RateImgDto request, [FromHeader] string authorization)
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

        GenerationEntity status = context.Generation.First(user => user.Id == request.Id);
        status.IsPublic = !status.IsPublic;
        context.Generation.Update(status);
        context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK);
    }


    [HttpGet("AllPublicView")]
    public IActionResult AllPublicView([FromHeader] string authorization)
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

        List<GenerationEntity> visible = context.Generation.Where(a => a.IsPublic).ToList();
        List<GenerationDto> visibleDto = new List<GenerationDto>();
       visible.ForEach(generation => visibleDto.Add(new GenerationDto()
        {
            Id = generation.Id,
            Image = generation.Image,
            Promt = generation.Promt,
            Rating = generation.Rating,
            IsPublic = generation.IsPublic
        }));
        return StatusCode(StatusCodes.Status200OK, visibleDto);
}
}

