namespace CommonLibrary;

public static class ConsoleUtility
{
	public static string GetValue(string showedText = "Введите значение")
	{
		var result = "";
		
		while (string.IsNullOrEmpty(result))
		{
			Console.WriteLine(showedText);

			result = Console.ReadLine();
		}

		return result;
	}
}