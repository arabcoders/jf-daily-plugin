using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Entities.TV;
using Jellyfin.Plugin.DAILYExtender.Helpers;

namespace Jellyfin.Plugin.DAILYExtender.Provider
{
    public class LocalEpisodeProvider : AbstractLocalProvider<LocalEpisodeProvider, Episode>
    {
        public LocalEpisodeProvider(IFileSystem fileSystem, ILogger<LocalEpisodeProvider> logger) : base(fileSystem, logger) { }

        public override string Name => Constants.PLUGIN_NAME;

        internal override MetadataResult<Episode> GetMetadataImpl(DTO dto)
        {
            return Utils.DTOToEpisode(dto);
        }
    }
}
