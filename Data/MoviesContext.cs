using Assignment3MovieApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment3MovieApi.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() { }

        // Constructor to use for injecting connection string
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Franchise> Franchises { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 1, Name = "Warner Bros", Description = "Big movie company" });
            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 2, Name = "Paramount Pictures", Description = "Another big movie company" });

            modelBuilder.Entity<Movie>().HasData(new Movie { Id = 1, MovieTitle = "Seven", Director = "David Fincher", ReleaseYear = 1995, Genre = "crime, thriller", Picture = "https://www.imageurl.com", Trailer = "https://youtube.com/someUrl", FranchiseId = 1 });
            modelBuilder.Entity<Movie>().HasData(new Movie { Id = 2, MovieTitle = "Iron Man", Director = "Jon Faveau", ReleaseYear = 2008, Genre = "superhero, action", Picture = "https://www.imageurl.com", Trailer = "https://youtube.com/someUrl", FranchiseId = 2 });
            modelBuilder.Entity<Movie>().HasData(new Movie { Id = 3, MovieTitle = "Top Gun: Maverick", Director = "Tony Scott", ReleaseYear = 2022, Genre = "action", Picture = "https://www.imageurl.com", Trailer = "https://youtube.com/someUrl", FranchiseId = 1 });

            modelBuilder.Entity<Character>().HasData(new Character { Id = 1, FullName = "Capt. Pete Mitchell", Alias = "Maverick", Gender = "Male", });
            modelBuilder.Entity<Character>().HasData(new Character { Id = 2, FullName = "Tony Stark", Alias = "Iron Man", Gender = "Male" });
            modelBuilder.Entity<Character>().HasData(new Character { Id = 3, FullName = "David Mills", Gender = "Male" });
            modelBuilder.Entity<Character>().HasData(new Character { Id = 4, FullName = "Detective William Sommerset", Gender = "Male" });

        }
    }
}
