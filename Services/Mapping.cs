using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Services.DTOs;
using Domain.Entities;

namespace Services
{
    public class Mapping : Profile
    {
        public Mapping() 
        {

            CreateMap<Person, PersonDTO>()
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.NombreCompleto))
            .ForMember(dest => dest.age, opt => opt.MapFrom(src => src.Edad))
            .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Domicilio))
            .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.Telefono))
            .ForMember(dest => dest.profession, opt => opt.MapFrom(src => src.Profesion))
            .ReverseMap();
        }
    }
}
