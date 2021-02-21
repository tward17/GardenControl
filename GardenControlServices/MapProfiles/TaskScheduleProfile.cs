using AutoMapper;
using GardenControlCore.Models;
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
            CreateMap<TaskScheduleEntity, TaskSchedule>();
        }
    }
}
