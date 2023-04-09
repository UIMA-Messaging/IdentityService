namespace ContactApi.Exceptions;

public class BadKeyBundleRequest : HttpException
{
    public BadKeyBundleRequest() : base(400, "Target user is the same as requesting user. Cannot fetch key bundle")
    {
    }
}