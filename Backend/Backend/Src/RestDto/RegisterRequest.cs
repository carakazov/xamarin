namespace Backend.RestDto;

public class RegisterRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}