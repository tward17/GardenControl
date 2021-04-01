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
    public class ScheduleDtoProfile : Profile
    {
        public ScheduleDtoProfile()
        {
            CreateMap<ScheduleDto, Schedule>()
                .ForMember(dest =>
                dest.TriggerType,
                opt => opt.MapFrom(src => src.TriggerTypeId))
                .ForMember(dest =>
                dest.TriggerOffsetAmountTimeIntervalUnit,
                opt => opt.MapFrom(src => src.TriggerOffsetAmountTimeIntervalUnitId))
                .ForMember(dest =>
                dest.IntervalAmountTimeIntervalUnit,
                opt => opt.MapFrom(src => src.IntervalAmountTimeIntervalUnitId))
                .ForMember(dest =>
                dest.ScheduleTasks,
                opt => opt.MapFrom(src => src.ScheduleTasks));

            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest =>
                dest.TriggerTypeId,
                opt => opt.MapFrom(src => src.TriggerType))
                .ForMember(dest =>
                dest.TriggerOffsetAmountTimeIntervalUnitId,
                opt => opt.MapFrom(src => src.TriggerOffsetAmountTimeIntervalUnit))
                .ForMember(dest =>
                dest.IntervalAmountTimeIntervalUnitId,
                opt => opt.MapFrom(src => src.IntervalAmountTimeIntervalUnit));
        }
    }
}
