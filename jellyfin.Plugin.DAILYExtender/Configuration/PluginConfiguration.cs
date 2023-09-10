using MediaBrowser.Model.Plugins;
using System;

public enum SomeOptions
{
    CustomDateParser = 1,
}

namespace jellyfin.Plugin.DAILYExtender.Configuration
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
