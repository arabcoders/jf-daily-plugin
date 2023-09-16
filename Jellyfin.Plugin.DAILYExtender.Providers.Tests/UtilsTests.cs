using Xunit;
using System;
using System.Text.Json;
using Jellyfin.Plugin.DAILYExtender.Helpers;

namespace Jellyfin.Plugin.DAILYExtender.Tests
{
    public class UtilsTest
    {
        [Theory]
        [InlineData("Foo", false)]
        [InlineData("20230910 - this is a test title [720p].mkv", true)]
        [InlineData("230910 - this is a test title [720p].mkv", true)]
        [InlineData("2023-09-10 - this is a test title [720p].mkv", true)]
        public void ParseFilesCorrectly(string fn, bool expected)
        {
            var dto = Utils.Parse(fn);
            Assert.Equal(expected, dto.Parsed);
            if (expected)
            {
                Assert.Equal("2023-09-10", dto.Date);
                Assert.Equal("this is a test title", dto.Title);
            }
        }

        [Fact]
        public void YTDLJsonToEpisodeTest()
        {
            var dto = Utils.Parse("20230910 - Foobar [720p].mkv");
            var result = Utils.DTOToEpisode(dto);

            Assert.True(dto.Parsed);
            Assert.True(result.HasMetadata);
            Assert.Equal("Foobar", result.Item.Name);
            Assert.Equal(2023, result.Item.ProductionYear);
            Assert.Equal("2023-09-10", (result.Item.PremiereDate ?? DateTime.Now).ToString("yyyy-MM-dd"));
            Assert.Equal("20230910-Foobar", result.Item.ForcedSortName);
            Assert.Equal(2023, result.Item.ParentIndexNumber);
        }
    }
}
