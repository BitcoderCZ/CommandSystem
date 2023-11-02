using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSystem
{
    public class CommandHandler
    {
        private Command[] commands;
        public string Prefix;
        public bool AllowHelp;

        public CommandHandler(Command[] _commands, string _prefix, bool _allowHelp = true)
        {
            commands = _commands;
            Prefix = _prefix;
            AllowHelp = _allowHelp;
        }

        public string Autocomplete(string input)
        {

        }

        public CommandHandlerResult RunCommand(string input, bool trim, out object result)
        {
            result = null;

            if (trim)
                input = input.Trim();

            if (!string.IsNullOrWhiteSpace(Prefix)) {
                int prefixStart;
                if ((prefixStart = input.IndexOf(Prefix)) >= 0) {
                    if (prefixStart > 0)
                        input = input.Substring(0, prefixStart) + input.Substring(prefixStart + Prefix.Length);
                    else
                        input = input.Substring(Prefix.Length);
                }
                else
                    return new CommandHandlerResult($"Prefix ({Prefix}) wasn't found",
                        new TextSpan(0, input.Length), CommandHandlerStatus.PrefixNotFound)
                    {
                        input = input
                    };
            }

            CommandHandlerResult parseRes = parseInput(input, out CommandSplit[] splits);
            parseRes.input = input;
            if (!parseRes.IsOk)
                return parseRes;
            else if (splits.Length < 1)
                return CommandHandlerResult.Error;

            string commandName = splits[0].Value;

            for (int i = 0; i < commands.Length; i++) {
                Command command = commands[i];
                if (command.Name != commandName)
                    continue;

                CommandHandlerResult res = runCommand(command, splits, 1, out result);
                res.input = input;
                return res;
            }

            return CommandHandlerResult.CommandNotFound;
        }

        private CommandHandlerResult runCommand(Command command, CommandSplit[] splits, 
            int arrayStart, out object result) {

            if (splits[arrayStart].Value == "help" && AllowHelp) {
                PrintHelp(command, 0);

                result = null;
                return CommandHandlerResult.Ok;
            }
            else if (command.SubCommands.Length == 0 && command.Parameters.Length == 0) {
                if (splits.Length - arrayStart != 0) {
                    result = null;
                    return CommandHandlerResult.Error;
                }
                else
                    return command.Run(new object[0], out result) ? CommandHandlerResult.Ok : CommandHandlerResult.Error;
            }
            else if (command.SubCommands.Length > 0) {
                for (int i = 0; i < command.SubCommands.Length; i++)
                    if (command.SubCommands[i].Name == splits[arrayStart].Value)
                        return runCommand(command.SubCommands[i], splits, arrayStart + 1, out result);
            }
            else if (command.Parameters.Length == 0)
                return command.Run(new object[0], out result) ? CommandHandlerResult.Ok : CommandHandlerResult.Error;

            CommandHandlerResult res = parseParameters(command.Parameters, splits, arrayStart, out object[] paramValues);
            if (res.IsOk)
                return command.Run(paramValues, out result) ? CommandHandlerResult.Ok : CommandHandlerResult.Error;
            else {
                result = null;
                return res;
            }
        }

        private void PrintHelp(Command command, int offset)
        {
            string off = new string(' ', offset * 4);
            Console.WriteLine($"{off}{command.Name} - {command.Description}");
            for (int i = 0; i < command.Parameters.Length; i++)
                Console.WriteLine(off + command.Parameters[i].Name + " - " + command.Parameters[i].Description);

            for (int i = 0; i < command.SubCommands.Length; i++)
                PrintHelp(command.SubCommands[i], offset + 1);
        }

        private CommandHandlerResult parseParameters(ICommandParameter[] parameters, CommandSplit[] splits,
            int arrayStart, out object[] values) 
        { 
            values = new object[parameters.Length];
            if (parameters.Length != splits.Length - arrayStart)
                return new CommandHandlerResult($"Incorrect number of parametrs, {splits.Length - arrayStart} expected {parameters.Length}",
                    splits.Length == arrayStart ? new TextSpan(splits[arrayStart - 1].Span.End + 1, 0)
                    : TextSpan.FromSpans(splits[arrayStart].Span, splits[splits.Length - 1].Span));

            for (int i = 0; i < parameters.Length; i++) {
                if (parameters[i].IsValid(splits[i + arrayStart], out object value))
                    values[i] = value;
                else
                    return new CommandHandlerResult($"For parameter \"{parameters[i].Name}\" input isn't valid",
                        splits[i + arrayStart].Span);
            }

            return CommandHandlerResult.Ok;
        }

        private CommandHandlerResult parseInput(string input, out CommandSplit[] splits)
        {
            List<CommandSplit> result = new List<CommandSplit>();
            int segmentStart = 0;
            bool inQuotes = false;

            for (int i = 0; i < input.Length; i++) {
                char c = input[i];

                if (c == '"') {
                    if (!inQuotes) {
                        segmentStart = i + 1;
                        inQuotes = true;
                    }
                    else {
                        if (segmentStart == i)
                            result.Add(new CommandSplit(string.Empty, true, new TextSpan(segmentStart - 1, i)));
                        else
                            result.Add(new CommandSplit(input.Substring(segmentStart, i - segmentStart),
                                true, TextSpan.FromSpan(segmentStart - 1, i)));
                        inQuotes = false;
                        segmentStart = i + 1;
                    }
                } else if (c == ' ' && !inQuotes) {
                    if (segmentStart != i)
                        result.Add(new CommandSplit(input.Substring(segmentStart, i - segmentStart), false,
                            TextSpan.FromSpan(segmentStart, i - 1)));
                    segmentStart = i + 1;
                }
            }

            if (inQuotes) {
                splits = null;
                return new CommandHandlerResult("Quotes weren't terminated",
                    TextSpan.FromSpan(segmentStart - 1, input.Length - 1));
            }

            if (segmentStart < input.Length)
                result.Add(new CommandSplit(input.Substring(segmentStart), false,
                    TextSpan.FromSpan(segmentStart, input.Length - 1)));

            splits = result.ToArray();
            return CommandHandlerResult.Ok;
        }
    }
}
