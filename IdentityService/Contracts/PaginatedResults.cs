using Newtonsoft.Json;

namespace IdentityService.Contracts
{
    public class PaginatedResults
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PreviousPage { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NextPage { get; set; }
        public object[] Results { get; set; }
    }
}
