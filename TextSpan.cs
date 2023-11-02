using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public struct TextSpan
    {
        public int Start;
        public int Length;
        public int End { get => Start + Length - 1; }

        public TextSpan(int _start, int _length)
        {
            Start = _start;
            Length = _length;
        }

        public static TextSpan FromSpan(int start, int end)
            => new TextSpan(start, end - start + 1);
        public static TextSpan FromSpans(TextSpan a, TextSpan b)
            => FromSpan(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));

        public string GetWith(string s)
            => s.Substring(Start, Length);
        public (string before, string after) GetWithout(string s)
            => (s.Substring(0, Start), s.Substring(End + 1));
    }
}
