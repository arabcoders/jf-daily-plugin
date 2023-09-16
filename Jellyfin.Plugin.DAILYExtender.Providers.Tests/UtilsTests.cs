using Xunit;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using Jellyfin.Plugin.DAILYExtender.Helpers;

namespace Jellyfin.Plugin.DAILYExtender.Tests
{
    public class UtilsTest
    {
        [Theory]
        [InlineData("Foo", "")]
        [InlineData("ChannelName - 20190113 - this is a test title [dQw4w9WgXcQ].mkv", "dQw4w9WgXcQ")]
        [InlineData("ChannelName - 20190113 - this is a test title [youtube-dQw4w9WgXcQ].mkv", "dQw4w9WgXcQ")]
        public void GetYouTubeChannelOrVideoIds(string fn, string expected)
        {
            var rxc = new Regex(Constants.VIDEO_RX, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var result = "";
            if (rxc.IsMatch(fn))
            {
                MatchCollection match = rxc.Matches(fn);
                result = match[0].Groups["id"].ToString();
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Foo", "")]
        [InlineData("ChannelName Videos [UCuAXFkgsw1L7xaCfnd5JJOw].info.json", "UCuAXFkgsw1L7xaCfnd5JJOw")]
        [InlineData("ChannelName Videos [youtube-UCuAXFkgsw1L7xaCfnd5JJOw].info.json", "UCuAXFkgsw1L7xaCfnd5JJOw")]
        [InlineData("ChannelName Videos [youtube-HCuAXFkgsw1L7xaCfnd5JJOw].info.json", "HCuAXFkgsw1L7xaCfnd5JJOw")]
        public void GetYouTubeChannelIds(string fn, string expected)
        {
            var rxc = new Regex(Constants.CHANNEL_RX, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var result = "";
            if (rxc.IsMatch(fn))
            {
                MatchCollection match = rxc.Matches(fn);
                result = match[0].Groups["id"].ToString();
            }

            Assert.Equal(expected, result);
        }

        [Fact]
        public void YTDLJsonToEpisodeTest()
        {
            var result = Utils.YTDLJsonToEpisode(GetYouTubeVideoData());
            Assert.True(result.HasMetadata);
            Assert.Equal("Never Gonna Give You Up", result.Item.Name);
            Assert.Equal("The official video for “Never Gonna Give You Up” by Rick Astley", result.Item.Overview);
            Assert.Equal(2009, result.Item.ProductionYear);
            Assert.Equal("20091025", (result.Item.PremiereDate ?? DateTime.Now).ToString("yyyyMMdd"));
            Assert.Equal("Rick Astley", result.People[0].Name);
            Assert.Equal("UCuAXFkgsw1L7xaCfnd5JJOw", result.People[0].ProviderIds[Constants.PLUGIN_NAME]);
            Assert.Equal("20091025-Never Gonna Give You Up", result.Item.ForcedSortName);
            Assert.Equal(110250725, result.Item.IndexNumber);
            Assert.Equal(2009, result.Item.ParentIndexNumber);
            Assert.Equal("dQw4w9WgXcQ", result.Item.ProviderIds[Constants.PLUGIN_NAME]);
        }

        [Fact]
        public void YTDLJsonToSeriesTest()
        {
            var result = Utils.YTDLJsonToSeries(GetYouTubeChannelData());
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(GetYouTubeChannelData()));
            Assert.True(result.HasMetadata);
            Assert.Equal("Rick Astley", result.Item.Name);
            Assert.Equal("Official YouTube channel for Rick Astley.", result.Item.Overview);
            Assert.Equal("UCuAXFkgsw1L7xaCfnd5JJOw", result.Item.ProviderIds[Constants.PLUGIN_NAME]);
        }

        public static DTO GetYouTubeVideoData()
        {
            string jsonString = "{\"id\":\"dQw4w9WgXcQ\",\"uploader\":\"Rick Astley\",\"upload_date\":\"20091025\",\"title\":\"Never Gonna Give You Up\",\"description\":\"The official video for “Never Gonna Give You Up” by Rick Astley\",\"channel_id\":\"UCuAXFkgsw1L7xaCfnd5JJOw\",\"track\":\"Music\",\"artist\":\"Rick Astley\",\"album\":null,\"epoch\":1673637911,\"file_path\":null,\"thumbnails\":null}";
            return JsonSerializer.Deserialize<DTO>(jsonString) ?? new DTO();
        }

        public static DTO GetYouTubeChannelData()
        {
            string jsonString = "{\"id\":\"UCuAXFkgsw1L7xaCfnd5JJOw\",\"uploader\":\"Rick Astley\",\"upload_date\":null,\"title\":\"Rick Astley\",\"description\":\"Official YouTube channel for Rick Astley.\",\"channel_id\":\"UCuAXFkgsw1L7xaCfnd5JJOw\",\"track\":null,\"artist\":null,\"album\":null,\"epoch\":1673637911,\"file_path\":null,\"thumbnails\":null}";
            return JsonSerializer.Deserialize<DTO>(jsonString) ?? new DTO();
        }
    }
}
