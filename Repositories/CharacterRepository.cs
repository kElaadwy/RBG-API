using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RBG_API.Data;
using RBG_API.Dtos;

namespace RBG_API.Repositories;

public class CharacterRepository : ICharacterRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterRepository(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public  async Task<ServiceResponse<List<CharacterGetDto>>> GetCharacters()
    {
        var characters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();

        return new ServiceResponse<List<CharacterGetDto>>()
        {
            Data = characters.Select(c => _mapper.Map<CharacterGetDto>(c)).ToList()
        };
    }


    public async Task<ServiceResponse<CharacterGetDto>> GetCharacter(int id)
    {
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
        
        return new ServiceResponse<CharacterGetDto>()
        {
            Data =_mapper.Map<CharacterGetDto>(character)
        }; 
    }

    public async Task<ServiceResponse<List<CharacterGetDto>>> AddCharacter(CharacterAddDto caracterAdd)
    {
        Character character = _mapper.Map<Character>(caracterAdd);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

        return new ServiceResponse<List<CharacterGetDto>>()
        {
            Data = await _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<CharacterGetDto>(c))
                .ToListAsync()
        };
    }

    public async Task<ServiceResponse<CharacterGetDto>> UpdateCharacter(CharacterUpdateDto character)
    {
        var response = new ServiceResponse<CharacterGetDto>();
        try
        {
            Character OldCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == character.Id && c.User.Id == GetUserId());

            if(OldCharacter is null)
            {
                response.Sucess = false;
                response.Message = "character is not found!";

                return response;
            }
            
            OldCharacter.Name = character.Name;
            OldCharacter.HitPoints = character.HitPoints;
            OldCharacter.Strength = character.Strength;
            OldCharacter.Defence = character.Defence;
            OldCharacter.Intelligence = character.Intelligence;
            OldCharacter.Class = character.Class;

            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<CharacterGetDto>(OldCharacter);

        } catch(Exception e)
        {
            response.Sucess = false;
            response.Message = e.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<List<CharacterGetDto>>> DeleteCharacter(int id)
    {
        var response = new ServiceResponse<List<CharacterGetDto>>();
        try
        {
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

            if(character is null)
            {
                response.Sucess = false;
                response.Message ="character is not found!";
                return response;
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            response.Data = await _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .Select(c=> _mapper.Map<CharacterGetDto>(c))
                .ToListAsync();

        } catch(Exception e)
        {
            response.Sucess = false;
            response.Message = e.Message;
        }
        return response;
    }

    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
