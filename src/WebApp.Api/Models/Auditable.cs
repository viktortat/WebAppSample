namespace WebApp.Api.Models;

public interface IAuditable
{
    /// <summary>
    /// DateTime of creation. This value will never changed
    /// </summary>
    DateTime create_date { get; set; }

    /// <summary>
    /// Author name. This value never changed
    /// </summary>
    string creator_id { get; set; }

    /// <summary>
    /// DateTime of last value update. Should be updated when entity data updated
    /// </summary>
    DateTime? update_date { get; set; }

    /// <summary>
    /// Author of last value update. Should be updated when entity data updated
    /// </summary>
    string updater_id { get; set; }

}
public abstract class Auditable : IAuditable
{
    /// <summary>
    /// DateTime when entity created.
    /// It's never changed
    /// </summary>
    public DateTime create_date { get; set; }

    /// <summary>
    /// User name who created entity.
    /// It's never changed
    /// </summary>
    public string creator_id { get; set; } = null!;

    /// <summary>
    /// Last date entity updated
    /// </summary>
    public DateTime? update_date { get; set; }

    /// <summary>
    /// Author of last updated
    /// </summary>
    public string? updater_id { get; set; }

}
