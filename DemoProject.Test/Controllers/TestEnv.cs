using DemoProject.Models;
using DemoProject.Services;
using DemoProject.Test.Fixtures;
using DemoProject.Test.Helper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Test.Controllers
{
    public class TestEnv:IClassFixture<WebApplicationFactoryFixture>
    {
        private readonly WebApplicationFactoryFixture _factory;

        public TestEnv(WebApplicationFactoryFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task OnGetStudents_WhenExecuteController_ShouldreturnTheExpecedStudents()
        {
            // Arrange

            // Act
            var response = await _factory.Client.GetAsync(HttpHelper.Urls.GetAllStudents);
            var result = await response.Content.ReadFromJsonAsync<List<Student>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            //result.Count.Should().Be(_factory.InitialStudentsCount);
           // result.Should()
             //   .BeEquivalentTo(DataFixture.GetStudents(_factory.InitialStudentsCount), options => options.Excluding(t => t.Id));
        }

        [Fact]
        public async Task OnGetStudent_WhenExecuteController_Shouldreturnthestudent()
        {
            // Arrange
            var newStudent = DataFixture.GetStudent(true);

            // Act
            var request = await _factory.Client.PostAsync(HttpHelper.Urls.AddStudent, HttpHelper.GetJsonHttpContent(newStudent));
            var response = await _factory.Client.GetAsync($"{HttpHelper.Urls.GetStudent}/{_factory.InitialStudentsCount + 1}");
            var result = await response.Content.ReadFromJsonAsync<Student>();

            // Assert
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


            result.FirstName.Should().Be(newStudent.FirstName);
            result.LastName.Should().Be(newStudent.LastName);
            result.Address.Should().Be(newStudent.Address);
            result.DOB.Should().Be(newStudent.DOB);
        }
        [Fact]
        public async Task GetStudentBGCIntegrationTest()
        {
            var expected = "Student BGC result came from a real service";
            BGCServiceMock obj = new BGCServiceMock();
            obj.StudentBGCApiClient
                .Setup(x => x.GetBGCAsync())
                .ReturnsAsync(expected);

            var client = _factory.Client;

            var response = await client.GetAsync("/StudentBGCExternal/backgroundcheck");
            var responseMessage = await response.Content.ReadAsStringAsync();

            Assert.Equal(responseMessage, expected);
            
        }

    }
}
