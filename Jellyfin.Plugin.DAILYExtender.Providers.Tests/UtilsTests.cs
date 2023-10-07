﻿using Xunit;
using System;
using System.Text.Json;
using Jellyfin.Plugin.DAILYExtender.Helpers;

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
        [InlineData("Series Title - 230910 - this is a test title [720p [720p].mkv", true)]
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
    }
}
