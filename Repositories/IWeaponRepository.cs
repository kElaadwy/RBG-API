using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RBG_API.Dtos;

namespace RBG_API.Repositories
{
    public interface IWeaponRepository
    {
        Task<ServiceResponse<CharacterGetDto>> AddWeapon(WeaponAddDto weaponAdd);
    }
}