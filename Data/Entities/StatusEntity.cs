using System.ComponentModel.DataAnnotations;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace Data.Entities;

[Index(nameof(StatustName), IsUnique =  true)]
public class StatusEntity
    {
    [Key]
    public int Id { get; set; }
    public string StatustName { get; set; } = null!;
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];

    }