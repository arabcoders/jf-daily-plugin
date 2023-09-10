using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Entities.Movies;
using jellyfin.Plugin.DAILYExtender.Helpers;

namespace jellyfin.Plugin.DAILYExtender.Provider
{
    public class LocalMovieProvider : AbstractLocalProvider<LocalMovieProvider, Movie>
    {
        public override string Name => Constants.PLUGIN_NAME;
        public LocalMovieProvider(IFileSystem fileSystem, ILogger<LocalMovieProvider> logger) : base(fileSystem, logger) { }
        internal override MetadataResult<Movie> GetMetadataImpl(YTDLData jsonObj)
        {
            return Utils.YTDLJsonToMovie(jsonObj);
        }
    }
}
