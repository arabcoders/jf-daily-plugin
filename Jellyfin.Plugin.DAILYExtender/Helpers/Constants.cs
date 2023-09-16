using System;
using System.Collections.Generic;

namespace Jellyfin.Plugin.DAILYExtender.Helpers
{
    public class Constants
    {
        public const string PLUGIN_NAME = "DAILYExtender";
        public const string PLUGIN_GUID = "be32c1b1-426a-467a-ae34-9182e1736f54";
        public static readonly string[] Patterns = {
            // (YY?YYMMDD
            @"(?<year>\d{2,4})(?<month>\d{2})(?<day>\d{2})\s?-?(?<title>.+)",
            @"(?<series>.+?)(?<year>\d{2,4})(?<month>\d{2})(?<day>\d{2})\s?-?(?<title>.+)",
            @"(?<title>.+?)(?<year>\d{2,4})(?<month>\d{2})(?<day>\d{2})$",
            // YYYY(-_.)MM(-_.)DD
            @"(?<year>\d{4})(\-|\.|_)(?<month>\d{2})(\-|\.|_)(?<day>\d{2})\s?-?(?<title>.+)",
            @"(?<series>.+?)(?<year>\d{4})(\-|\.|_)(?<month>\d{2})(\-|\.|_)(?<day>\d{2})\s?-?(?<title>.+)",
            @"(?<title>.+?)(?<year>\d{4})(\-|\.|_)(?<month>\d{2})(\-|\.|_)(?<day>\d{2})$",
        };
    }
}
