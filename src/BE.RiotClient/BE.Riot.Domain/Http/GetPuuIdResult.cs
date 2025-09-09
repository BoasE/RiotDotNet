namespace BE.Riot.Http;

public readonly record struct GetPuuIdResult(bool Found, PuuId Puiid)
{
    public static GetPuuIdResult None => new (false, default);
};
