using AutoMapper;
using GardenControlCore.Models;
using GardenControlCore.Scheduler;
using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.MapProfiles
{
    public class TaskScheduleProfile : Profile
    {
        public TaskScheduleProfile()
        {
            CreateMap<TaskSchedule, TaskScheduleEntity>()
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
                opt => opt.MapFrom(src => src.IntervalAmountTimeIntervalUnit));

            CreateMap<TaskScheduleEntity, TaskSchedule>()
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
        }
    }
}
