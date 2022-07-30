using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RBG_API.Dtos;

namespace RBG_API.Services
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, CharacterGetDto>();
            CreateMap<CharacterAddDto, Character>();
            CreateMap<CharacterUpdateDto,Character>();
            CreateMap<WeaponAddDto, Weapon>();
            CreateMap<Weapon, WeaponGetDto>();

        }
    }
}