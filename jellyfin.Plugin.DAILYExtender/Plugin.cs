using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using jellyfin.Plugin.DAILYExtender.Configuration;
using jellyfin.Plugin.DAILYExtender.Helpers;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace jellyfin.Plugin.DAILYExtender
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public override string Name => Constants.PLUGIN_NAME;
        public static Plugin Instance { get; private set; }
        public override Guid Id => Guid.Parse(Constants.PLUGIN_GUID);
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[] {
                new PluginPageInfo {
                    Name = this.Name,
                    EmbeddedResourcePath = string.Format("{0}.Configuration.configPage.html", GetType().Namespace)
                }
            };
        }
    }

    /// <summary>
    /// Register webhook services.
    /// </summary>
    public class PluginServiceRegistrator : IPluginServiceRegistrator
    {
        /// <inheritdoc />
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<System.IO.Abstractions.IFileSystem, FileSystem>();
        }
    }
}
