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
    [DynamicData(nameof(GetIncorrectPoints), DynamicDataSourceType.Method)]
    public void CalculateAzimut_IncorrectPoints_ThrowDistanceException(GeoPoint startPoint, GeoPoint endPoint)
    {
        GeoManager.CalculateAzimut(startPoint, endPoint);
    }

    #endregion

    #region Minor-methods

    private static IEnumerable<object? []> GetCorrectPointsAzimut()
    {
        yield return new object? []
        {
            new GeoPoint {},
            new GeoPoint {},
            0
        };
    }
    
    private static IEnumerable<object? []> GetCorrectPointsDistance()
    {
        yield return new object? []
        {
            new GeoPoint {},
            new GeoPoint {},
            0
        };
    }
    
    private static IEnumerable<object? []> GetIncorrectPoints()
    {
        yield return new object? []
        {
            new GeoPoint {},
            new GeoPoint {}
        };
    }
    
    private static IEnumerable<object? []> GetIncorrectPointsUnpossibleAzimut()
    {
        yield return new object? []
        {
            new GeoPoint {},
            new GeoPoint {},
            UnpossibleAzimut.None
        };
    }

    #endregion
}