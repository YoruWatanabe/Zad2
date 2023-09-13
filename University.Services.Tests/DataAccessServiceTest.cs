using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;
using University.Data;
using University.Interfaces;
using University.Models;
using System.Collections.ObjectModel;
using Telerik.JustMock;

namespace University.Services.Tests
{
    [TestClass]
    public class DataAccessServiceTest
    {
        private IDialogService _dialogService;
        private DbContextOptions<UniversityContext> _options;

        [TestInitialize()]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "UniversityTestDB")
                .Options;
            SeedTestDB();
            _dialogService = new DialogService();
        }

        private void SeedTestDB()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                context.Database.EnsureDeleted();

                List<Student> students = new List<Student>
                {
                    new Student { StudentId = "1", Name = "Wieńczysław", LastName = "Nowakowicz", PESEL="PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                    new Student { StudentId = "2", Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                    new Student { StudentId = "3", Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) }
                };

                List<Course> courses = new List<Course>
                {
                    new Course { CourseCode = "MAT",
                        Title = "Matematyka",
                        Instructor = "Michalina Beldzik",
                        Schedule = "schedule1",
                        Description = "desc",
                        Credits = 5,
                        Department = "dep1" },
                    new Course { CourseCode = "BIOL",
                        Title = "Biologia",
                        Instructor = "Halina",
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
                };

                List<Book> books = new List<Book>
                {
                    new Book { BookId = "B0001",
                        Title = "It",
                        Author = "Stephen King",
                        ISBN = "978-1501142970",
                        Publisher = "Scribner Book Company",
                        PublicationDate = new DateTime(2016, 1, 6),
                        Description = "Desc... ",
                        Genre = "Novel" },
                    new Book { BookId = "B0002",
                        Title = "Pet Sematary",
                        Author = "Stephen King",
                        ISBN = "978-1982112394",
                        Publisher = "Random House US",
                        PublicationDate = new DateTime(2018, 12, 4),
                        Description = "Desc... ",
                        Genre = "Novel" }
                };

                List<Exam> exams = new List<Exam>
                {
                    new Exam { ExamId = "E001",
                        CourseCode = "MAT",
                        Date = new DateTime(2024, 10, 11),
                        StartTime = new DateTime(2019, 1, 1, 10, 0, 0),
                        EndTime = new DateTime(2019, 1, 1, 12, 0, 0),
                        Description = "Finals",
                        Location = "Auditorium B", Professor = "Marta Kowalska" }
                };

                context.Students.AddRange(students);
                context.Courses.AddRange(courses);
                context.Books.AddRange(books);
                context.Exams.AddRange(exams);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void Test1_SaveDataToFile()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                string fileName = "data.json";
                IDataAccessService dataAccessService = new DataAccessService(context);
                dataAccessService.SaveDataToFile(fileName);
                Assert.IsTrue(File.Exists(fileName));
            }
        }

        [TestMethod]
        public void Test2_LoadDataFromFile()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                const string fileName = "data.json";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                var mockDataAccessService = Mock.Create<IDataAccessService>();
                Mock.Arrange(() => mockDataAccessService.SaveDataToFile(Arg.AnyString)).DoInstead(
                    () =>
                    {
                        const string fileContent = "[{\"StudentId\":\"1\",\"Name\":\"Wie\\u0144czys\\u0142aw\",\"LastName\":\"Nowakowicz\",\"PESEL\":\"PESEL1\",\"BirthDate\":\"1987-05-22T00:00:00\",\"PlaceOfBirth\":\"\",\"PlaceOfResidence\":\"\",\"AddressLine1\":\"\",\"AddressLine2\":\"\",\"Courses\":null,\"Exams\":null},{\"StudentId\":\"2\",\"Name\":\"Stanis\\u0142aw\",\"LastName\":\"Nowakowicz\",\"PESEL\":\"PESEL2\",\"BirthDate\":\"2019-06-25T00:00:00\",\"PlaceOfBirth\":\"\",\"PlaceOfResidence\":\"\",\"AddressLine1\":\"\",\"AddressLine2\":\"\",\"Courses\":null,\"Exams\":null},{\"StudentId\":\"3\",\"Name\":\"Eugenia\",\"LastName\":\"Nowakowicz\",\"PESEL\":\"PESEL3\",\"BirthDate\":\"2021-06-08T00:00:00\",\"PlaceOfBirth\":\"\",\"PlaceOfResidence\":\"\",\"AddressLine1\":\"\",\"AddressLine2\":\"\",\"Courses\":null,\"Exams\":null}]\r\n[{\"CourseCode\":\"MAT\",\"Title\":\"Matematyka\",\"Instructor\":\"Michalina Beldzik\",\"Schedule\":\"schedule1\",\"Description\":\"desc\",\"Credits\":5,\"Department\":\"dep1\",\"IsSelected\":false},{\"CourseCode\":\"BIOL\",\"Title\":\"Biologia\",\"Instructor\":\"Halina\",\"Schedule\":\"schedule2\",\"Description\":\"desc\",\"Credits\":6,\"Department\":\"dep3\",\"IsSelected\":false},{\"CourseCode\":\"CHEM\",\"Title\":\"Chemia\",\"Instructor\":\"Jan Nowak\",\"Schedule\":\"schedule3\",\"Description\":\"desc\",\"Credits\":7,\"Department\":\"dep3\",\"IsSelected\":false}]\r\n[{\"ExamId\":\"E001\",\"CourseCode\":\"MAT\",\"Date\":\"2024-10-11T00:00:00\",\"StartTime\":\"2019-01-01T10:00:00\",\"EndTime\":\"2019-01-01T12:00:00\",\"Location\":\"Auditorium A\",\"Description\":\"Finals\",\"Professor\":\"Marta Kowalska\",\"IsSelected\":false}]\r\n[{\"BookId\":\"B0001\",\"Title\":\"It\",\"Author\":\"Stephen King\",\"Publisher\":\"Scribner Book Company\",\"PublicationDate\":\"2016-1-6T00:00:00\",\"ISBN\":\"978-1501142970\",\"Genre\":\"Novel\",\"Description\":\"Desc... \"},{\"BookId\":\"B0002\",\"Title\":\"Pet Sematary\",\"Author\":\"Stephen King\",\"Publisher\":\"Random House US\",\"PublicationDate\":\"2018-12-4T00:00:00\",\"ISBN\":\"978-1982112394\",\"Genre\":\"Novel\",\"Description\":\"Desc... \"}]\n";
                        File.WriteAllText(fileName, fileContent);
                    }
                );
                
                IDataAccessService dataAccessService = new DataAccessService(context);
                mockDataAccessService.SaveDataToFile(fileName);
                dataAccessService.ReadDataFromFile(fileName);
                Assert.IsTrue(context.Students.Count() == 3 && context.Courses.Count() == 3 && context.Books.Count() == 2 && context.Exams.Count() == 1);
            }
        }
    }
}