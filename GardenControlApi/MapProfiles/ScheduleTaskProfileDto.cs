using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.MapProfiles
{
    public class ScheduleTaskProfileDto : Profile
    {
        public ScheduleTaskProfileDto()
        {
            CreateMap<ScheduleTaskDto, ScheduleTask>()
                .ForMember(dest =>
                dest.ControlDevice,
                opt => opt.MapFrom(src => new ControlDevice { ControlDeviceId = src.ControlDeviceId }))
                .ForMember(dest =>
                dest.TaskAction,
                opt => opt.MapFrom(src => new TaskAction { TaskActionId = src.TaskActionId }));

            CreateMap<ScheduleTask, ScheduleTaskDto>()
                .ForMember(dest =>
                dest.ScheduleId,
                opt => opt.MapFrom(src => src.Schedule.ScheduleId))
                .ForMember(dest =>
                dest.ControlDeviceId,
                opt => opt.MapFrom(src => src.ControlDevice.ControlDeviceId))
                .ForMember(dest =>
                dest.TaskActionId,
                opt => opt.MapFrom(src => src.TaskAction.TaskActionId));
        }
    }
}
