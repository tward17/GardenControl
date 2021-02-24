using AutoMapper;
using GardenControlCore.Models;
using GardenControlCore.Scheduler;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.MapProfiles
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Schedule, ScheduleEntity>()
                .ForMember(dest =>
                dest.TriggerTypeId,
                opt => opt.MapFrom(src => src.TriggerType))
                .ForMember(dest =>
                dest.TriggerOffsetAmountTimeIntervalUnitId,
                opt => opt.MapFrom(src => src.TriggerOffsetAmountTimeIntervalUnit))
                .ForMember(dest =>
                dest.IntervalAmountTimeIntervalUnitId,
                opt => opt.MapFrom(src => src.IntervalAmountTimeIntervalUnit));

            CreateMap<ScheduleEntity, Schedule>()
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
