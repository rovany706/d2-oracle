namespace D2Oracle.Core.Services.DotaKnowledge;

public record ItemDescription(
    int Id,
    string Name,
    int Cost,
    int SecretShop,
    int SideShop,
    int Recipe,
    string LocalizedName
);
