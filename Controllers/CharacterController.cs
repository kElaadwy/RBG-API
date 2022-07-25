using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RBG_API.Dtos;
using RBG_API.Repositories;

namespace RBG_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController: ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;


        public CharacterController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        [HttpGet]
        public  async Task<ActionResult<ServiceResponse<List<CharacterGetDto>>>> GetCharacters()
        {
            return Ok(await _characterRepository.GetCharacters());
        }

        [HttpGet("{id}")]
        public  async Task<ActionResult<ServiceResponse<CharacterGetDto>>> GetCharacter(int id)
        {
            return Ok(await _characterRepository.GetCharacter(id));
        }

        [HttpPost]
        public  async Task<ActionResult<ServiceResponse<List<CharacterGetDto>>>> AddCharacter(CharacterAddDto character)
        {
            return Ok(await _characterRepository.AddCharacter(character));
        }

        [HttpPut]
        public  async Task<ActionResult<ServiceResponse<CharacterGetDto>>> UpdateCharacter(CharacterUpdateDto character)
        {
            var response = await _characterRepository.UpdateCharacter(character);

            if(response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public  async Task<ActionResult<ServiceResponse<List<CharacterGetDto>>>> DeleteCharacter(int id)
        {
            var response = await _characterRepository.DeleteCharacter(id);

            if(response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

    }
}