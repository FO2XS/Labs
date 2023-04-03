using AzimutLibrary;

namespace AzimutTests;

[TestClass]
public class GeoManagerTest
{
    #region Constants

    /// <summary>
    /// Радиус земли.
    /// </summary>
    private const int _earthRadius = 6371;
    
    /// <summary>
    /// Половина длины земного экватора.
    /// </summary>
    private const int _halfEarthLength = 20037;

    #endregion

    #region Tests

    /// <summary>
    /// Проверка на правильность расчета азимута.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="expectedAzimut"></param>
    [TestMethod]
    [DynamicData(nameof(GetCorrectPointsAzimut), DynamicDataSourceType.Method)]
    public void CalculateAzimut_CorrectPoints_CorrectAzimut(GeoPoint startPoint, GeoPoint endPoint, double expectedAzimut)
    {
        var result = GeoManager.CalculateAzimut(startPoint, endPoint);
        
        Assert.AreEqual(expectedAzimut, result.Degree);
    }
    
    /// <summary>
    /// Проверка на правильность расчета расстояния.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="expectedAzimut"></param>
    [TestMethod]
    [DynamicData(nameof(GetCorrectPointsDistance), DynamicDataSourceType.Method)]
    public void CalculateAzimut_CorrectPoints_CorrectDistance(GeoPoint startPoint, GeoPoint endPoint, double expectedAzimut)
    {
        var result = GeoManager.CalculateAzimut(startPoint, endPoint);
        
        Assert.AreEqual(expectedAzimut, result.Distance);
    }
    
    /// <summary>
    /// Проверка на частные случаи расчета азимута.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="expectedAzimut"></param>
    [TestMethod]
    [DynamicData(nameof(GetIncorrectPointsUnpossibleAzimut), DynamicDataSourceType.Method)]
    public void CalculateAzimut_IncorrectPoints_UnpossibleAzimut(GeoPoint startPoint, GeoPoint endPoint, UnpossibleAzimut expectedAzimut)
    {
        var result = GeoManager.CalculateAzimut(startPoint, endPoint);
        
        Assert.AreEqual(expectedAzimut, result.Degree);
    }
    
    /// <summary>
    /// Проверка на то, что расстояние между точками не может быть больше половины экватора.
    /// </summary>
    /// <param name="startPoint">Начальная точка.</param>
    /// <param name="endPoint">Конечная точка.</param>
    [TestMethod]
    [ExpectedException(typeof(ArithmeticException))]
    [DynamicData(nameof(GetPoints), DynamicDataSourceType.Method)]
    public void CalculateAzimut_Points_СheckHalfEarthLength(GeoPoint startPoint, GeoPoint endPoint)
    {
        Assert.IsTrue(GeoManager.CalculateAzimut(startPoint, endPoint).Distance <= _halfEarthLength);
    }

    #endregion

    #region Minor-methods

    private static IEnumerable<object? []> GetCorrectPointsAzimut()
    {
        // Точки на экваторе.
        yield return new object? []
        {
            new GeoPoint(0, 120),
            new GeoPoint(0, -100),
            90
        };
        
        // Точки на экваторе.
        yield return new object? []
        {
            new GeoPoint(0, 120),
            new GeoPoint(0, 10),
            270
        };
        
        // Идем на север
        yield return new object? []
        {
            new GeoPoint(1, 50),
            new GeoPoint(45, 50),
            0
        };
        
        // Идем на юг
        yield return new object? []
        {
            new GeoPoint(1, 50),
            new GeoPoint(-45, 50),
            180
        };
        
        // Случайные точки.
        yield return new object? []
        {
            new GeoPoint(37, 67),
            new GeoPoint(-42, -87),
            247.97
        };
    }
    
    private static IEnumerable<object? []> GetCorrectPointsDistance()
    {
        // Случайные точки.
        yield return new object? []
        {
            new GeoPoint(37, 67),
            new GeoPoint(-42, -87),
            12532.95
        };
        
        // Случайные точки.
        yield return new object? []
        {
            new GeoPoint(87, 67),
            new GeoPoint(-42, -87),
            11735.77
        };
    }
    
    private static IEnumerable<object? []> GetPoints()
    {
        // Проверка на то, что расстояние между точками не может быть больше половины экватора.
        yield return new object? []
        {
            new GeoPoint(0, 179),
            new GeoPoint(0, -179)
        };
    }
    
    private static IEnumerable<object? []> GetIncorrectPointsUnpossibleAzimut()
    {
        // Совпадающие точки.
        yield return new object? []
        {
            new GeoPoint(5, 5),
            new GeoPoint(5, 5),
            UnpossibleAzimut.None
        };
        
        // Точки, лежащие на прямой через центр земли.
        yield return new object? []
        {
            new GeoPoint(0, 0),
            new GeoPoint(0, 180),
            UnpossibleAzimut.Any
        };
        
        // Точки, лежащие на одном полюсе.
        yield return new object? []
        {
            new GeoPoint(90, 0),
            new GeoPoint(90, 50),
            UnpossibleAzimut.None
        };
        
        // Точки, лежащие на противоположных полюсах.
        yield return new object? []
        {
            new GeoPoint(90, 0),
            new GeoPoint(-90, 0),
            UnpossibleAzimut.Any
        };
    }

    #endregion
}