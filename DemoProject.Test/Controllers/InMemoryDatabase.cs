﻿using DemoProject.Models;
using DemoProject.Test.Fixtures;
using DemoProject.DbContexts;
using DemoProject.Test.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;

namespace DemoProject.Test.Controllers
{
    public class InMemoryDatabase
    {
        private WebApplicationFactory<Program> _factory;

        public InMemoryDatabase()
        {
            
            _factory = new WebApplicationFactory<DemoProject.Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<SchoolDbContext>));
                        services.AddDbContext<SchoolDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("test");
                        });
                    });
                });
        }

        [Fact]
        public async void OnGetStudent_WhenExecuteApi_ShouldReturnExpectedStudent()
        {
            // Arrange

            using (var scope = _factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<SchoolDbContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.Students.Add(new Models.Student()
                {
                    FirstName = "Hrushikesh",
                    LastName = "Mahapatra",
                    Address = "Delaware",
                    DOB = new DateTime(1981, 08, 04)
                });

                dbContext.SaveChanges();
            }

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(HttpHelper.Urls.GetAllStudents);
            var result = await response.Content.ReadFromJsonAsync<List<Student>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result.Count.Should().Be(1);

            result[0].FirstName.Should().Be("Hrushikesh");
            result[0].LastName.Should().Be("Mahapatra");
            result[0].Address.Should().Be("Delaware");
            result[0].DOB.Should().Be(new DateTime(1981, 08, 04));

        }

        [Fact]
        public async Task OnAddStudent_WhenExecuteController_ShouldStoreInDb()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<SchoolDbContext>();

                cntx.Database.EnsureDeleted();
                cntx.Database.EnsureCreated();
            }
            var client = _factory.CreateClient();
            var newStudent = DataFixture.GetStudent();

            var httpContent = HttpHelper.GetJsonHttpContent(newStudent);

            // Act
            var request = await client.PostAsync(HttpHelper.Urls.AddStudent, httpContent);
            var response = await client.GetAsync(HttpHelper.Urls.GetAllStudents);
            var result = await response.Content.ReadFromJsonAsync<List<Student>>();

            // Assert
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


            result.Count.Should().Be(1);
            result[0].FirstName.Should().Be(newStudent.FirstName);
            result[0].LastName.Should().Be(newStudent.LastName);
            result[0].Address.Should().Be(newStudent.Address);
            result[0].DOB.Should().Be(newStudent.DOB);
        }


    }
}
