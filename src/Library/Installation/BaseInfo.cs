using GameHost.Core.Features;

namespace GameHost.Games.Lib.Installation;

public static class BaseInfo
{
    public const string USERNAME = "lgsm";
    public const string MODULE_INFO = "GameInstallLibrary";
    public const string PLUGIN_MODULE_NAME = LinuxGameServerKeys.MODULE_NAME;
    public const string MOCK_FOLDER = "mockup";
    public const string DISTRO_DEP_FILENAME_FORMAT = "dep_{0}_{1}";
    public const string DISTRO_DEP_FILENAME = "distro_dependencies.json";
    //$"dep_{distroInfo.Id.ToLower()}_{distroInfo.VersionId}.json"
    public static string[] dependencies = ["sudo"];
}
