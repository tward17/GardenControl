using GardenControlCore.Models;
using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace GardenControlServices.MapProfiles
{
    public class AppSettingProfile : Profile
    {
        public AppSettingProfile()
        {
            CreateMap<AppSettingEntity, AppSetting>();
        }
    }
    
}
