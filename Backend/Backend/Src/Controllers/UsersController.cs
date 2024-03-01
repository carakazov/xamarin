using Backend.Model;
using Backend.RestDto;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationContext.ApplicationContext context;

    public UsersController(ApplicationContext.ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet("test")]
    public string Test()
    {
        return "test";
    }
    

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        List<UserEntity> users = context.Users.ToList();
        if (users.Find(user => user.Login.Equals(request.Login)) != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse() { Text = "Такой логин уже существует" });
        }

        UserEntity newUser = new UserEntity()
        {
            Name = request.Name,
            Surname = request.Surname,
            Login = request.Login,
            Password = request.Password,
            Status = "Access"
        };
        RoleEntity roleUser = context.Roles.First(role => role.Title == "ROLE_USER");
        newUser.Role = roleUser;
        context.Users.Add(newUser);
        context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK, new User()
        {
            Name = newUser.Name,
            Surname = newUser.Surname,
            Login = newUser.Login,
            Status = newUser.Status
        });
    }

    [HttpPost("auth")]
    public IActionResult Auth(AuthRequest request)
    {
        UserEntity existingUser = context.Users.Include(user => user.Role).FirstOrDefault(user => user.Login == request.Login);
        if (existingUser == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized,
                new ErrorResponse() { Text = "Неверный логин или пароль" });
        }

        if (existingUser.Password != request.Password)
        {
            return StatusCode(StatusCodes.Status401Unauthorized,
                new ErrorResponse() { Text = "Неверный логин или пароль" });
        }

        if (existingUser.Status != "Banned")
        {
            return StatusCode(StatusCodes.Status200OK, new AuthResponse()
            {
                Token = TokenUtils.EncodeToToken(existingUser)
            });   
        }
     
        return StatusCode(StatusCodes.Status401Unauthorized,
            new ErrorResponse() { Text = "Неверный логин или пароль" });
    }

    [HttpGet("all")]
    public IActionResult GetAllUsers([FromHeader] string authorization)
    {
        string login = TokenUtils.CheckToken(authorization);
        if (login == "Error")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        UserEntity userEntity = context.Users.Include(user => user.Role).First(user => user.Login == login);
        if (userEntity.Role.Title != "ROLE_ADMIN")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        List<UserForAdmin> users = new List<UserForAdmin>();
        List<UserEntity> allUsers = context.Users.Where(user => user.Login != login).ToList();
        allUsers.ForEach(user => users.Add(new UserForAdmin()
        {
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Status = user.Status,
            Id = user.Id
        }));
        return StatusCode(StatusCodes.Status200OK, users);
    }

    [HttpPut]
    public IActionResult ChangeUserStatus([FromBody] ChangeStatusRequest request, [FromHeader] string authorization)
    {
        string login = TokenUtils.CheckToken(authorization);
        UserEntity admin = context.Users.Include(user => user.Role).First(user => user.Login == login);

        if (admin.Role.Title != "ROLE_ADMIN")
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        UserEntity user = context.Users.Where(u => u.Id == request.UserId).First();
        if (user.Status == "Access")
        {
            user.Status = "Banned";
        }
        else
        {
            user.Status = "Access";
        }

        context.Users.Update(user);
        context.SaveChanges();
        
        return StatusCode(StatusCodes.Status200OK);
    }
}