namespace AzimutLibrary;

/// <summary>
/// Географическая точка.
/// </summary>
public class GeoPoint
{
    /// <summary>
    /// Широта.
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Долгота.
    /// </summary>
    public double Longitude { get; set; }
    
    public GeoPoint(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public GeoPoint()
    {
    }
}