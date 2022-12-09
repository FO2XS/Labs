using System.Runtime.InteropServices;
using System.Text;


[DllImport("user32.dll")]
static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);
[DllImport("user32")]
static extern bool GetMessage(ref Message lpMsg, IntPtr handle, uint mMsgFilterInMain, uint mMsgFilterMax);

const int MOD_ALT = 0x0001;
const int MOD_CONTROL = 0x0002;
const int MOD_SHIFT = 0x004;
const int MOD_NOREPEAT = 0x400;
const int WM_HOTKEY = 0x312;
const int DSIX = 0x36;
 
	if (!RegisterHotKey(IntPtr.Zero, 1, MOD_ALT | MOD_NOREPEAT, DSIX))
	{
		Console.WriteLine("failed key register!");
	}

	Message msg = new Message();

	while (!GetMessage(ref msg, IntPtr.Zero, 0, 0))
	{
		if (msg.message == WM_HOTKEY)
		{
			Console.WriteLine("do work..");
		}
	}

	Console.ReadLine();

public class Message
{
	public int message { get; set; }
}




