using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RBG_API.Data;
using RBG_API.Dtos;

namespace RBG_API.Repositories;

public class CharacterRepository : ICharacterRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CharacterRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public  async Task<ServiceResponse<List<CharacterGetDto>>> GetCharacters(int userId)
    {
        var characters = await _context.Characters.Where(c => c.User.Id == userId).ToListAsync();

        return new ServiceResponse<List<CharacterGetDto>>()
        {
            Data = characters.Select(c => _mapper.Map<CharacterGetDto>(c)).ToList()
        };
    }


    public async Task<ServiceResponse<CharacterGetDto>> GetCharacter(int id)
    {
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
        
        return new ServiceResponse<CharacterGetDto>()
        {
            Data =_mapper.Map<CharacterGetDto>(character)
        }; 
    }

    public async Task<ServiceResponse<List<CharacterGetDto>>> AddCharacter(CharacterAddDto character)
    {
        _context.Characters.Add(_mapper.Map<Character>(character));
        await _context.SaveChangesAsync();

        return new ServiceResponse<List<CharacterGetDto>>()
        {
            Data = await _context.Characters
            .Select(c => _mapper.Map<CharacterGetDto>(c))
            .ToListAsync()
        };
    }

    public async Task<ServiceResponse<CharacterGetDto>> UpdateCharacter(CharacterUpdateDto character)
    {
        var response = new ServiceResponse<CharacterGetDto>();
        try
        {
            Character OldCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);

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
            var character = await _context.Characters.FirstAsync(c => c.Id == id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

        response.Data = await _context.Characters.Select(c=> _mapper.Map<CharacterGetDto>(c)).ToListAsync();

        } catch(Exception e)
        {
            response.Sucess = false;
            response.Message = e.Message;
        }
        return response;

    }
}
