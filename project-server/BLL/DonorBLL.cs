using AutoMapper;
using System.Collections.Generic;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApplication1.BLL
{
    public class DonorBLL : IdonorBLL
    {
        private readonly IdonorDAL donorDAL;
        private readonly IMapper _mapper;
        private readonly ILogger<DonorBLL> _logger;

        public DonorBLL(IdonorDAL donorDAL, IMapper mapper, ILogger<DonorBLL> logger)
        {
            this.donorDAL = donorDAL;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<List<DonorDto>> Get()
        {
            _logger.LogInformation("Attempting to fetch all donors.");
            var donorFromDb = await donorDAL.Get();

            _logger.LogInformation("Fetched {Count} donors from database. Mapping to DTOs.", donorFromDb?.Count ?? 0);
            return _mapper.Map<List<DonorDto>>(donorFromDb);
        }

        public async Task<DonorDto> GetById(int id)
        {
            var donorFromDb = await donorDAL.GetById(id);
            return _mapper.Map<DonorDto>(donorFromDb);
        }

        public async Task Post(DonorDto donorDto)
        {
            _logger.LogInformation("Mapping and posting new donor: {DonorName}", donorDto.FirstName + " " + donorDto.LastName);
            var donorModel = _mapper.Map<DonorModel>(donorDto);
            await donorDAL.Post(donorModel);
            _logger.LogInformation("Donor posted successfully.");
        }

        public async Task Update(int id, DonorDto donorDto)
        {
            _logger.LogInformation("Attempting to update donor with ID: {Id}", id);
            var donorModel = _mapper.Map<DonorModel>(donorDto);
            await donorDAL.Update(id, donorModel);
            _logger.LogInformation("Update operation completed for donor ID: {Id}", id);
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete donor with ID: {Id}", id);
            await donorDAL.Delete(id);
            _logger.LogInformation("Delete operation completed for donor ID: {Id}", id);
        }

        public async Task<DonorDto> GetByEmail(string email)
        {
            _logger.LogInformation("Searching for donor with email: {Email}", email);
            var donorModel = await donorDAL.GetByGmail(email);

            if (donorModel == null)
            {
                _logger.LogWarning("No donor found with email: {Email}", email);
                return null;
            }

            return _mapper.Map<DonorDto>(donorModel);
        }

        public async Task<DonorDto> GetByName(string name)
        {
            _logger.LogInformation("Searching for donor with name: {Name}", name);
            var donorModel = await donorDAL.GetByName(name);

            if (donorModel == null)
            {
                _logger.LogWarning("No donor found with name: {Name}", name);
                return null;
            }

            return _mapper.Map<DonorDto>(donorModel);
        }

        public async Task<List<DonorDto>> GetByGift(string name)
        {
            _logger.LogInformation("Searching for donor associated with gift ID: {GiftId}", name);
            var donorModel = await donorDAL.GetByGift(name);

            if (donorModel == null)
            {
                _logger.LogWarning("No donor found for gift ID: {GiftId}", name);
                return null;
            }

            return _mapper.Map<List<DonorDto>>(donorModel);
        }
    }
}