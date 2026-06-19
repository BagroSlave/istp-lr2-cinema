using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieApi.Models;

// Режисер. Доменна модель є прикладом і може бути змінена.
public class Director
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    // Навігаційна властивість: фільми цього режисера.
    // JsonIgnore — щоб уникнути циклічної серіалізації у відповіді API.
    [JsonIgnore]
    public List<Movie> Movies { get; set; } = new();
}
