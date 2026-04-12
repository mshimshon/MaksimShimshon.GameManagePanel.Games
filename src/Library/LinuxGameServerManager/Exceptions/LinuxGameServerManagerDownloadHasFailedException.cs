using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;

namespace GameHost.Games.Lib.LinuxGameServerManager.Exceptions;

public class LinuxGameServerManagerDownloadHasFailedException : Exception
{
    public LinuxGameServerManagerDownloadHasFailedException(LinuxCommandResult linuxCommandResult) : base("Couldn't not download LGSM, failed")
    {
        LinuxCommandResult = linuxCommandResult;
    }

    public LinuxCommandResult LinuxCommandResult { get; }
}
