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
        [InlineData("Series Title - 2023-09-10 - this is a test title [720p [720p].mkv", true)]
        [InlineData("Series Title - 230910 - this is a test title [720p [720p].mkv", true)]
        [InlineData("this is a test title 230910.mkv", true)]
        public void ParseFilesCorrectly(string fn, bool expected)
        {
            var dto = Utils.Parse(fn);
            Assert.Equal(expected, dto.Parsed);
            if (false == expected)
            {
                return;
            }

            var expectedDTO = new DTO
            {
                Parsed = true,
                Date = "2023-09-10",
                Title = "this is a test title",
                Year = "2023",
                Season = "2023",
                File = fn
            };

            Assert.Equal(expectedDTO.Date, dto.Date);
            Assert.Equal(expectedDTO.Title, dto.Title);
            Assert.Equal(expectedDTO.Year, dto.Year);
            Assert.Equal(expectedDTO.Season, dto.Season);
            Assert.Equal(expectedDTO.File, dto.File);
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
