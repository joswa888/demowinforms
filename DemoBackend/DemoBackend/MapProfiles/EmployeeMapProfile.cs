using AutoMapper;
using DemoBackend.Contracts.Requests;
using DemoBackend.Contracts.Responses;
using DemoBackend.Models;

namespace DemoBackend.MapProfiles
{
    public class EmployeeMapProfile : Profile
    {
        public EmployeeMapProfile()
        {
            CreateMap<UpsertEmployeeRequest, Employee>()
                .ForMember(dest => dest.Id, map => map.Ignore())
                .ForMember(dest => dest.DateCreated, map => map.Ignore());

            CreateMap<Employee, GetEmployeeResponse>();

            CreateMap<Employee, Employee>()
                .ForMember(dest => dest.Id, map => { map.UseDestinationValue(); map.Ignore(); })
                .ForMember(dest => dest.DateCreated, map => { map.UseDestinationValue(); map.Ignore(); });
        }
    }
}
