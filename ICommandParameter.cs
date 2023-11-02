using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public interface ICommandParameter
    {
        public bool CanBeSuggested { get; }
        public string Name { get; } // myEnum [Enum(Player,NPC)]
        public string Description { get; }

        public bool IsValid(CommandSplit split, out object value);
    }
}
