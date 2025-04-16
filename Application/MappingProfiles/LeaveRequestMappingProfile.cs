using AutoMapper;
using LeaveManagement.Application.Commands;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Core.Entities;
using LeaveManagement.Core.Enums;

namespace LeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestMappingProfile : Profile
    {
        public LeaveRequestMappingProfile()
        {
            CreateMap<CreateLeaveRequestCommand, LeaveRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LeaveStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));


            CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();

            CreateMap<LeaveRequest, LeaveRequestByEmployeeIdDto>();

        }
    }
}
