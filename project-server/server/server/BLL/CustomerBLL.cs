using AutoMapper;
using Microsoft.Extensions.Logging;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplication1.BLL
{
    public class CustomerBLL : IcustomerBLL
    {
        private readonly IcustomerDAL _customerDAL;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerBLL> _logger;

        public CustomerBLL(IcustomerDAL customerDAL, IMapper mapper, ILogger<CustomerBLL> logger)
        {
            _customerDAL = customerDAL;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CustomerDto>> Get()
        {
            _logger.LogInformation("Starting Get method in CustomerBLL");

            var customersFromDb = await _customerDAL.Get();

            if (customersFromDb == null)
            {
                _logger.LogWarning("Data from DAL is null");
            }
            else
            {
                _logger.LogInformation("Fetched {Count} records from Database", customersFromDb.Count);
            }

            _logger.LogInformation("Attempting to map database records to CustomerDetails");
            var mappedResult = _mapper.Map<List<CustomerDto>>(customersFromDb);

            _logger.LogInformation("Mapping completed successfully");
            return mappedResult;
        }

        public async Task<CustomerDto> GetById(int id)
        {
            var customerFromDb = await _customerDAL.GetById(id);

            return _mapper.Map<CustomerDto>(customerFromDb);
        }
    }
}