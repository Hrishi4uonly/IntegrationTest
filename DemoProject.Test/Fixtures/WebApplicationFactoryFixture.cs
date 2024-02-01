using DemoProject.DbContexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Test.Fixtures
{
    //for disposing inmemory and database records after every integration test and 
    //overriding dbcontext 
    public class WebApplicationFactoryFixture : IAsyncLifetime
    {
        private const string _connectionString = @$"Server=(localdb)\.;Database=UserIntegration;Trusted_Connection=True";
       // private const string _connectionString = @$"Server=Swapnali;Database=StudentDB;Integrated Security=True;MultipleActiveResultSets=true;Encrypt=False";

        private WebApplicationFactory<Program> _factory;

        public HttpClient Client { get; private set; }
        public int InitialStudentsCount { get; set; } = 3;

        public WebApplicationFactoryFixture()
        {
            _factory = new WebApplicationFactory<DemoProject.Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(Services =>
                {
                    Services.RemoveAll(typeof(DbContextOptions<SchoolDbContext>));
                    Services.AddDbContext<SchoolDbContext>(options =>
                    {
                        options.UseSqlServer(_connectionString);
                    });
                });
            });
            Client = _factory.CreateClient();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<SchoolDbContext>();

                await cntx.Database.EnsureDeletedAsync();
            }
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<SchoolDbContext>();

                await cntx.Database.EnsureCreatedAsync();

                await cntx.Students.AddRangeAsync(DataFixture.GetStudents(InitialStudentsCount));
                await cntx.SaveChangesAsync();
            }
        }
    }
}
