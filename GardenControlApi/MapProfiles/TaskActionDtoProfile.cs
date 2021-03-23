using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.MapProfiles
{
    public class TaskActionDtoProfile : Profile
    {
        public TaskActionDtoProfile()
        {
            CreateMap<TaskAction, TaskActionDto>();
        }
    }
}
