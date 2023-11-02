using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public struct CommandHandlerResult
    {
        public static readonly CommandHandlerResult Ok = new CommandHandlerResult(
            CommandHandlerStatus.Ok);
        public static readonly CommandHandlerResult PrefixNotFound = new CommandHandlerResult(
            CommandHandlerStatus.PrefixNotFound);
        public static readonly CommandHandlerResult NotTerminatedQuotes = new CommandHandlerResult(
            CommandHandlerStatus.NotTerminatedQuotes);
        public static readonly CommandHandlerResult CommandNotFound = new CommandHandlerResult(
            CommandHandlerStatus.CommandNotFound);
        public static readonly CommandHandlerResult Error = new CommandHandlerResult(
            CommandHandlerStatus.Error);

        public bool IsOk;
        public CommandHandlerStatus Status;
        public string Message;
        public TextSpan Span;

        internal string input = string.Empty;

        public CommandHandlerResult(CommandHandlerStatus _status)
        {
            IsOk =  _status == CommandHandlerStatus.Ok;
            Status = _status;
            Message = _status.ToString();
            Span = default;
        }

        public CommandHandlerResult(string _message, TextSpan _span,
            CommandHandlerStatus _status = CommandHandlerStatus.Error)
        {
            IsOk = false;
            Message = _message;
            Span = _span;
            Status = _status;
        }

        public void Print()
        {
            Console.WriteLine($"Error: {Message}");

            (string before, string after) without = Span.GetWithout(input);
            string error = Span.GetWith(input);
            Console.Write(without.before);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(error);
            Console.ResetColor();
            Console.WriteLine(without.after);
        }
    }

    public enum CommandHandlerStatus : byte
    {
        Ok = 0,
        PrefixNotFound = 1,
        NotTerminatedQuotes = 2,
        CommandNotFound = 3,
        Error = 255,
    }
}
