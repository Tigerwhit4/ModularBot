/*
 * User: Nick
 * Date: 26/12/2013
 * Time: 8:23 PM
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ModularBotServer
{
	/// <summary>
	/// Description of ConsoleBoxExtension.
	/// </summary>
	public static class ConsoleBoxExtension
	{
		public static void AppendText(this RichTextBox box, string text, Color color)
	    {
	        box.SelectionStart = box.TextLength;
	        box.SelectionLength = 0;
	
	        box.SelectionColor = color;
	        box.AppendText(text);
	        box.SelectionColor = box.ForeColor;
	        
	        box.AppendText(Environment.NewLine);
	    }
	}
}
