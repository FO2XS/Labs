namespace AzimutLibrary;

/// <summary>
/// Модель результата расчета азимута.
/// </summary>
public class AzimutResult
{
    private double _degree;

    /// <summary>
    /// Азимут в градусах.
    /// </summary>
    public double Degree
    {
        get => _degree;
        set
        {
            if (value > 360)
            {
                throw new ArgumentException();
            }

            _degree = value;
        }
    }

    /// <summary>
    /// Расстояние в километрах.
    /// </summary>
    public double Distance { get; set; }
}