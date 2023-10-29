using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.DAILYExtender.Helpers;

namespace Jellyfin.Plugin.DAILYExtender.Provider
{
    public abstract class AbstractLocalProvider<B, T> : ILocalMetadataProvider<T> where T : BaseItem
    {
        protected readonly ILogger<B> _logger;
        protected readonly IFileSystem _fileSystem;

        /// <summary>
        /// Providers name, this appears in the library metadata settings.
        /// </summary>
        public abstract string Name { get; }

        public AbstractLocalProvider(IFileSystem fileSystem, ILogger<B> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves metadata of item.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="directoryService"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<MetadataResult<T>> GetMetadata(ItemInfo info, IDirectoryService directoryService, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<T>();

            // ignore youtube content due to it's overriding yt-info reader provider.
            if (Utils.IsYouTubeContent(info.Path))
            {
                _logger.LogDebug("GetMetadata: Ignoring Path {Path}", info.Path);
                return Task.FromResult(result);
            }

            _logger.LogDebug("DELocal GetMetadata: {Path}", info.Path);

            var dto = Utils.Parse(info.Path);

            if (dto == null || dto.Year == null)
            {
                _logger.LogDebug("DELocal GetMetadata: {Path} - No DTO", info.Path);
                return Task.FromResult(result);
            }

            _logger.LogDebug("DELocal GetMetadata Result: {DTO}", dto.ToString());
            result = this.GetMetadataImpl(dto);

            return Task.FromResult(result);
        }

        internal abstract MetadataResult<T> GetMetadataImpl(DTO jsonObj);
    }
}
