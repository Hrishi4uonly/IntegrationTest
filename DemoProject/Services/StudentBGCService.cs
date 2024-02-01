using System.Threading.Tasks;
namespace DemoProject.Services
{
    public interface IStudentBGCService
    {
        Task<string> GetBGCFromExternalApiAsync();
    }

    public class StudentBGCService : IStudentBGCService
    {
        private readonly IStudentBGCApiClient _studentBGCApiClient;

        public StudentBGCService(IStudentBGCApiClient temperatureApiClient)
        {
            _studentBGCApiClient = temperatureApiClient;
        }

        
        public async Task<string> GetBGCFromExternalApiAsync()
        {
            return await _studentBGCApiClient.GetBGCAsync();
        }
    }
}

