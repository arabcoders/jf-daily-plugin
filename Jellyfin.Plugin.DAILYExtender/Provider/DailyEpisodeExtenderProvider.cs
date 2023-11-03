using Jellyfin.Plugin.DAILYExtender.Helpers;
using MediaBrowser.Controller.Configuration;
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
        protected readonly IServerConfigurationManager _config;
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger<DailyEpisodeExtenderProvider> _logger;
        protected readonly IFileSystem _fileSystem;
        protected readonly System.IO.Abstractions.IFileSystem _afs;

        public DailyEpisodeExtenderProvider(IFileSystem fileSystem,
            IHttpClientFactory httpClientFactory,
            ILogger<DailyEpisodeExtenderProvider> logger,
            IServerConfigurationManager config,
            System.IO.Abstractions.IFileSystem afs)
        {
            _config = config;
            _fileSystem = fileSystem;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _afs = afs;
        }

        public string Name => Constants.PLUGIN_NAME;

        public bool HasChanged(BaseItem item, IDirectoryService directoryService)
        {
            _logger.LogDebug("ALP HasChanged: {Path}", item.Path);

            FileSystemMetadata fileInfo = _fileSystem.GetFileSystemInfo(item.Path);

            var result = fileInfo.Exists && DateTime.UtcNow.Subtract(fileInfo.LastWriteTimeUtc).Days <= 10;

            _logger.LogDebug("ALP HasChanged Result: {result}", result.ToString());

            return result;
        }

        public Task<MetadataResult<Episode>> GetMetadata(EpisodeInfo info, CancellationToken cancellationToken)
        {
            var result = Task.FromResult(new MetadataResult<Episode>());

            // ignore youtube content due to it's overriding yt-info reader provider.
            if (Utils.IsYouTubeContent(info.Path))
            {
                _logger.LogDebug("ALP GetMetadata: Ignoring Path {Path}", info.Path);
                return result;
            }

            // ignore non daily content.
            if (!Utils.IsDailyContent(info.Path))
            {
                _logger.LogDebug("ALP GetMetadata: Ignoring Non daily content {Path}", info.Path);
                return result;
            }

            _logger.LogDebug("ALP GetMetadata: {Path}", info.Path);

            var dto = Utils.Parse(info.Path);

            if (dto == null || dto.Year == null)
            {
                _logger.LogDebug("ALP GetMetadata: {Path} - No DTO", info.Path);
                return result;
            }

            _logger.LogDebug("ALP GetMetadata Result: {DTO}", dto.ToString());

            return Task.FromResult(Utils.DTOToEpisode(dto));
        }

        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(EpisodeInfo searchInfo, CancellationToken cancellationToken) => throw new NotImplementedException();

        public virtual Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
