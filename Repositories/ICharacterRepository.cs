using RBG_API.Dtos;

namespace RBG_API.Repositories
{
    public interface ICharacterRepository
    {
        Task<ServiceResponse<List<CharacterGetDto>>> GetCharacters(int userId);
        Task<ServiceResponse<CharacterGetDto>> GetCharacter(int id);
        Task<ServiceResponse<List<CharacterGetDto>>> AddCharacter(CharacterAddDto character);
        Task<ServiceResponse<CharacterGetDto>> UpdateCharacter(CharacterUpdateDto character);
        Task<ServiceResponse<List<CharacterGetDto>>> DeleteCharacter(int id);

    }
}