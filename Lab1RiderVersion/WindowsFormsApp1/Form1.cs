using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		private Color initiateColor;
		private Color endColor;
		private const int _width = 50;
		private const int _height = 50;
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.Cancel)
				return;

			initiateColor = colorDialog1.Color;
			
			//paintMonoColor();
			paintMonoColorv2();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.Cancel)
				return;

			endColor = colorDialog1.Color;
			
			
		}

		private void paintMonoColorv2()
		{
			var x = GetSystemMetrics(SystemMetric.SM_CXSCREEN);
			var y = GetSystemMetrics(SystemMetric.SM_CYSCREEN);
			
			IntPtr hwnd = GetDesktopWindow();
			var desktopScreen = GetDC(hwnd);
			var test = CreateCompatibleDC(desktopScreen);
			var bitmap = CreateCompatibleBitmap(desktopScreen, _width, _height);
			SelectObject(test, bitmap);
			
			BitBlt(test, 0, 0, _width, _height, 
				desktopScreen, 0, 0, 
				(uint) TernaryRasterOperations.SRCCOPY);
			
			var newColorInt = ColorToInt(initiateColor);

			for (var i = 0; i < _width; i++)
			{
				for (var j = 0; j < _height; j++)
				{
					var pixel = GetPixelColor(test, i, j);
					
					Trace.WriteLine($"{i}, {j} Color: {pixel}");
					
					if (pixel.R == Color.Aqua.R && pixel.G == Color.Aqua.G && pixel.B == Color.Aqua.B)
					{
						Trace.WriteLine($"{i}, {j}: A: {pixel.A}, R: {pixel.R}, G: {pixel.G}, B: {pixel.B}");
					}
					SetPixel(test, i, j, newColorInt);
				}
			}
			
			BitBlt(desktopScreen, 0, 0, _width, _height, 
				test, 0, 0, 
				(uint) TernaryRasterOperations.SRCCOPY);
			
			DeleteObject(bitmap);
			DeleteObject(test);
			
			ReleaseDC(hwnd, desktopScreen);
		}
		
		private void paintMonoColorAfter(int width = _width * 2, int height = _height * 2)
		{
			IntPtr hwnd = GetDesktopWindow();
			var desktopScreen = GetDC(hwnd);
			var test = CreateCompatibleDC(desktopScreen);
			var bitmap = CreateCompatibleBitmap(desktopScreen, width, height);
			SelectObject(test, bitmap);
			
			BitBlt(test, 0, 0, width, height, 
				desktopScreen, 0, 0, 
				(uint) TernaryRasterOperations.SRCCOPY);
			
			var newColorInt = ColorToInt(endColor);

			for (var i = 0; i < width; i++)
			{
				for (var j = 0; j < height; j++)
				{
					var pixel = GetPixelColor(test, i, j);
					// ya etot rot Alpha-channel'a shated
					if (pixel.R == initiateColor.R && pixel.G == initiateColor.G && pixel.B == initiateColor.B)
					{
						SetPixel(test, i, j, newColorInt);
					}
				}
			}
			
			BitBlt(desktopScreen, 0, 0, width, height, 
				test, 0, 0, 
				(uint) TernaryRasterOperations.SRCCOPY);
			
			DeleteObject(bitmap);
			DeleteObject(test);
			
			ReleaseDC(hwnd, desktopScreen);
		}

		private int ColorToInt(Color color)
		{
			//return (uint) (color.ToArgb() & 0x00FFFFFF);
			//var result = (uint)(((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B) & 0xffffffffL);
			
			//return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8)  | (color.B << 0));
			return color.B << 16 | color.G << 8 | color.R;
			//return result;
		}

		private void paintMonoColor()
		{
			var mainWindow = GetDC(IntPtr.Zero); // в pinvoke пишут, что зеро выдаёт весь экран.
			var compatibleDc = CreateCompatibleDC(mainWindow);
			
			//ya не могу понять, почему обязательно нужен битмап, мы не можем напрямую пиксель ебашить?
			var bitmap = CreateCompatibleBitmap(mainWindow, _width, _height);
			SelectObject(compatibleDc, bitmap);
			BitBlt(mainWindow, 0, 0, _width, _height, 
				compatibleDc, 0, 0, (uint) 
				TernaryRasterOperations.SRCCOPY);
			for (var i = 0; i < _width; i++)
			{
				for (var j = 0; j < _height; j++)
				{
					var test = initiateColor.ToArgb();
					SetPixel(compatibleDc, i, j, (uint) (initiateColor.ToArgb() & 0x00FFFFFF));
					
				}
			}
			
			BitBlt(mainWindow, 0, 0, _width, _height, 
				compatibleDc, 0, 0, (uint) 
				TernaryRasterOperations.SRCCOPY);
			DeleteObject(bitmap);
			ReleaseDC(mainWindow, compatibleDc);
			DeleteObject(compatibleDc);
			DeleteObject(mainWindow);
		}
		
		private void button3_Click(object sender, EventArgs e)
		{
			/*
			var mainWindow = GetDC(IntPtr.Zero); // в pinvoke пишут, что зеро выдаёт весь экран.
			var compatibleDc = CreateCompatibleDC(mainWindow);
			
			//ya не могу понять, почему обязательно нужен битмап, мы не можем напрямую пиксель ебашить?
			var bitmap = CreateCompatibleBitmap(mainWindow, _width, _height);
			SelectObject(compatibleDc, bitmap);
			BitBlt(compatibleDc, 0, 0, _width, _height, 
			bitmap, 0, 0, (uint) 
			TernaryRasterOperations.SRCCOPY);
			
			for (var i = 0; i < _width; i++)
			{
				for (var j = 0; j < _height; j++)
				{
					var pix = GetPixelColor(compatibleDc, i, j);

					if (pix.Equals(initiateColor))
					{
						SetPixel(compatibleDc, i, j, (uint) (endColor.ToArgb() & 0x00FFFFFF));
					}
					
				}
			}
			
			BitBlt(mainWindow, 0, 0, _width, _height, 
				bitmap, 0, 0, (uint) TernaryRasterOperations.SRCCOPY);
			DeleteObject(bitmap);
			ReleaseDC(mainWindow, compatibleDc);
			DeleteObject(compatibleDc);
			DeleteObject(mainWindow);
			*/
			
			// Попытка 2
			
			paintMonoColorAfter();
		}
		
		#region Импорты апи
		
		[DllImport("user32.dll", SetLastError = false)]
		static extern IntPtr GetDesktopWindow();
		
		/// <summary>
		/// Контекст устройства (DC) в памяти дает возможность системе рассматривать часть памяти как виртуальное устройство.
		/// Получает контекст устройства.
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hwnd);
		
		/// <summary>
		/// Создает контекст устройства в памяти  (DC), совместимый с заданным устройством.
		/// </summary>
		/// <param name="hdc">Дескриптор контекста устройства.</param>
		/// <returns>Если гуд, то дискриптор устройства, иначе PtrZero?</returns>
		[DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
		static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);
		
		/// <summary>
		/// Высвобождает контекст устройства.
		/// Нужно вызвать из-за GetDc. (чисти, чисти).
		/// </summary>
		/// <param name="hwnd">Дескриптор окна, контроллер домена которого должен быть освобожден.</param>
		/// <param name="hdc">Дескриптор, отпустимый контроллер домена.</param>
		/// <returns>1, если контроллер освобожден, иначе 0.</returns>
		[DllImport("user32.dll")]
		static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
		
		
		/// <summary>
		/// создает точечный рисунок, совместимый с устройством, которое связано с заданным контекстом устройства.
		/// (Боже, сохрани создателя сайта http://vsokovikov.narod.ru).
		/// </summary>
		/// <param name="hdc">Контекст устройства.</param>
		/// <param name="nWidth">Ширина рисунка в пикселях</param>
		/// <param name="nHeight">Высота рисунка в пикселях.</param>
		/// <returns></returns>
		[DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
		static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);
		
		/// <summary>
		/// Выбирает объект в заданный контекст устройства.
		/// Новый объект заменяет предыдущий объект того же самого типа.
		/// </summary>
		/// <param name="hdc">Контекст устройства.</param>
		/// <param name="hgdiobj">Новый объект.</param>
		/// <returns>Если успешно, то вернется дескриптор заменяемого объекта.</returns>
		[DllImport("gdi32.dll", EntryPoint = "SelectObject")]
		public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
		
		/// <summary>
		/// После 2 перезагрузок компа допер...
		/// </summary>
		/// <param name="hObject"></param>
		/// <returns></returns>
		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject([In] IntPtr hObject);
		
		/// <summary>
		/// Получает пиксель
		/// </summary>
		/// <param name="hdc">Контекст устройства.</param>
		/// <param name="nXPos">Координа абсцисс</param>
		/// <param name="nYPos">Координата ординат.</param>
		/// <returns>RGB-значение. По идее структура COLORREF.</returns>
		[DllImport("gdi32.dll")]
		static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
		
		//Обертка сразу.
		private Color GetPixelColor(IntPtr hdc, int nXPos, int nYPos)
		{
			var result = GetPixel(hdc, nXPos, nYPos);

			//return Color.FromArgb((int) result);
			
			return Color.FromArgb((int)(result & 0x000000FF),
				(int)(result & 0x0000FF00) >> 8,
				(int)(result & 0x00FF0000) >> 16);
		}
		
		/// <summary>
		/// Сетить пиксель.
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <param name="crColor"></param>
		/// <returns></returns>
		[DllImport("gdi32.dll")]
		static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);
		
		[DllImport("gdi32.dll")]
		static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
		
		
		[DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth,
			int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
		
		[DllImport("user32.dll")]
		static extern int GetSystemMetrics(SystemMetric smIndex);
		
		public enum TernaryRasterOperations : uint {
			SRCCOPY     = 0x00CC0020,
			SRCPAINT    = 0x00EE0086,
			SRCAND      = 0x008800C6,
			SRCINVERT   = 0x00660046,
			SRCERASE    = 0x00440328,
			NOTSRCCOPY  = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY   = 0x00C000CA,
			MERGEPAINT  = 0x00BB0226,
			PATCOPY     = 0x00F00021,
			PATPAINT    = 0x00FB0A09,
			PATINVERT   = 0x005A0049,
			DSTINVERT   = 0x00550009,
			BLACKNESS   = 0x00000042,
			WHITENESS   = 0x00FF0062,
			CAPTUREBLT  = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
		}

		#endregion


		
	}
}