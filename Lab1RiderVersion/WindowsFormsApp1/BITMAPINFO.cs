using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
	[StructLayout( LayoutKind.Sequential )]
	struct BITMAPINFO {
		/// <summary>
		/// A BITMAPINFOHEADER structure that contains information about the dimensions of color format.
		/// </summary>
		public BITMAPINFOHEADER bmiHeader;

		/// <summary>
		/// An array of RGBQUAD. The elements of the array that make up the color table.
		/// </summary>
		[MarshalAsAttribute( UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct )]
		public RGBQUAD[] bmiColors;
	}
	
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct RGBQUAD
	{
		public byte rgbBlue;
		public byte rgbGreen;
		public byte rgbRed;
		public byte rgbReserved;
	}
}