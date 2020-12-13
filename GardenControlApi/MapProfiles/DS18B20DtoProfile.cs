using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.MapProfiles
{
    public class DS18B20DtoProfile : Profile
    {
        public DS18B20DtoProfile()
        {
            CreateMap<TemperatureReading, DS18B20Dto>();
        }
    }
}
