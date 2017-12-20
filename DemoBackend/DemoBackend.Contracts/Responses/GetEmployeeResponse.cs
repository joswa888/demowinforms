using System;

namespace DemoBackend.Contracts.Responses
{
    public class GetEmployeeResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
