namespace Jellyfin.Plugin.DAILYExtender.Helpers
{
    public class Constants
    {
        public const string PLUGIN_NAME = "DAILYExtender";
        public const string PLUGIN_GUID = "be32c1b1-426a-467a-ae34-9182e1736f54";
        public const string CHANNEL_URL = "https://www.youtube.com/channel/{0}";
        public const string CHANNEL_RX = @"(?<=\[)(?:youtube-)?(?<id>(UC|HC)[a-zA-Z0-9\-_]{22})(?=\])";
        public const string VIDEO_URL = "https://www.youtube.com/watch?v={0}";
        public const string VIDEO_RX = @"(?<=\[)(?:youtube-)?(?<id>[a-zA-Z0-9\-_]{11})(?=\])";
    }
}
