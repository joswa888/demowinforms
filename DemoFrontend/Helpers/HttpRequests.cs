using Newtonsoft.Json;
using System.Net.Http;
using DemoBackend.Contracts.Responses;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Helpers
{
    public static class HttpRequests
    {
       // private  HttpClient client = new HttpClient();

        //public static GetEmployeeResponse PostEmployee(UpsertEmployeeRequest upsertEmployeeRequest)
        //{
        //    var requestMessage = new HttpRequestMessage(HttpMethod.Post, @"http://localhost:53088/api/Employees");
        //}

        //http://localhost:53088/api/Employees
        public static async Task<IEnumerable<GetEmployeeResponse>> GetEmployees()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, @"http://localhost:53088/api/Employees");

            var httpClient = new HttpClient();
            var responseMessage = await httpClient.SendAsync(requestMessage);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var queryResult = JsonConvert.DeserializeObject<ICollection<GetEmployeeResponse>>(responseContent);

            return queryResult;
        }
    }
}
