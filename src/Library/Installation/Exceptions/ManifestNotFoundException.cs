namespace GameHost.Games.Lib.Installation.Exceptions;

public class ManifestNotFoundException : Exception
{
    public ManifestNotFoundException() : base("manifest.json was not found")
    {
    }
}
