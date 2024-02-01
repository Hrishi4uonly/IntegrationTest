using DemoProject.Models;
using DemoProject.Test.Fixtures;
using DemoProject.Test.Helper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
namespace DemoProject.Test.Controllers
{
    public class StudentBGCExternalAPITest
    {


        private readonly WebApplicationFactoryFixture _factory;

        public StudentBGCExternalAPITest(WebApplicationFactoryFixture factory)
        {
            _factory = factory;
        }
    }
}
