using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public struct CommandSplit
    {
        public string Value;
        public bool WasInQuotes;
        public TextSpan Span;

        public CommandSplit(string _value, bool _wasInQuotes, TextSpan _span)
        {
            Value = _value;
            WasInQuotes = _wasInQuotes;
            Span = _span;
        }

        public override string ToString() => Value;
    }
}
