using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RBG_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController: ControllerBase
    {
        private List<Character> cs = new List<Character>
        {
            new Character(),
            new Character(){Id = 1, Name = "neco robin", Class = RpgClass.Mage}
        };

        [HttpGet]
        public ActionResult<Character> GetCharacters()
        {
            return Ok(cs);
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetCharacters(int id)
        {
            return Ok(cs.FirstOrDefault(c => c.Id == id));
        }

        [HttpPost]
        public ActionResult<Character> CreateCharacter(Character c)
        {
            cs.Add(c);
            return Ok(cs);
        }

    }
}