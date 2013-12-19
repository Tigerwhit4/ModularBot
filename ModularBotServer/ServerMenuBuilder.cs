/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 11:26 AM
 * 
 */
using System;
using System.Collections.Generic;
using ModularBot;

namespace ModularBotServer
{
	/// <summary>
	/// Description of ServerMenuBuilder.
	/// </summary>
	public class ServerMenuBuilder : IServerMenuBuilder
	{
		private List<IModularPlugin> plugins;
		private IServerOutput output;
		private ServerIRCConnection ircConnection;
		
		private IModularPlugin innerPlugin;
		private MenuLocation location = MenuLocation.Main;
		
		public ServerMenuBuilder(List<IModularPlugin> plugins, IServerOutput output, ServerIRCConnection ircConnection)
		{
			this.plugins = plugins;
			this.output = output;
			this.ircConnection = ircConnection;
		}
		
		public bool BuildMenu(string input)
		{			
			int inputInt = -1;
			bool inputIn = int.TryParse(input, out inputInt);
			
			switch (location) {
				case ServerMenuBuilder.MenuLocation.Main:
					if(!inputIn) ListMain();
					else 
					{
						switch (inputInt)
						{
							case(0):
								ListPlugins();
								return true;
							case(1):
								return false;
							case(2):
								SendFakePublic();
								return true;
							default:
								return true;
						}
					}
					break;
				case ServerMenuBuilder.MenuLocation.ListPlugins:
					//User did not choose an integer from the list
					if(!inputIn) 
					{
						output.ThrowError("Input not an integer");
						break;
					}
					//Select the plugin as the chosen one :O
					if(inputInt >= plugins.Count) 
						ListMain();
					else
					{
						innerPlugin = plugins[inputInt];
						location = MenuLocation.InnerPlugin;
					}
					
					break;
				case ServerMenuBuilder.MenuLocation.InnerPlugin:
					//Send the input to the plugin
					
					break;
				default:
					return true;
			}
			
			return true;
		}
		
		private void ListMain()
		{
			output.ThrowMenu(0, "List Plugins");
			output.ThrowMenu(1, "Exit");
			location = MenuLocation.Main;
		}
		
		private void ListPlugins()
		{
			int pluginCount = plugins.Count;
			for(int i = 0 ; i < pluginCount; i++)
				output.ThrowMenu(i, plugins[i].GetPluginName());
			output.ThrowMenu(pluginCount, "Return to Main");
			location = MenuLocation.ListPlugins;
		}
		
		private void SendFakePublic()
		{
			string user = "", message = "", channel = "Channel";
			Console.WriteLine("User: ");
			user = Console.ReadLine();
			Console.WriteLine("Message: ");
			message = Console.ReadLine();
			ircConnection.IrcPublic(new Sharkbite.Irc.UserInfo(user, user, ""), channel, message);
			Console.ReadKey();
		}
		
		private enum MenuLocation
		{
			Main = 0, 
			ListPlugins,
			InnerPlugin
		}
	}
}
