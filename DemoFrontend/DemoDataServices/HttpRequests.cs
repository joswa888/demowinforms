using Newtonsoft.Json;
using System.Net.Http;
using DemoBackend.Contracts.Responses;
using DemoBackend.Contracts.Requests;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace DemoFrontend
{
    public static class HttpRequests
    {
        private static string url = @"http://localhost:53088/api/Employees/";
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<GetEmployeeResponse> CreateEmployee(UpsertEmployeeRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            var serializeRequest = JsonConvert.SerializeObject(request);

            requestMessage.Content = new StringContent(serializeRequest, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var queryResult = JsonConvert.DeserializeObject<GetEmployeeResponse>(responseContent);

            return queryResult;
        }

        public static async Task<IEnumerable<GetEmployeeResponse>> GetEmployees()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var responseMessage = await httpClient.SendAsync(requestMessage);

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var queryResult = JsonConvert.DeserializeObject<ICollection<GetEmployeeResponse>>(responseContent);

            return queryResult;
        }

        public static async Task UpdateEmployee(int employeeId, UpsertEmployeeRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, string.Concat(url,employeeId));

            var serializeRequest = JsonConvert.SerializeObject(request);

            requestMessage.Content = new StringContent(serializeRequest, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);
        }

        public static async Task DeleteEmployee(int employeeId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, string.Concat(url, employeeId));
            
            var responseMessage = await httpClient.SendAsync(requestMessage);
        }


    }
}
