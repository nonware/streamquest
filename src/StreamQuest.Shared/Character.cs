using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamQuest.Shared
{
    public class Character
    {
        public string Name { get; set; }
    }

    public class PlayerCharacter : Character
    { 
    }

    public class NonPlayerCharacter : Character
    {
    }
}
