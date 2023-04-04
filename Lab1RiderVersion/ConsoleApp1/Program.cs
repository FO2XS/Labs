// See https://aka.ms/new-console-template for more information

using AzimutLibrary;


Console.WriteLine("Введите первую точку");
var firstPoint = GetPointFromConsole();

Console.WriteLine("Введите вторую точку");
var secondPoint = GetPointFromConsole();

var result = GeoManager.CalculateAzimut(firstPoint, secondPoint);

Console.WriteLine($"Азимут - {result.Degree}, дистанция - {result.Distance}");

GeoPoint GetPointFromConsole()
{
    var geoPoint = new GeoPoint();
    
    Console.WriteLine("Введите широту в градусах");
    geoPoint.LatitudeDegree = double.Parse(Console.ReadLine());
    Console.WriteLine("Введите долготу в градусах");
    geoPoint.LongitudeDegree = double.Parse(Console.ReadLine());

    return geoPoint;
}