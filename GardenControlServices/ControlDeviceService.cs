using AutoMapper;
using GardenControlCore.Models;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    /// <summary>
    /// Service responsible for managing the Control Devices that are stored in the system.
    /// Does not perform the interaction with the physical devices.
    /// </summary>
    public class ControlDeviceService : IControlDeviceService
    {
        private IControlDeviceRepository _controlDeviceRepository { get; init; }
        private IMapper _mapper { get; init; }

        public ControlDeviceService(IControlDeviceRepository controlDeviceRepository, IMapper mapper)
        {
            _controlDeviceRepository = controlDeviceRepository;
            _mapper = mapper;
        }

        public async Task DeleteDevice(int id)
        {
            await _controlDeviceRepository.DeleteDevice(id);
        }

        public async Task<IEnumerable<ControlDevice>> GetAllDevices()
        {
            return _mapper.Map<IEnumerable<ControlDevice>>(await _controlDeviceRepository.GetAllDevices());
        }

        public async Task<ControlDevice> GetDevice(int id)
        {
            return _mapper.Map<ControlDevice>(await _controlDeviceRepository.GetDevice(id));
        }

        public async Task InsertDevice(ControlDevice device)
        {
            await _controlDeviceRepository.InsertDevice(_mapper.Map<ControlDeviceEntity>(device));
        }

        public async Task UpdateDevice(ControlDevice device)
        {
            await _controlDeviceRepository.UpdateDevice(_mapper.Map<ControlDeviceEntity>(device));
        }
    }
}
