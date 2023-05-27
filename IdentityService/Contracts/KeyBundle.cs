namespace IdentityService.Contracts;

public class KeyBundle
{
    public string IdentityKey { get; set; }
    public string OneTimePreKey { get; set; }
    public string SignedPreKey { get; set; }
    public string Signature { get; set; }
}