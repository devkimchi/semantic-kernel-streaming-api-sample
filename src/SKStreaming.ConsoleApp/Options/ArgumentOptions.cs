using System;

namespace SKStreaming.ConsoleApp.Options;

public class ArgumentOptions
{
    public QuestionType QuestionType { get; set; }
    public bool Help { get; set; } = false;

    public static ArgumentOptions Parse(string[] args)
    {
        var options = new ArgumentOptions();
        if (args.Length == 0)
        {
            return options;
        }

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-t":
                case "--question-type":
                    if (i < args.Length - 1)
                    {
                        options.QuestionType = Enum.TryParse<QuestionType>(args[++i].Replace("-", ""), ignoreCase: true, out var result)
                            ? result
                            : QuestionType.Undefined;
                    }
                    break;

                case "-h":
                case "--help":
                    options.Help = true;
                    break;
            }
        }

        return options;
    }
}
