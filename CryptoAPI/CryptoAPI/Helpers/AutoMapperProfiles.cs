using System;
using AutoMapper;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;

namespace CryptoAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<CryptoCurrency, CryptoDto>();
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}
