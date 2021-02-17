using AutoMapper;
using GardenControlApi.Models;
using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenControlApi.MapProfiles
{
    public class MeasurementUnitDtoProfile : Profile
    {
        public MeasurementUnitDtoProfile()
        {
            CreateMap<MeasurementUnit, MeasurementUnitDto>().ReverseMap();
        }
    }
}
