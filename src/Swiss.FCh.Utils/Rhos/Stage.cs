namespace Swiss.FCh.Utils.Rhos;

public sealed class Stage
{
    public const string Dev = "dev";
    public const string Ref = "ref";
    public const string Abn = "abn";
    public const string Prod = "prod";

    public static bool IsRhosStage(string? stage)
    {
        return stage?.ToLowerInvariant() is Dev or Ref or Abn or Prod;
    }
}
