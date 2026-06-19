using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieApi.Models;

// Фільм. Пов'язаний з режисером (Director).
public class Movie
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public int Year { get; set; }

    // Рейтинг 0..10
    public double Rating { get; set; }

    // Зовнішній ключ на режисера
    public int DirectorId { get; set; }

    [ForeignKey(nameof(DirectorId))]
    [JsonIgnore]
    public Director? Director { get; set; }
}
