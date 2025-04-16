using AutoMapper;
using LeaveManagement.Application.Commands;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();
        }
    }
}
