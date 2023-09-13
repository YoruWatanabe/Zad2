using University.Models;
using Microsoft.EntityFrameworkCore;
using University.Services;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Exam> Exams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("UniversityDb");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().Ignore(s => s.IsSelected);

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = "1", Name = "Wieńczysław", LastName = "Nowakowicz", PESEL = "PESEL1", BirthDate = new DateTime(1987, 05, 22), PlaceOfBirth = "Warszawa", AddressLine1 = "ul. Długa 1", AddressLine2 = "", PlaceOfResidence = "Warszawa", Courses = new List<Course>() },
                new Student { StudentId = "2", Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25), PlaceOfBirth = "Wrocław", AddressLine1 = "ul. Krótka 20", AddressLine2 = "", PlaceOfResidence = "Kraków", Courses = new List<Course>() },
                new Student { StudentId = "3", Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08), PlaceOfBirth = "Poznań", AddressLine1 = "ul. Kolorowa 8", AddressLine2 = "", PlaceOfResidence = "Gdanśk", Courses = new List<Course>() });

            modelBuilder.Entity<Student>().HasKey(x => x.StudentId);

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseCode = "MAT",
                    Title = "Matematyka",
                    Instructor = "Marta Kowalska",
                    Schedule = "schedule1",
                    Description = "desc",
                    Credits = 5,
                    Department = "dep1" },
                new Course { CourseCode = "BIOL",
                    Title = "Biologia",
                    Instructor = "Halina Katowice",
                    Schedule = "schedule2",
                    Description = "desc",
                    Credits = 6,
                    Department = "dep3" },
                new Course { CourseCode = "CHEM",
                    Title = "Chemia",
                    Instructor = "Jan Nowak",
                    Schedule = "schedule3",
                    Description = "desc",
                    Credits = 7,
                    Department = "dep3" }
            );
            modelBuilder.Entity<Course>().HasKey(x => x.CourseCode);
            
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = "B0001",
                    Title = "It",
                    Author = "Stephen King",
                    ISBN = "978-1501142970",
                    Publisher = "Scribner Book Company",
                    PublicationDate = new DateTime(2016, 1, 6),
                    Description = "Des...",
                    Genre = "Novel"
                },
                new Book {
                    BookId = "B0002",
                    Title = "Pet Sematary",
                    Author = "Stephen King",
                    ISBN = "978-1982112394",
                    Publisher = "Random House US",
                    PublicationDate = new DateTime(2018, 12, 4),
                    Description = "Des...",
                    Genre = "Novel"
                }
            );
            modelBuilder.Entity<Book>().HasKey(x => x.BookId);

            modelBuilder.Entity<Exam>().HasData( 
                new Exam {
                    ExamId = "E001",
                    CourseCode = "MAT",
                    Date = new DateTime(2024, 10, 11),
                    StartTime = new DateTime(2019, 1, 1, 10, 0, 0),
                    EndTime = new DateTime(2019, 1, 1, 12, 0, 0),
                    Description = "Finals",
                    Location = "Auditorium B",
                    Professor = "Marta Kowalska"
                } 
            );
            modelBuilder.Entity<Exam>().HasKey(x => x.ExamId);
        }
    }
}
