using AutoMapper;
using server.DTO;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer Mappings
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();

            CreateMap<RegisterDTO, CustomerModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "user"));

            CreateMap<CustomerModel, AuthDTO>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            // Donor Mappings
            CreateMap<DonorModel, DonorDto>().ReverseMap();

            // CustomerDetails Mapping
            CreateMap<CustomerDatails, CustomerDetailsDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    src.customer != null ? $"{src.customer.FirstName} {src.customer.LastName}" : "לא ידוע"))
                .ForMember(dest => dest.GiftName, opt => opt.MapFrom(src =>
                    src.gift != null ? src.gift.Name : "מתנה לא נמצאה"))
                .ForMember(dest => dest.GiftPrice, opt => opt.MapFrom(src =>
                    src.gift != null ? src.gift.PriceCard : 0))
                .ReverseMap();

            // Winner Mappings
            CreateMap<CustomerModel, WinnerDTO>();

            CreateMap<GiftModel, WinnerDTO>()
                .ForMember(dest => dest.GiftId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GiftName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GiftImage, opt => opt.MapFrom(src => src.GiftImage))
                .ForMember(dest => dest.WinnerFullName, opt => opt.MapFrom(src =>
                    src.Winner != null ? $"{src.Winner.FirstName} {src.Winner.LastName}" : "אין זוכה"))
                .ForMember(dest => dest.WinnerEmail, opt => opt.MapFrom(src => src.Winner != null ? src.Winner.Email : ""))
                .ForMember(dest => dest.WinnerPhone, opt => opt.MapFrom(src => src.Winner != null ? src.Winner.Phone : ""))
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src =>
                    src.Donor != null ? $"{src.Donor.FirstName} {src.Donor.LastName}" : "ללא תורם"));

            CreateMap<GiftModel, GiftDto>()
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src =>
                    src.Donor != null ? $"{src.Donor.FirstName} {src.Donor.LastName}" : "ללא תורם"))

                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    src.Winner != null ? $"{src.Winner.FirstName} {src.Winner.LastName}" : null))

                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.WinnerId))

                .ReverseMap();
        }
    }
}