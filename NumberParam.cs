using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public abstract class NumberParam<T> : ICommandParameter
    {
        public bool CanBeSuggested => false;
        public string Name
        {
            get {
                string s = $"[{typeof(T)}]";
                if (!string.IsNullOrEmpty(name))
                    s = s + " " + name;

                return s;
            }
        }
        private string name;
        public string Description { get; private set; }

        public NumberParam(string _name, string _description)
        {
            name = _name;
            Description = _description;
        }

        public bool IsValid(CommandSplit split, out object value)
        {
            Type t = typeof(T);

            object[] parametes = new object[]
            {
                split.Value,
                (t == typeof(float) || t == typeof(double)) ? NumberStyles.Float | NumberStyles.AllowThousands : NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                null
            };
            MethodInfo tryParse = t.GetMethod("TryParse", BindingFlags.Public
                | BindingFlags.Static, new Type[]
            {
                typeof(string),
                typeof(NumberStyles),
                typeof(IFormatProvider),
                t.MakeByRefType(),
            });
            if (tryParse is null)
                throw new Exception($"{t} doesn't contain method \"TryParse\"");
            
            bool isValid = (bool)tryParse.Invoke(null, parametes);

            value = parametes[3];
            return isValid;
        }
    }

    public class IntParam : NumberParam<int>
    {
        public IntParam(string _name = "", string _description = "No description")
            : base(_name, _description)
        { }
    }

    public class FloatParam : NumberParam<float>
    {
        public FloatParam(string _name = "", string _description = "No description")
            : base(_name, _description)
        { }
    }
}
