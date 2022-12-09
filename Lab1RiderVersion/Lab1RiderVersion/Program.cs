// See https://aka.ms/new-console-template for more information

//Вывести список всех доступных логических дисков. Реализовать поиск файла с
//заданным именем на локальном диске по выбору пользователя

using CommonLibrary;
using Lab1RiderVersion.Utilities;

var logicalDrives = DriveHelper.GetLogicalDriveNames();

Console.WriteLine("В системе имеются логические диски:");
foreach (var logicalDrive in logicalDrives)
{
	Console.WriteLine(logicalDrive);
}

var tom = ConsoleUtility.GetValue("Введите имя тома в формате: \"МеткаТома:\""); //"D:";
var fileName = ConsoleUtility.GetValue("Введите имя файла в формате: \"ИмяФайла.расширение\""); //"Алукард.jpg";

var test = DriveHelper.FindFileOnLogicalDrive(tom, fileName);

Console.WriteLine($"Файл найден по пути {test}");