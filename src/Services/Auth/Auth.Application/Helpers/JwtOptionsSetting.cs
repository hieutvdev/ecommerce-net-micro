namespace Auth.Application.Helpers;

public class JwtOptionsSetting
{
    public string Secret { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
}