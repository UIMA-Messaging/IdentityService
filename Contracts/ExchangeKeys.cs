using System.ComponentModel.DataAnnotations;

namespace ContactApi.Contracts
{
    public class ExchangeKeys
    {
        public string UserId { get; }
        [RegularExpression(@"\b[0-9a-fA-F]+\b", ErrorMessage = "Invalid hex string.")]
        public string IdentityKey { get; }
        [RegularExpression(@"\b[0-9a-fA-F]+\b", ErrorMessage = "Invalid hex string.")]
        public string SignedPreKey { get; }
        [MinLength(200, ErrorMessage = "A minimum of 200 key bundles is required.")]
        public string[] OneTimePreKeys { get; }
        public string Signature { get; set; }
    }
}
