using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBG_API.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "no name";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public User? User { get; set; }
        public Weapon weapon { get; set; }
    }
}