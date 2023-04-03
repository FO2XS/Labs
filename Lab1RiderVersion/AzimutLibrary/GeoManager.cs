namespace AzimutLibrary;
public static class GeoManager
{
    /// <summary>
    /// Радиус земли.
    /// </summary>
    public const int EarthRadius = 6372;
    
    /// <summary>
    /// Расчет азимута и расстояния между двумя точками.
    /// </summary>
    /// <param name="startPoint">Начальная точка</param>
    /// <param name="endPoint">Конечная точка</param>
    /// <returns>Азимут и расстояние между точками.</returns> 
    public static AzimutResult CalculateAzimut(GeoPoint startPoint, GeoPoint endPoint)
    {
        
        var firstResult = new AzimutResult
        {
            Distance = Math.Round(GetDistance(startPoint, endPoint), 2),
            Degree = Math.Round(GetAzimut(startPoint, endPoint), 2)
        };

        return ProcessResult(firstResult, startPoint, endPoint);
    }

    /// <summary>
    /// Постобработка результата для частных случаев.
    /// </summary>
    /// <param name="firstResult">Первичные результат расчета</param>
    /// <param name="startPoint">Начальная точка</param>
    /// <param name="endPoint">Конечная точка</param>
    /// <returns>Обработанный результат</returns>
    private static AzimutResult ProcessResult(AzimutResult firstResult, GeoPoint startPoint, GeoPoint endPoint)
    {
        if (startPoint.Equals(endPoint) 
            || (startPoint.LatitudeDegree == endPoint.LatitudeDegree && startPoint.LatitudeDegree is 90 or -90))
            firstResult.Degree = -1;
        else if (Math.Abs(startPoint.LatitudeDegree) + Math.Abs(endPoint.LatitudeDegree) == 180
                 || Math.Abs(startPoint.LongitudeDegree) + Math.Abs(endPoint.LongitudeDegree) == 180)
            firstResult.Degree = -2;
        

        return firstResult;
    }

    /// <summary>
    /// Вычисление азимута между двумя точками.
    /// </summary>
    /// <param name="startPoint">Начальная точка</param>
    /// <param name="endPoint">Конечная точка</param>
    /// <returns></returns>
    private static double GetAzimut(GeoPoint startPoint, GeoPoint endPoint)
    {
        var cosStart = Math.Cos(startPoint.LatitudeRadian);
        var cosEnd = Math.Cos(endPoint.LatitudeRadian);
        var sinStart = Math.Sin(startPoint.LatitudeRadian);
        var sinEnd = Math.Sin(endPoint.LatitudeRadian);
        
        var longitudeDiff = endPoint.LongitudeRadian - startPoint.LongitudeRadian;
        var diffCos = Math.Cos(longitudeDiff);
        var diffSin = Math.Sin(longitudeDiff);

        var x = cosStart * sinEnd - sinStart * cosEnd * diffCos;
        var y = diffSin * cosEnd;
        var z = Math.Atan(-y / x) * 180 / Math.PI;

        if (x < 0)
            z += 180;
        
        var z2 = - (z + 180 % 360 - 180) * Math.PI / 180;
        
        var azimut1 = z2 - 2 * Math.PI * Math.Floor(z2/(2*Math.PI));
        
        return azimut1 * 180 / Math.PI;
    }

    /// <summary>
    /// Вычисление расстояния между двумя точками по формуле Хаверсина.
    /// </summary>
    /// <param name="startPoint">Начальная точка</param>
    /// <param name="endPoint">Конечная точка</param>
    /// <returns></returns>
    private static double GetDistance(GeoPoint startPoint, GeoPoint endPoint)
    {
        var cosStart = Math.Cos(startPoint.LatitudeRadian);
        var cosEnd = Math.Cos(endPoint.LatitudeRadian);
        var sinStart = Math.Sin(startPoint.LatitudeRadian);
        var sinEnd = Math.Sin(endPoint.LatitudeRadian);
        
        var longitudeDiff = endPoint.LongitudeRadian - startPoint.LongitudeRadian;
        var diffCos = Math.Cos(longitudeDiff);
        var diffSin = Math.Sin(longitudeDiff);

        var y = Math.Sqrt(Math.Pow(cosEnd * diffSin, 2) + Math.Pow(cosStart * sinEnd - sinStart * cosEnd * diffCos, 2));
        var x = sinStart * sinEnd + cosStart * cosEnd * diffCos;
        var ad = Math.Atan2(y, x);
        return ad * EarthRadius;
    }
}