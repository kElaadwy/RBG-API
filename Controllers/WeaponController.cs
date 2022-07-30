using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBG_API.Dtos;
using RBG_API.Repositories;

namespace RBG_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController: ControllerBase
    {
        private readonly IWeaponRepository _weaponRepository;

        public WeaponController(IWeaponRepository weaponRepository)
        {
            _weaponRepository = weaponRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<CharacterGetDto>>> AddWeapon(WeaponAddDto weaponAdd)
        {
            return Ok(_weaponRepository.AddWeapon(weaponAdd));
        }
    }
}