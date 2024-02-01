
using System.Net.Http;
using System.Threading.Tasks;
namespace DemoProject.Services
{

    public interface IStudentBGCApiClient
    {
        Task<string> GetBGCAsync();
    }
    public class StudentBGCApiClient: IStudentBGCApiClient
    {
     private readonly HttpClient _httpClient;
            
            public StudentBGCApiClient(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public Task<string> GetBGCAsync()
            {
                return Task.FromResult("Student BGC result came from a real service");
            }
        }
    }

