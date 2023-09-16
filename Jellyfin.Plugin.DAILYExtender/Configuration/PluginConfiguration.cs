using MediaBrowser.Model.Plugins;

public enum SomeOptions
{
    CustomDateParser = 1,
}

namespace Jellyfin.Plugin.DAILYExtender.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public SomeOptions IDType { get; set; }
        public PluginConfiguration()
        {
            // defaults
            IDType = SomeOptions.CustomDateParser;
        }
    }
}
