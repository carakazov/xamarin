using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

[Table("Roles")]
public class RoleEntity
{
    [Key]
    public int Id { get; set; }
    public String Title { get; set; }
}