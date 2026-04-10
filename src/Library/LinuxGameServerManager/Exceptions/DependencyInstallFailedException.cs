namespace GameHost.Games.Lib.LinuxGameServerManager.Exceptions;

public class DependencyInstallFailedException : Exception
{

    public DependencyInstallFailedException(params string[] dependecies) :
        base($"One or more of the following dependencies: {string.Join(' ', dependecies)} failed to installed.")
    {
        Dependecies = dependecies;
    }

    public string[] Dependecies { get; }
}
