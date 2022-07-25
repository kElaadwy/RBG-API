using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBG_API.Dtos
{
    public class CharacterUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "no name";
        public int HitPoints { get; set; } = 100;
        public int Stringth { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;

    }
}