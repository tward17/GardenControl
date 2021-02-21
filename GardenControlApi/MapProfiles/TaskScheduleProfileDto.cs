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
    public class TaskScheduleProfileDto : Profile
    {
        public TaskScheduleProfileDto()
        {
            CreateMap<TaskScheduleDto, TaskSchedule>()
                .ForMember(dest =>
                dest.ControlDevice,
                opt => opt.MapFrom(src => new ControlDevice { ControlDeviceId = src.ControlDeviceId }))
                .ForMember(dest =>
                dest.TaskAction,
                opt => opt.MapFrom(src => new TaskAction { TaskActionId = src.TaskActionId }))
                .ForMember(dest =>
                dest.TriggerType,
                opt => opt.MapFrom(src => src.TriggerTypeId))
                .ForMember(dest =>
                dest.TriggerOffsetAmountTimeIntervalUnit,
                opt => opt.MapFrom(src => src.TriggerOffsetAmountTimeIntervalUnitId))
                .ForMember(dest =>
                dest.IntervalAmountTimeIntervalUnit,
                opt => opt.MapFrom(src => src.IntervalAmountTimeIntervalUnitId));

            CreateMap<TaskSchedule, TaskScheduleDto>()
                .ForMember(dest =>
                dest.ControlDeviceId,
                opt => opt.MapFrom(src => src.ControlDevice.ControlDeviceId))
                .ForMember(dest =>
                dest.TaskActionId,
                opt => opt.MapFrom(src => src.TaskAction.TaskActionId))
                .ForMember(dest =>
                dest.TriggerTypeId,
                opt => opt.MapFrom(src => src.TriggerType))
                .ForMember(dest =>
                dest.TriggerOffsetAmountTimeIntervalUnitId,
                opt => opt.MapFrom(src => src.TriggerOffsetAmountTimeIntervalUnit))
                .ForMember(dest =>
                dest.IntervalAmountTimeIntervalUnitId,
                opt => opt.MapFrom(src => src.IntervalAmountTimeIntervalUnit))
                .ReverseMap();
        }
    }
}
