namespace Ecoeden.Catalogue.Domain.Configurations;

public sealed class IdentityGroupAccessOption
{
    public const string OptionName = "IdentityGroupAccess";
    public string Authority { get; set; }
    public string Audience { get; set; }
}
