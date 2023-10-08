using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApp.Api.Models;

public class Product: Auditable
{
    [Key]
    public int Id { get; set; }
    //[Required]
    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    public double? Price { get; set; } = 0;
}
