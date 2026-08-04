namespace Swiss.FCh.Utils.Rhos;

/// <summary>
/// This class holds the default stages on the Redhat Openshift environment of the FOITT.
/// </summary>
public sealed class Stage
{
    /// <summary>
    /// Default name of the "development" environment.
    /// </summary>
    public const string Dev = "dev";

    /// <summary>
    /// The default name of the "reference" environment.
    /// </summary>
    public const string Ref = "ref";


    /// <summary>
    /// The default name of the "staging" (pre-production) environment.
    /// </summary>
    public const string Abn = "abn";

    /// <summary>
    /// The default name of the "production" environment.
    /// </summary>
    public const string Prod = "prod";

    /// <summary>
    /// Reads the provided stage environment variable and determines whether it is one of the RHOS stages or not.
    /// </summary>
    /// <param name="stage">The name of the stage environment variable</param>
    /// <returns>
    /// <see langword="true"/> if the stage is on RHOS
    /// <see langword="false"/> otherwise
    /// </returns>
    public static bool IsRhosStage(string? stage)
    {
        return stage?.ToLowerInvariant() is Dev or Ref or Abn or Prod;
    }
}
