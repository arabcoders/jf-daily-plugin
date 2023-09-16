using System;
using System.Collections.Generic;

namespace Jellyfin.Plugin.DAILYExtender.Helpers
{
    public class Constants
    {
        public const string PLUGIN_NAME = "DAILYExtender";
        public const string PLUGIN_GUID = "be32c1b1-426a-467a-ae34-9182e1736f54";
        public static readonly string[] Patterns = {
            @"(?<year>\d{2,4})(?<month>\d{2})(?<day>\d{2})\s?-?(?<title>.+)",
            @"(?<year>\d{4})\-(?<month>\d{2})\-(?<day>\d{2})\s?-?(?<title>.+)",
        };
    }
}
