using System.Xml.Schema;
using Moq;
using FluentAssertions;
using AutoFixture;
using DemoProject.Controllers;
using DemoProject.Services;
using Microsoft.AspNetCore.Mvc;
using DemoProject.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DemoProjectUnitTest
{
    public class StudentControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IStudentService> _serviceMock;
        //private readonly Mock<IStudentBGCService> _BGCserviceMock;
        private readonly StudentController _sut;

        public StudentControllerTest()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IStudentService>>();
            //_BGCserviceMock = _fixture.Freeze<Mock<IStudentBGCService>>();
            // _sut = new StudentController(_serviceMock.Object, _BGCserviceMock.Object);//implementation in-memory
            _sut = new StudentController(_serviceMock.Object);
        }


        //[Fact]
        //public void StudentControllerConstructor_ShouldReturnNullReferenceException_WhenServiceIsNull()
        //{
        //    // Arrange
        //    IStudentService StudentService = null;

        //    // Act && Assert
        //    Assert.Throws<ArgumentNullException>(() => new StudentController(StudentService));
        //}


        [Fact]
        public async Task GetStudents_ShouldReturnOkResponse_WhenDataFound()
        {
            // Arrange
            var StudentMock = _fixture.Create<IEnumerable<Student>>();
            _serviceMock.Setup(x => x.GetStudentsAsync()).ReturnsAsync(StudentMock);
             
            // Act
            var result = await _sut.GetAllStudentsAsync().ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
            result.Should().NotBeNull();
           // result.Should().BeAssignableTo<ActionResult<IEnumerable<Student>>>();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(StudentMock.GetType());
            _serviceMock.Verify(x => x.GetStudentsAsync(), Times.Once());
        }
        [Fact]
        public async Task GetStudents_ShouldReturnNotFound_WhenDataNotFound()
        {
            // Arrange
            List<Student> response = null;
            _serviceMock.Setup(x => x.GetStudentsAsync()).ReturnsAsync(response);

            // Act
            var result = await _sut.GetAllStudentsAsync().ConfigureAwait(false);

            // Assert
            result.Should().BeAssignableTo<NotFoundObjectResult>();
            result.Should().NotBeNull();
            _serviceMock.Verify(x => x.GetStudentsAsync(), Times.Once());
        }
        [Fact]
        public async Task GetStudent_ShouldReturnOkResponse_WhenValidInput()
        {
            // Arrange
            var studentMock = _fixture.Create<Student>();
            //var id = _fixture.Create<int>();
            var id = studentMock.Id;
            _serviceMock.Setup(x => x.GetStudentAsync(id)).ReturnsAsync(studentMock);

            // Act
            var result = await _sut.GetStudentAsync(id).ConfigureAwait(false);

            // Assert
            
            result.Should().NotBeNull();
            //result.Should().BeAssignableTo<ActionResult<Student>>();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(studentMock.GetType());
            _serviceMock.Verify(x => x.GetStudentAsync(id), Times.Once());
        }
        [Fact]
        public async Task GetStudent_ShouldReturnOkResponse_WhenValidInput_ShouldReturnNotFound_WhenNoDataFound()
        {
            // Arrange


            Student response = null;
            var id = _fixture.Create<int>();
            _serviceMock.Setup(x => x.GetStudentAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _sut.GetStudentAsync(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            _serviceMock.Verify(x => x.GetStudentAsync(id), Times.Once());
        }
        [Fact]
        public async Task GetStudent_ShouldReturnBadRequest_WhenInputIsEqualsZero()
        {
            // Arrange
            var response = _fixture.Create<Student>();
            int id = 0;

            // Act
            var result = await _sut.GetStudentAsync(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();
        }
       // [Fact]
        //public async Task AddAsync_ShouldReturnBadRequest_WhenExtBGCreturnsNothing()
        //{
        //    // Arrange
        //    var response = _fixture.Create<Student>();
        //   var BGCServiceresponse= _BGCserviceMock.Setup(x => x.GetBGCFromExternalApiAsync()).Returns(Task.FromResult("External Service returnd"));

        //    // Act
        //    var result = await _sut.AddAsync(response).ConfigureAwait(false);

        //    // Assert
        //    Assert.Null(BGCServiceresponse);
            
        //    result.Should().BeAssignableTo<BadRequestObjectResult>();
        //}




    }
}
