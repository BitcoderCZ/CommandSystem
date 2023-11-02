using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public class EnumParam : ICommandParameter
    {
        public bool CanBeSuggested => true;
        public string Name
        {
            get {
                string s = "[Enum(";
                for (int i = 0; i < Values.Length; i++)
                    s += Values[i] + (i < Values.Length - 1 ? ", " : "");

                s += ")]";

                if (!string.IsNullOrEmpty(name))
                    s += s + " " + name;

                return s;
            }
        }
        private string name;
        public string Description { get; private set; }

        public string[] Values;

        public EnumParam(string[] _values, string _name = "", string _description = "No description")
        {
            Values = _values;
            name = _name;
            Description = _description;
        }

        public bool IsValid(CommandSplit split, out object value)
        {
            value = split.Value;
            return Values.Contains(split.Value);
        }
    }
}
