using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

[Table("Users")]
public class UserEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
    
    public RoleEntity Role { get; set; }
    public List<HistoryEntity> History { get; set; }
    public List<GenerationEntity> Generation { get; set; }

}