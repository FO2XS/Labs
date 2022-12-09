using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuckMyMind
{
	enum SwapMode
	{
		RuToEng,
		EngToRu
	}
	public partial class Form1 : Form
	{
		[DllImport("user32")]
		static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);
		
		private static List<char> RussianKey = new List<char>() { 'Ф', 'Ы', 'В', 'А', 'Й', 'Ц', 'У', 'К', 'Е', 'Н', 'Г', 'Ш', 'Щ', 'З', 'Х', 'Ъ', 'П', 'Р', 'О', 'Л', 'Д', 'Ж', 'Э', 'Ё', 'Я', 'Ч', 'С', 'М', 'И', 'Т', 'Ь', 'Б', 'Ю' };
		private static List<char> EnglishKey = new List<char>() { 'A', 'S', 'D', 'F', 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', '[', ']', 'G', 'H', 'J', 'K', 'L', ';', '\'', '`', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', ',', '.' };

		public void SwapLastWord()
		{
			var locale = new StringBuilder(9);
			GetKeyboardLayoutName(locale);

			var swapMode = locale.ToString() == "A0000409" ? SwapMode.EngToRu : SwapMode.RuToEng;
			
			var swapWord = textBox.Text.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
			
			if (string.IsNullOrEmpty(swapWord))
				return;

			var newWord = swap(swapWord, swapMode);

			textBox.Text = textBox.Text.Replace(swapWord, newWord);
		}

		private string swap(string word, SwapMode mode)
		{
			var result = "";
			var oldChars = EnglishKey;
			var newChars = RussianKey;
			
			switch (mode)
			{
				case SwapMode.EngToRu:
					oldChars = EnglishKey;
					newChars = RussianKey;
					break;
				case SwapMode.RuToEng:
					oldChars = RussianKey;
					newChars = EnglishKey;
					break;
			}

			foreach (var symbol in word)
			{
				var index = oldChars.FindIndex(oldChar => oldChar == char.ToUpper(symbol));
				if (index < 0)
				{
					result += symbol;
					continue;
				}
				var newChar = newChars[index];

				if (char.IsUpper(symbol))
				{
					result += newChar;
				}
				else
				{
					result += char.ToLower(newChar);
				}
			}

			return result;
		}
		
		
		public Form1()
		{
			InitializeComponent();
		}
	}
}