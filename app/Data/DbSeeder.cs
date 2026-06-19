using MovieApi.Models;

namespace MovieApi.Data;

// Початкове наповнення бази демонстраційними даними.
public static class DbSeeder
{
    public static void Seed(MovieContext context)
    {
        if (context.Movies.Any())
        {
            return; // Дані вже є.
        }

        var nolan = new Director { Name = "Крістофер Нолан" };
        var villeneuve = new Director { Name = "Дені Вільньов" };

        context.Directors.AddRange(nolan, villeneuve);

        context.Movies.AddRange(
            new Movie { Title = "Початок", Year = 2010, Rating = 8.8, Director = nolan },
            new Movie { Title = "Інтерстеллар", Year = 2014, Rating = 8.7, Director = nolan },
            new Movie { Title = "Дюна", Year = 2021, Rating = 8.0, Director = villeneuve },
            new Movie { Title = "Той, що біжить по лезу 2049", Year = 2017, Rating = 8.0, Director = villeneuve }
        );

        context.SaveChanges();
    }
}
