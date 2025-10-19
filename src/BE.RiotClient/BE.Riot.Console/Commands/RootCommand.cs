using System.CommandLine;

namespace Commands;

public static class CommandFactory
{
    public static RootCommand Root()
    {
        RootCommand rootCommand = new ("Start")
        {
            new Option<int>("--name")
            {
                Description = "Summoner Name"
            },
            new Option<string>("--tag")
            {
                Description = "after #"
            }
        };

        return rootCommand;
    }
}