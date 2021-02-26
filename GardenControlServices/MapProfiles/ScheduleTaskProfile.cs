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
    public class ScheduleTaskProfile : Profile
    {
        public ScheduleTaskProfile()
        {
            CreateMap<ScheduleTaskEntity, ScheduleTask>()
                .ForMember(dest =>
                dest.TaskAction,
                opt => opt.MapFrom(src => new TaskAction { TaskActionId = src.TaskActionId }));
        }
    }
}
