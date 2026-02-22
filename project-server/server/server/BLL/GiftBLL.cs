using AutoMapper;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using server.DAL;
using server.DAL.Interfaces;
using server.DTO;
using System.Net;
using MailKit.Net.Smtp;
using WebApplication1.BLL;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WebApplication1.BLL
{
    public class GiftBLL : IgiftBLL
    {
        private readonly IgiftDAL giftDAL;
        private readonly IMapper _mapper;
        private readonly ICustomerDatailsDAL _customerDetailsDAL;
        private readonly ILogger<GiftBLL> _logger;

        public GiftBLL(IgiftDAL giftDAL, IMapper mapper, ICustomerDatailsDAL customerDetailsDAL, ILogger<GiftBLL> logger)
        {
            this.giftDAL = giftDAL;
            this._mapper = mapper;
            this._customerDetailsDAL = customerDetailsDAL;
            this._logger = logger;
        }

        public async Task<List<GiftDto>> Get()
        {
            var giftFromDb = await giftDAL.Get();

            var giftDtos = _mapper.Map<List<GiftDto>>(giftFromDb);

            for (int i = 0; i < giftFromDb.Count; i++)
            {
                if (giftFromDb[i].Winner != null)
                {
                    giftDtos[i].CustomerName = giftFromDb[i].Winner.FirstName + " " + giftFromDb[i].Winner.LastName;
                }
                else
                {
                    giftDtos[i].CustomerName = "אין זוכה עדיין";
                }
            }

            return giftDtos;
        }
        public async Task<GiftDto> GetById(int id)
        {
            var giftFromDb = await giftDAL.GetById(id);
            return _mapper.Map<GiftDto>(giftFromDb);
        }

        public async Task Post(GiftDto gift)
        {
            _logger.LogInformation("Adding a new gift: {GiftName}", gift.Name);
            var giftModel = _mapper.Map<GiftModel>(gift);

            if (gift.DonorId != 0)
            {
                giftModel.DonorId = gift.DonorId;
                giftModel.Donor = null;
            }

            giftModel.WinnerId = null;
            giftModel.Winner = null;

            await giftDAL.Post(giftModel);
            _logger.LogInformation("Gift {GiftName} added successfully.", gift.Name);
        }

        public async Task Update(int id, GiftDto gift)
        {
            _logger.LogInformation("Updating gift with ID: {Id}", id);
            var giftModel = _mapper.Map<GiftModel>(gift);
            await giftDAL.Update(id, giftModel);
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Deleting gift with ID: {Id}", id);
            await giftDAL.Delete(id);
        }

        public async Task<GiftDto> GetByName(string name)
        {
            _logger.LogInformation("Searching for gift by name: {Name}", name);
            var giftModel = await giftDAL.GetByName(name);
            return giftModel == null ? null : _mapper.Map<GiftDto>(giftModel);
        }

        public async Task<GiftDto> GetByCategory(string category)
        {
            _logger.LogInformation("Searching for gifts in category: {Category}", category);
            var giftModel = await giftDAL.GetByCategory(category);
            return giftModel == null ? null : _mapper.Map<GiftDto>(giftModel);
        }

        public async Task<List<GiftDto>> GetByDonor(string name)
        {
            _logger.LogInformation("Searching for gifts by donor: {DonorName}", name);
            var giftModels = await giftDAL.GetByDonor(name);

            if (giftModels == null || !giftModels.Any())
            {
                return new List<GiftDto>();
            }
            return _mapper.Map<List<GiftDto>>(giftModels);
        }

        [HttpGet("by-purchases/{num}")]
        public async Task<List<GiftDto>> GetByPurchasesCount(int num)
        {
            _logger.LogInformation("Searching for gifts with purchase count: {Num}", num);
            var giftModels = await giftDAL.GetByPurchasesCount(num);

            if (giftModels == null) return new List<GiftDto>();

            return giftModels.Select(p => new GiftDto
            {
                Id = p.Id,
                Name = p.Name,
                PriceCard = p.PriceCard,
                DonorName = p.Donor != null ? $"{p.Donor.FirstName} {p.Donor.LastName}" : "ללא תורם",
                Category = p.Category
            }).ToList();
        }

        public async Task<List<CustomerDetailsDto>> GetByParches(int giftId)
        {
            _logger.LogInformation("Fetching purchases for gift ID: {GiftId}", giftId);
            var purchases = await _customerDetailsDAL.GetByGiftId(giftId);
            return purchases.Select(p => new CustomerDetailsDto
            {
                CustomerName = p.CustomerName,
                Quntity = p.Quntity,
            }).ToList();
        }

        public async Task<List<GiftDto>> giftExpensive()
        {
            _logger.LogInformation("Fetching all gifts sorted by price (Expensive first).");
            var giftsList = await giftDAL.giftExpensive();

            if (giftsList == null)
            {
                return new List<GiftDto>();
            }

            return _mapper.Map<List<GiftDto>>(giftsList);
        }

        public async Task<GiftDto> byMostParches()
        {
            _logger.LogInformation("Fetching gift with most purchases.");
            var giftModel = await giftDAL.byMostParches();
            return giftModel == null ? null : _mapper.Map<GiftDto>(giftModel);
        }

        public async Task<WinnerDTO> Winner(int giftId)
        {
            _logger.LogInformation("Starting lottery process for gift ID: {GiftId}", giftId);
            var gift = await giftDAL.Winner(giftId);

            var confirmedPurchases = gift?.customerDatails
                .Where(d => d.Status == "Confirmed")
                .ToList();

            if (gift == null || confirmedPurchases == null || !confirmedPurchases.Any() || gift.WinnerId != null)
            {
                _logger.LogWarning("Lottery failed for gift ID {GiftId}.", giftId);
                return null;
            }

            var random = new Random();
            var winningPurchase = confirmedPurchases[random.Next(confirmedPurchases.Count)];
            var winner = winningPurchase.customer;

            _logger.LogInformation("Winner selected: {WinnerName} for gift {GiftName}", winner.FirstName, gift.Name);

            gift.WinnerId = winner.Id;
            gift.Winner = null;

            await giftDAL.Update(gift.Id, gift);
            gift.Winner = winner;

            try
            {
                _logger.LogInformation("Attempting to send winner email to: {Email}", winner.Email);
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("מערכת הגרלות", "talyatoledano10@gmail.com"));
                message.To.Add(new MailboxAddress(winner.FirstName, winner.Email));
                message.Subject = "מזל טוב! זכית בפרס";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
            <div dir='rtl' style='font-family: Arial;'>
                <h2>שלום {winner.FirstName}!</h2>
                <p>איזה כיף! הגרלנו עכשיו את הפרס <b>{gift.Name}</b> ואת/ה הזוכה!</p>
                <p>ניצור איתך קשר בהקדם.</p>
            </div>"
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("talyatoledano10@gmail.com", "xcba vdpp dmaa dvls");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                _logger.LogInformation("Email sent successfully to {Email}", winner.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send winner email for gift {GiftId}", giftId);
            }

            return _mapper.Map<WinnerDTO>(gift);
        }
        public async Task<List<GiftDto>> reportWinners()
        {
            _logger.LogInformation("Generating winners report.");
            var giftFromDb = await giftDAL.reportWinners();
            return _mapper.Map<List<GiftDto>>(giftFromDb);
        }

        Task<List<CustomerDetailsDto>> IgiftBLL.GetByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public async Task<int> reportAchnasot()
        {
            _logger.LogInformation("Generating achnasot report.");
            return await this.giftDAL.reportAchnasot();
        }
    }
}