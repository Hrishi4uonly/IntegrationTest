using Bogus;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Test.Fixtures
{
    internal class DataFixture
    {
        public static List<Student> GetStudents(int count, bool useNewSeed = false)
        {
            return GetStudentFaker(useNewSeed).Generate(count);
        }
        public static Student GetStudent(bool useNewSeed = false)
        {
            return GetStudents(1, useNewSeed)[0];
        }

        private static Faker<Student> GetStudentFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            var student= new Faker<Student>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.FirstName, (faker, t) => faker.Name.FirstName())
                .RuleFor(t => t.LastName, (faker, t) => faker.Name.LastName())
                .RuleFor(t => t.Address, (faker, t) => faker.Address.FullAddress())
                .RuleFor(t => t.DOB, (faker, t) => faker.Date.Past(5, new DateTime(2010, 1, 1)))
                .UseSeed(seed);
            return student;
        }
    }
}
