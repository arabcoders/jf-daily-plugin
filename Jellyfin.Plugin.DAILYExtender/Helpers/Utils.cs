using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Jellyfin.Plugin.DAILYExtender.Helpers
{
    public class Utils
    {
        /// <summary>
        /// Reads JSON data from file.
        /// </summary>
        /// <param name="metaFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static DTO ReadYTDLInfo(string fpath, FileSystemMetadata path, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string jsonString = File.ReadAllText(fpath);
            DTO data = JsonSerializer.Deserialize<DTO>(jsonString, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            });
            data.file_path = path;
            return data;
        }

        /// <summary>
        /// Provides a Episode Metadata Result from a json object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static MetadataResult<Episode> YTDLJsonToEpisode(DTO json)
        {
            var item = new Episode();
            var result = new MetadataResult<Episode>
            {
                HasMetadata = true,
                Item = item
            };
            result.Item.Name = json.title;
            result.Item.Overview = json.description;
            var date = new DateTime(1970, 1, 1);
            try
            {
                date = DateTime.ParseExact(json.upload_date, "yyyyMMdd", null);
            }
            catch { }
            result.Item.ProductionYear = date.Year;
            result.Item.PremiereDate = date;
            result.Item.ForcedSortName = date.ToString("yyyyMMdd") + "-" + result.Item.Name;
            result.Item.IndexNumber = int.Parse("1" + date.ToString("MMdd"));
            result.Item.ParentIndexNumber = int.Parse(date.ToString("yyyy"));
            result.Item.ProviderIds.Add(Constants.PLUGIN_NAME, json.id);

            // If the json data has epoch, do not bother calling file data.
            if (json.epoch != null)
            {
                result.Item.IndexNumber = int.Parse("1" + date.ToString("MMdd") + DateTimeOffset.FromUnixTimeSeconds(json.epoch ?? new long()).ToString("hhmm"));
                return result;
            }

            // if no json.epoch is found fallback to file last modification time.
            if (json.file_path != null)
            {
                result.Item.IndexNumber = int.Parse("1" + date.ToString("MMdd") + json.file_path.LastWriteTimeUtc.ToString("hhmm"));
            }

            return result;
        }

        /// <summary>
        /// Provides a MusicVideo Metadata Result from a json object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static MetadataResult<Series> YTDLJsonToSeries(DTO json)
        {
            var item = new Series();
            var result = new MetadataResult<Series>
            {
                HasMetadata = true,
                Item = item
            };
            result.Item.Name = json.uploader;
            result.Item.Overview = json.description;
            result.Item.ProviderIds.Add(Constants.PLUGIN_NAME, json.channel_id);
            return result;
        }
    }

}
