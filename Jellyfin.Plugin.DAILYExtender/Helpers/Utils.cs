using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Jellyfin.Plugin.DAILYExtender.Helpers
{
    public class Utils
    {
        /// <summary>
        /// Parse filename for metadata.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DTO Parse(string fileName)
        {
            var fn = Path.GetFileNameWithoutExtension(fileName);
            var dto = new DTO { File = fileName };

            foreach (var pattern in Constants.Patterns)
            {
                var rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (rx.IsMatch(fn))
                {
                    return MakeDTO(dto, rx.Matches(fn));
                }
            }

            return dto;
        }

        protected static DTO MakeDTO(DTO dto, MatchCollection match)
        {
            var titlePrefix = "";

            var year = match[0].Groups["year"].ToString();
            if (year.Length == 2)
            {
                year = "20" + year;
            }

            dto.Year = year;
            dto.Date = dto.Year + "-" + match[0].Groups["month"].ToString() + "-" + match[0].Groups["day"].ToString();
            dto.Episode = "1" + match[0].Groups["month"].ToString() + match[0].Groups["day"].ToString();
            dto.Title = match[0].Groups["title"].ToString();
            if (!string.IsNullOrEmpty(dto.Title))
            {
                var epNumber = match[0].Groups["epNumber"].ToString();
                var episode = match[0].Groups["episode"].ToString();

                if (!string.IsNullOrEmpty(epNumber))
                {
                    titlePrefix = epNumber + " - ";
                }
                if (string.IsNullOrEmpty(titlePrefix) && !string.IsNullOrEmpty(episode))
                {
                    titlePrefix = episode + " - ";
                }

                if (!string.IsNullOrEmpty(titlePrefix))
                {
                    dto.Title = dto.Title.Trim();
                    dto.Title = titlePrefix + dto.Title;
                }

                dto.Title = Regex.Replace(dto.Title, @"\[.+\]", "").Trim();
                dto.Title = dto.Title.Trim();
            }

            if (string.IsNullOrEmpty(dto.Title))
            {
                dto.Title = dto.Date;
            }

            dto.Season = dto.Year;
            dto.Parsed = true;

            return dto;
        }

        /// <summary>
        /// Provides a Episode Metadata Result from a json object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static MetadataResult<Episode> DTOToEpisode(DTO dto)
        {
            var item = new Episode();
            var result = new MetadataResult<Episode>
            {
                HasMetadata = true,
                Item = item
            };

            result.Item.Name = dto.Title;
            var date = new DateTime(1970, 1, 1);
            try
            {
                date = DateTime.ParseExact(dto.Date, "yyyy-MM-dd", null);
            }
            catch { }
            result.Item.ProductionYear = date.Year;
            result.Item.PremiereDate = date;
            result.Item.ForcedSortName = date.ToString("yyyyMMdd") + "-" + result.Item.Name;
            result.Item.ParentIndexNumber = int.Parse(dto.Season);
            result.Item.IndexNumber = int.Parse(dto.Episode);

            return result;
        }
    }

}
