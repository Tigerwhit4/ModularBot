/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 10:35 AM
 * 
 */
using System;
using System.Windows.Forms;

namespace ModularBotServer
{
	internal sealed class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}