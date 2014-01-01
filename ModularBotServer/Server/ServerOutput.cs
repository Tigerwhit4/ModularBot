/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 11:16 AM
 * 
 */
using System;
using ModularBot;

namespace ModularBotServer
{
	/// <summary>
	/// Description of ServerOutput.
	/// </summary>
	public class ServerOutput : IServerOutput
	{
		public void ThrowError(string errorMessage)
		{
			ThrowMessage("Error", errorMessage);
		}
		
		public void ThrowInfo(string infoMessage)
		{
			ThrowMessage("Info", infoMessage);
		}
		
		public void ThrowMenu(int inputOption, string menuMessage)
		{
			ThrowMessage(inputOption.ToString(), menuMessage);
		}
		
		public void ThrowMessage(string messageType, string message)
		{
			//todo: Implement log4net
			
			string messageFormat = "[{0}] {1}";
			Console.WriteLine(string.Format(messageFormat, messageType, message));
		}
		
		public void ClearMessages()
		{
			Console.Clear();
		}
		
		public void Pause()
		{
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
		
		public void ThrowPluginError(string pluginName, string errorMessage)
		{
			string pluginError = "[{0}] {1}";
			ThrowError(string.Format(pluginError, pluginName, errorMessage));
		}
		
		public void ThrowPluginInfo(string pluginName, string infoMessage)
		{
			string pluginInfo = "[{0}] {1}";
			ThrowInfo(string.Format(pluginInfo, pluginName, infoMessage));
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
