using System.ComponentModel.DataAnnotations;

namespace DemoBackend.Contracts.Requests
{
    public class UpsertEmployeeRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
