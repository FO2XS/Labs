namespace AzimutLibrary;

/// <summary>
/// Географическая точка.
/// </summary>
public class GeoPoint
{
    private double _latitudeDegree;
    private double _longitudeDegree;

    /// <summary>
    /// Широта.
    /// </summary>
    public double LatitudeDegree
    {
        get => _latitudeDegree;
        set
        {
            if (value is <= 90 and >= -90)
                _latitudeDegree = value;
            else
            {
                throw new ArgumentException();
            }

        }
    }

    /// <summary>
    /// Широта в радианах.
    /// </summary>
    public double LatitudeRadian => LatitudeDegree * Math.PI / 180;

    /// <summary>
    /// Долгота.
    /// </summary>
    public double LongitudeDegree
    {
        get => _longitudeDegree;
        set
        {
            if (value is <= 180 and >= -180)
                _longitudeDegree = value;
            else
            {
                throw new ArgumentException();
            }

        }
    }

    /// <summary>
    /// Долгота в радианах.
    /// </summary>
    public double LongitudeRadian => LongitudeDegree * Math.PI / 180;
    
    /// <summary>
    /// Конструктор точки.
    /// </summary>
    /// <param name="latitudeDegree">Широта в градусах</param>
    /// <param name="longitudeDegree">Долгота в градусах</param>
    public GeoPoint(double latitudeDegree, double longitudeDegree)
    {
        LatitudeDegree = latitudeDegree;
        LongitudeDegree = longitudeDegree;
    }
    
    public GeoPoint()
    {
    }

    /// <summary>
    /// Проверка на равенство двух точек.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is GeoPoint point)
        {
            return LatitudeDegree == point.LatitudeDegree && LongitudeDegree == point.LongitudeDegree;
        }

        return base.Equals(obj);
    }
}