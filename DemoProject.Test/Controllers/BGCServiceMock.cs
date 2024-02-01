using System;
using System.Collections.Generic;
using System.Linq;
using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemoProject.Services;


namespace DemoProject.Test.Controllers
{
    public class BGCServiceMock
    {
        public Mock<IStudentBGCApiClient> StudentBGCApiClient { get; }


        public BGCServiceMock()
        {
            StudentBGCApiClient = new Mock<IStudentBGCApiClient>();
        }

        public IEnumerable<(Type, object)> GetMocks()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x =>
                {
                    var underlyingType = x.PropertyType.GetGenericArguments()[0];
                    var value = x.GetValue(this) as Mock;

                    return (underlyingType, value.Object);
                })
                .ToArray();
        }
    }
}


