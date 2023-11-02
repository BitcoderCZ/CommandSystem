using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public Command[] SubCommands = new Command[0];
        public ICommandParameter[] Parameters = new ICommandParameter[0];

        public abstract bool Run(object[] parameters, out object result);
    }
}
