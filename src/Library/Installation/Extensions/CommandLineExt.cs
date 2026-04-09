using GameHost.Games.Lib.Installation.Contracts.Responses;
using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;
using System.CommandLine;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameHost.Games.Lib.Installation.Extensions;

internal static class CommandLineExt
{
    private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true
    };
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
            var json = JsonSerializer.Serialize(outResult, _jsonSerializerOptions);
            Console.Out.WriteLine(json);
            success = true;
            return success;
        }
        catch (Exception ex)
        {
            var outResult = new ResultResponse()
            {
                Error = new ErrorResponse(ex)
            };
            var json = JsonSerializer.Serialize(outResult, _jsonSerializerOptions);
            //Console.Error.WriteLine(ex);
            // TODO: LOG
            Console.Out.WriteLine(json);
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
