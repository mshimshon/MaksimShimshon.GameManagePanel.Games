namespace GameHost.Games.Lib.Installation.Exceptions;

public class ModServiceUnavailableException : Exception
{
    // TODO: LOCALIZE
    public ModServiceUnavailableException() : base("Mods are not supported for that game server at the moment, try updating the console or the game server.")
    {
    }
}
