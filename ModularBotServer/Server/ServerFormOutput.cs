/*
 * User: Nick
 * Date: 26/12/2013
 * Time: 7:27 PM
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using ModularBotServer;

using ModularBot;

namespace ModularBotServer.Server
{
	/// <summary>
	/// Description of ServerFormOutput.
	/// </summary>
	public class ServerFormOutput : IServerOutput
	{
		RichTextBox _consoleBox;
		
		private readonly Color _defaultColor = Color.Black, _errorColor = Color.Red, _infoColor = Color.Black;
		
		public ServerFormOutput(RichTextBox consoleBox)
		{
			_consoleBox = consoleBox;
		}
		
		public void ThrowPluginError(string pluginName, string errorMessage)
		{
			string errorInfo = "[{0}] {1}";
			ThrowError(string.Format(errorInfo, pluginName, errorMessage));
		}
		
		public void ThrowPluginInfo(string pluginName, string infoMessage)
		{
			string pluginInfo = "[{0}] {1}";
			ThrowInfo(string.Format(pluginInfo, pluginName, infoMessage));
		}
		
		public void ThrowError(string errorMessage)
		{
			ThrowMessage("Error", _errorColor, errorMessage);
		}
		
		public void ThrowInfo(string infoMessage)
		{
			ThrowMessage("Info", _infoColor, infoMessage);
		}
		
		public void ThrowMenu(int menuOption, string menuMessage)
		{
			
		}
		
		public void ThrowMessage(string messageType, Color messageColor, string message)
		{
			string messageFormat = "[{0}] {1}";
			_consoleBox.AppendText(string.Format(messageFormat, messageType, message), messageColor);
		}
		
		public void ThrowMessage(string messageType, string message)
		{
			ThrowMessage(messageType, _defaultColor, message);
		}
		
		public void ClearMessages()
		{
			//Not necessary in this form
		}
		
		public void Pause()
		{
			//Not necessary in this form
		}
		
		public void ThrowPluginInfo(IModularPlugin plugin, string infoMessage)
		{
			ThrowPluginInfo(plugin.GetPluginName(), infoMessage);
		}
		
		public void ThrowPluginError(IModularPlugin plugin, string errorMessage)
		{
			ThrowPluginError(plugin.GetPluginName(), errorMessage);
		}
	}
}
