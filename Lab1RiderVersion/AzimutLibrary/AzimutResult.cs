namespace AzimutLibrary;

/// <summary>
/// Модель результата расчета азимута.
/// </summary>
public class AzimutResult
{
    /// <summary>
    /// Азимут в градусах.
    /// </summary>
    public double Degree { get; set; }
    
    /// <summary>
    /// Расстояние в километрах.
    /// </summary>
    public double Distance { get; set; }
}