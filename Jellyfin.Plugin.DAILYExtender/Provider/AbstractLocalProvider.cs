using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;
using System.IO;
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
            _logger.LogDebug("DELocal GetMetadata: {Path}", info.Path);
            var result = new MetadataResult<T>();

            var infoFile = Path.ChangeExtension(info.Path, "info.json");

            if (!File.Exists(infoFile))
            {
                return Task.FromResult(result);
            }

            var jsonObj = Utils.ReadYTDLInfo(infoFile, directoryService.GetFile(info.Path), cancellationToken);
            _logger.LogDebug("YTLocal GetMetadata Result: {JSON}", jsonObj.ToString());
            result = this.GetMetadataImpl(jsonObj);

            return Task.FromResult(result);
        }

        internal abstract MetadataResult<T> GetMetadataImpl(DTO jsonObj);
    }
}
