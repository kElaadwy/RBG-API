using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBG_API.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public Character character { get; set; }
        public int CharacterId { get; set; }
    }
}