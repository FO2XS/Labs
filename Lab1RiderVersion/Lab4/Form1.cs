using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsualLibrary;

namespace Lab4
{
	//todo: Возвращает N-е по счету число Фибоначи
	//todo: Запускает поток, в котором запоминаются 10 последних нажатых клавиш клавиатуры. Интерфейсная функция возвращает их по запросу.
	// про дин. подгрузку: https://metanit.com/sharp/tutorial/14.3.php
	
	public partial class Form1 : Form
	{
		private Assembly dynamicLibrary;
		public Form1()
		{
			InitializeComponent();
			dynamicLibrary = Assembly.LoadFrom("D:\\Университет\\7 семестр\\АрхитектураЭВМ\\GitRepository\\Labs\\Lab1RiderVersion\\DynamicLibrary\\bin\\Debug\\DynamicLibrary.dll");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var number = int.Parse(textBox1.Text);

			var calculator = new FibCalculator();

			var result = calculator.GetFibonachiSequence(number);

			textBox2.Text = result.ToString();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var libraryClass = dynamicLibrary.GetType("DynamicLibrary.Class1");

			var method = libraryClass.GetMethod("GetHelloNiggas", BindingFlags.Public | BindingFlags.Static);

			var result = method.Invoke(null, new object[] {"Гриша"});

			label2.Text = (string) result;
		}
	}
}