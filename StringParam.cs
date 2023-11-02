using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public class StringParam : ICommandParameter
    {
        public bool CanBeSuggested => false;

        public string Name
        {
            get {
                string s = "[String]";
                if (!string.IsNullOrEmpty(name))
                    s = s + " " + name;

                return s;
            }
        }
        private string name;
        public string Description { get; private set; }

        public StringParam(string _name = "", string _description = "No description")
        {
            name = _name;
            Description = _description;
        }

        public bool IsValid(CommandSplit split, out object value)
        {
            value = split.Value;
            if (!split.WasInQuotes)
                return false;

            return true;
        }
    }
}
