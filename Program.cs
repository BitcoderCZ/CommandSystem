namespace CommandSystem
{
    static class Program
    {
        static void Main()
        {
            CommandHandler handler = new CommandHandler(new Command[]
            {
                new SetPos()
            }, "/");
            while (true) {
                CommandHandlerResult status = handler.RunCommand(Console.ReadLine(), true, out object result);
                if (!status.IsOk)
                    status.Print();
                else
                    Console.WriteLine(result);
            }

            Console.ReadKey(true);
        }
    }

    class SetPos : Command
    {
        public override string Name => "setpos";
        public override string Description => "Sets player's position";

        public SetPos()
        {
            SubCommands = new Command[]
            {
                new OrNot()
            };

            Parameters = new ICommandParameter[]
            {
                new FloatParam(),
                new StringParam(),
                new EnumParam(new string[] { "NPC", "Bitch" })
            };
        }

        public override bool Run(object[] parameters, out object result)
        {
            result = "";
            for (int i = 0; i < parameters.Length; i++)
                result += parameters[i].ToString();

            return true;
        }
    }

    class OrNot : Command
    {
        public override string Name => "OrNot";

        public override string Description => "OrNot????";

        public OrNot()
        {
            Parameters = new ICommandParameter[]
            {
                new StringParam("or not string", "THE or not string"),
            };
        }

        public override bool Run(object[] parameters, out object result)
        {
            result = "";
            for (int i = 0; i < parameters.Length; i++)
                result += parameters[i].ToString();

            result += " ....... or not?????";
            return true;
        }
    }
}