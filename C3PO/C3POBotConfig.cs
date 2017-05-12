using Newtonsoft.Json;

namespace C_3PO_bot
{
    internal sealed class C3POBotConfig
    {
        [JsonProperty("token")]
        public string Token { get; private set; } = string.Empty;

        [JsonProperty("command_prefix")]
        public string CommandPrefix { get; private set; } = "d#+";

        [JsonProperty("shards")]
        public int ShardCount { get; private set; } = 1;
    }
}
