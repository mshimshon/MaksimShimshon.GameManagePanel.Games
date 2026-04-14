using GameHost.Games.Lib.Installation.Contracts.Responses;
using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;
using GameHost.Games.Lib.Installation.Services;
using System.CommandLine;

namespace GameHost.Games.Lib.Installation.Extensions;

internal static class CommandLineExt
{
    private static PayloadBuilderService _payloadBuilderService = new PayloadBuilderService();
    internal static async Task<bool> ExecuteCommandAsync(Func<Task> task)
    {
        bool success = await ExecuteCommandAsync(async () =>
        {
            await task();
            return "Success";
        });
        return success;
    }
    internal static async Task<bool> ExecuteCommandAsync(Func<Task<object?>> task)
    {
        bool success = false;
        try
        {

            object? serviceResult = await task();
            var outResult = new ResultResponse()
            {
                Data = serviceResult
            };

            await _payloadBuilderService.PrintAsync(outResult);
            success = true;
            return success;
        }
        catch (Exception ex)
        {
            var outResult = new ResultResponse()
            {
                Error = new ErrorResponse(ex)
            };
            //Console.Error.WriteLine(ex);
            // TODO: LOG
            await _payloadBuilderService.PrintAsync(outResult);
            return success;
        }
    }
    internal static Command AddOption<T>(this Command command, string name, string alias, string desc)
    {
        var option = new Option<T>($"--{name}", $"-{alias}")
        {
            Description = desc
        };
        command.Options.Add(option);
        return command;
    }

    internal static Command PrintHelp(this Command command)
    {
        foreach (var option in command.Options)
        {
            Console.Error.WriteLine($"{string.Join(',', [option.Name, .. option.Aliases])} - {option.Description}");
        }
        foreach (Command subCommand in command.Subcommands)
        {
            Console.Error.WriteLine($"{string.Join(',', [subCommand.Name, .. subCommand.Aliases])} - {subCommand.Description}");
        }
        return command;
    }
    internal static RootCommand WithSubCommand(this RootCommand rootCommand, Command command)
    {
        rootCommand.Subcommands.Add(command);
        return rootCommand;
    }
}
