using Jellyfin.Plugin.DAILYExtender.Helpers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.DAILYExtender.Provider
{
    public class DailyEpisodeExtenderProvider : IRemoteMetadataProvider<Episode, EpisodeInfo>, IHasItemChangeMonitor
    {
        protected readonly ILogger<DailyEpisodeExtenderProvider> _logger;

        public DailyEpisodeExtenderProvider(ILogger<DailyEpisodeExtenderProvider> logger)
        {
            _logger = logger;
        }

        public string Name => Constants.PLUGIN_NAME;

        public bool HasChanged(BaseItem item, IDirectoryService directoryService)
        {
            _logger.LogDebug($"DEP HasChanged: {item.Path}");

            FileSystemMetadata fileInfo = directoryService.GetFile(item.Path);
            var result = fileInfo.Exists && fileInfo.LastWriteTimeUtc.ToUniversalTime() > item.DateLastSaved.ToUniversalTime();

            string status = result ? "Has Changed" : "Has Not Changed";

            _logger.LogDebug($"DEP HasChanged Result: {status}");

            return result;
        }

        public Task<MetadataResult<Episode>> GetMetadata(EpisodeInfo info, CancellationToken cancellationToken)
        {
            var result = Task.FromResult(new MetadataResult<Episode>());

            // ignore non daily content.
            if (!Utils.IsDailyContent(info.Path))
            {
                _logger.LogDebug($"DEP GetMetadata: Ignoring Non daily content {info.Path}");
                return result;
            }

            _logger.LogDebug($"DEP GetMetadata: {info.Path}", info.Path);

            var dto = Utils.Parse(info.Path);

            if (dto == null || dto.Year == null)
            {
                _logger.LogDebug($"DEP GetMetadata: {info.Path} - No DTO");
                return result;
            }

            _logger.LogDebug($"DEP GetMetadata Result: {dto}");

            return Task.FromResult(Utils.DTOToEpisode(dto));
        }

        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(EpisodeInfo searchInfo, CancellationToken cancellationToken) => throw new NotImplementedException();

        public virtual Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
