using System;
using System.Collections.Generic;

namespace Jellyfin.Plugin.DAILYExtender.Helpers
{
    public class Constants
    {
        public const string PLUGIN_NAME = "DAILYExtender";
        public const string PLUGIN_GUID = "be32c1b1-426a-467a-ae34-9182e1736f54";
        public static readonly string[] Patterns = {
            // YY?YY(-._)MM(-._)DD -? series -? epNumber -? title
            @"^(?<year>\d{2,4})(\-|\.|_)?(?<month>\d{2})(\-|\.|_)?(?<day>\d{2})\s-?(?<series>.+?)(?<epNumber>\#(\d+)|ep(\d+)|DVD[0-9.-]+) -?(?<title>.+)",
            // YYYY(-_.)?MM(-_.)?DD title
            @"^(?<year>\d{2,4})(\-|\.|_)?(?<month>\d{2})(\-|\.|_)?(?<day>\d{2})\s?-?(?<title>.+)",
            // title YY?YY(-_.)?MM(-_.)?DD
            @"(?<title>.+?)(?<year>\d{2,4})(\-|\.|_)?(?<month>\d{2})(\-|\.|_)?(?<day>\d{2})$",
            // series YYYY(-_.)?MM(-_.)?DD -? title
            @"(?<series>.+?)(?<year>\d{2,4})(\-|\.|_)?(?<month>\d{2})(\-|\.|_)?(?<day>\d{2})\s?-?(?<title>.+)?",
        };
    }
}
