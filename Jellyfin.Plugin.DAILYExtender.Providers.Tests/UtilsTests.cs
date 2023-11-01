using Xunit;
using System;
using System.Text.Json;
using Jellyfin.Plugin.DAILYExtender.Helpers;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.DAILYExtender.Tests
{
    public class UtilsTest
    {
        [Theory]
        [InlineData("Foo", false)]
        [InlineData("230910 - this is a test title [720p].mkv", true)]
        [InlineData("20230910 - this is a test title [720p].mkv", true)]
        [InlineData("2023-09-10 - this is a test title [720p].mkv", true)]
        [InlineData("2023_09_10 - this is a test title [720p].mkv", true)]
        [InlineData("2023.09.10 - this is a test title [720p].mkv", true)]

        // Date at the middle of file name.
        [InlineData("Series Title - 2023-09-10 - this is a test title [720p [720p].mkv", true)]
        [InlineData("Series Title - 2023_09_10 - this is a test title [720p [720p].mkv", true)]
        [InlineData("Series Title - 2023.09.10 - this is a test title [720p [720p].mkv", true)]
        [InlineData("Series Title - 20230910 - this is a test title [720p [720p].mkv", true)]

        // Date at the end of file name.
        [InlineData("this is a test title 230910.mkv", true)]
        [InlineData("this is a test title 20230910.mkv", true)]
        [InlineData("this is a test title 2023-09-10.mkv", true)]
        [InlineData("this is a test title 2023_09_10.mkv", true)]
        [InlineData("this is a test title 2023.09.10.mkv", true)]
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
                Episode = "10910",
                File = fn
            };

            Assert.Equal(expectedDTO.Date, dto.Date);
            Assert.Equal(expectedDTO.Title, dto.Title);
            Assert.Equal(expectedDTO.Year, dto.Year);
            Assert.Equal(expectedDTO.Season, dto.Season);
            Assert.Equal(expectedDTO.File, dto.File);
        }

        // Date at the end of file name.
        [Theory]
        [InlineData("230910 SeriesTitle ep01 - this is a test title.mkv", true)]
        [InlineData("20230910 SeriesTitle DVD1 - this is a test title.mkv", true)]
        [InlineData("20230910 SeriesTitle SP1 - this is a test title.mkv", true)]
        [InlineData("20230910 SeriesTitle #01 - this is a test title.mkv", true)]
        [InlineData("2023-09-10 SeriesTitle DVD1.1 - this is a test title.mkv", true)]
        [InlineData("2023-09-10 SeriesTitle SP1.1 - this is a test title.mkv", true)]
        public void ParseFilesCorrectlySpecialCase(string fn, bool expected)
        {
            var dto = Utils.Parse(fn);
            Assert.Equal(expected, dto.Parsed);
            if (false == expected)
            {
                return;
            }

            var rx = new Regex(@"(?<episode>\#(\d+)|ep(\d+)|DVD[0-9.-]+|SP[0-9.-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var epNumber = rx.Matches(fn)[0].Groups["episode"].ToString();

            var expectedDTO = new DTO
            {
                Parsed = true,
                Date = "2023-09-10",
                Title = epNumber + " - this is a test title",
                Year = "2023",
                Season = "2023",
                Episode = "10910",
                File = fn
            };

            Assert.Equal(expectedDTO.Date, dto.Date);
            Assert.Equal(expectedDTO.Title, dto.Title);
            Assert.Equal(expectedDTO.Year, dto.Year);
            Assert.Equal(expectedDTO.Season, dto.Season);
            Assert.Equal(expectedDTO.File, dto.File);
        }



    }
}
