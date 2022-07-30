using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RBG_API.Data;
using RBG_API.Dtos;

namespace RBG_API.Repositories;

public class WeaponRepository : IWeaponRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeaponRepository(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<ServiceResponse<CharacterGetDto>> AddWeapon(WeaponAddDto weaponAdd)
    {
        var respons = new ServiceResponse<CharacterGetDto>();

        try
        {
            Character character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == weaponAdd.CharacterId && c.User.Id == GetUserId());

            if(character is null)
            {
                respons.Sucess = false;
                respons.Message = "character is not found!";
                return respons;
            }

            Weapon weapon = _mapper.Map<Weapon>(weaponAdd);
            weapon.character = character;
            _context.Weapons.Add(weapon);

            await _context.SaveChangesAsync();

            respons.Data = _mapper.Map<CharacterGetDto>(character);
        } 
        catch(Exception ex)
        {
            respons.Sucess = false;
            respons.Message = ex.Message;

        }
        return respons;
    }

    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    }
