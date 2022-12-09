using System.Diagnostics;
using System.Runtime.InteropServices;
using static PInvoke.Kernel32;

namespace Lab1RiderVersion.Utilities;

/// <summary>
/// Класс работы с хранилищами данных.
/// </summary>
public static class DriveHelper
{
	private static readonly string[] ignoreDirectoryNames = {".", ".."};
	/// <summary>
	/// Ищет файл в логическом диске.
	/// </summary>
	/// <param name="directoryPath">Метка тома в виде "МЕТКА_Тома:".</param>
	/// <param name="fileName">Название файла с расширением.</param>
	/// <returns>Путь к файлу.</returns>
	public static string FindFileOnLogicalDrive(string directoryPath, string fileName)
	{
		var currentDirectory = $"{directoryPath}\\";

		var pathFile = FindFileAtDirectory(directoryPath, fileName);

		if (pathFile is not null)
		{
			return pathFile;
		}

		var childDirectories = GetDirectoriesAtDirectory(currentDirectory);

		foreach (var childDirectory in childDirectories)
		{
			pathFile = FindFileOnLogicalDrive(currentDirectory + childDirectory, fileName);

			if (pathFile is not null)
			{
				break;
			}
		}

		return pathFile;
	}
	
	/// <summary>
	/// Возвращает список имен логических дисков компьютера.
	/// </summary>
	/// <param name="bufferSize">Размер буфера, в который записываются данные.</param>
	/// <returns>Список имен логических дисков.</returns>
	public static IEnumerable<string> GetLogicalDriveNames(uint bufferSize = 32)
	{
		var result = new List<string>();
		var buffer = new char[bufferSize];
		var test = GetLogicalDriveStrings(bufferSize, buffer);
		
		var start = 0;
		for (var i = 0; i < test; i++)
		{
			if (buffer[i] != 0) continue;
			
			result.Add(new string(buffer, start, i - start));
			start = i + 1;
		}

		return result;
	}

	#region private-методы

	/// <summary>
	/// Поиск файла в папке.
	/// </summary>
	/// <param name="directoryPath">Путь до директории в виде: "МеткаТома:\...\Имя_директории".</param>
	/// <param name="fileName">Название файла с расширением.</param>
	/// <returns>Путь до файла.</returns>
	private static string? FindFileAtDirectory(string directoryPath, string fileName)
	{
		var FIND_FIRST_EX_LARGE_FETCH = 2;
		
		WIN32_FIND_DATA findData;
		var findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoStandard;
		
		var additionalFlags = 0;
		
		if (Environment.OSVersion.Version.Major >= 6)
		{
			findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoStandard;
			additionalFlags = FIND_FIRST_EX_LARGE_FETCH;
		}

		var hFile = FindFirstFileEx(
			$"{directoryPath}\\{fileName}",
			findInfoLevel,
			out findData,
			FINDEX_SEARCH_OPS.FindExSearchNameMatch,
			IntPtr.Zero,
			additionalFlags);
		
		var error = Marshal.GetLastWin32Error();

		if (hFile.ToInt64() != -1)
		{
			do
			{
				if ((findData.dwFileAttributes & FileAttribute.FILE_ATTRIBUTE_DIRECTORY) != FileAttribute.FILE_ATTRIBUTE_DIRECTORY)
				{
					Trace.WriteLine("Found file {0}", findData.cFileName);

					if (findData.cFileName == fileName)
					{
						return $"{directoryPath}\\{fileName}";
					}
				}
			}
			while (FindNextFile(hFile, out findData));
		}

		return null;
	}

	/// <summary>
	/// Возвращает список папок в указанной директории.
	/// </summary>
	/// <param name="directoryPath">Путь до директории в виде: "МеткаТома:\...\Имя_директории".</param>
	/// <returns>Список папок в директории.</returns>
	private static IEnumerable<string> GetDirectoriesAtDirectory(string directoryPath)
	{
		var result = new List<string>();
		var FIND_FIRST_EX_LARGE_FETCH = 2;
		WIN32_FIND_DATA findData;
		var findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoStandard;
		var additionalFlags = 0;
		
		if (Environment.OSVersion.Version.Major >= 6)
		{
			findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoStandard;
			additionalFlags = FIND_FIRST_EX_LARGE_FETCH;
		}

		var hFile = FindFirstFileEx(
			$"{directoryPath}\\*",
			findInfoLevel,
			out findData,
			FINDEX_SEARCH_OPS.FindExSearchNameMatch,
			IntPtr.Zero,
			additionalFlags);
		
		var error = Marshal.GetLastWin32Error();

		if (hFile.ToInt64() != -1)
		{
			do
			{
				if ((findData.dwFileAttributes & FileAttribute.FILE_ATTRIBUTE_HIDDEN) == FileAttribute.FILE_ATTRIBUTE_HIDDEN)
					continue;
				if (ignoreDirectoryNames.Contains(findData.cFileName))
					continue;
				
				if ((findData.dwFileAttributes & FileAttribute.FILE_ATTRIBUTE_DIRECTORY) == FileAttribute.FILE_ATTRIBUTE_DIRECTORY)
					result.Add(findData.cFileName);
			}
			while (FindNextFile(hFile, out findData));
		}

		return result;
	}

	#endregion

	#region Импорты api
	/// <summary>
	/// 
	/// </summary>
	/// <param name="bufferLength"></param>
	/// <param name="buffer"></param>
	/// <returns></returns>
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern uint GetLogicalDriveStrings(uint bufferLength, [Out] char[] buffer);
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="lpFileName"></param>
	/// <param name="fInfoLevelId"></param>
	/// <param name="lpFindFileData"></param>
	/// <param name="fSearchOp"></param>
	/// <param name="lpSearchFilter"></param>
	/// <param name="dwAdditionalFlags"></param>
	/// <returns></returns>
	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern IntPtr FindFirstFileEx(
		string lpFileName,
		FINDEX_INFO_LEVELS fInfoLevelId,
		out WIN32_FIND_DATA lpFindFileData,
		FINDEX_SEARCH_OPS fSearchOp,
		IntPtr lpSearchFilter,
		int dwAdditionalFlags);
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hFindFile"></param>
	/// <param name="lpFindFileData"></param>
	/// <returns></returns>
	[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
	static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hFindFile"></param>
	/// <returns></returns>
	[DllImport("kernel32.dll")]
	static extern bool FindClose(IntPtr hFindFile);
	
	#endregion
}