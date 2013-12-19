/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 10:47 AM
 * 
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ModularBot;

namespace ModularBotServer
{
	/// <summary>
	/// Description of ModularBotServer.
	/// </summary>
	public class ModularBotServer
	{
		//Globals Declaration
		private IServerOutput m_Output;
		private IServerMenuBuilder m_MenuBuilder;
		private List<IModularPlugin> m_Plugins;
		private IServerIRCConnection m_IRCConnection;
		
		public ModularBotServer()
		{
			m_Output = new ServerOutput();
		}
		
		public void Start()
		{
			//Load all of the plugins
			LoadPlugins();
			
			//Run init
			InitPlugins();
			
			//Connect to the IRC
			if(!SetupIRC()) 
				return;
			
			//Start the plugins
			StartPlugins();
			
			//Wait for user input
			Thread listenThread = new Thread(ListenLoop);
			listenThread.Start();
			
			m_MenuBuilder = new ServerMenuBuilder(m_Plugins, m_Output);
		}
		
		private bool SetupIRC()
		{
			m_IRCConnection = new ServerIRCConnection();
			
			string username = "", password = "", channel = "";
			username = ConfigurationManager.AppSettings["IRCUsername"];
			password = ConfigurationManager.AppSettings["IRCPassword"];
			channel = ConfigurationManager.AppSettings["IRCChannel"];
			
			if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) 
			{
				Abort("Username and/or password is empty");
				return false;
			}
			
			if(string.IsNullOrEmpty(channel))
			{
				Abort("Channel setting is empty");
				return false;
			}
			
			//Set the command prefix
			string commandPrefix = ConfigurationManager.AppSettings["CommandPrefix"];
			m_IRCConnection.SetCommandPrefix(commandPrefix);
			if(string.IsNullOrEmpty(commandPrefix))
			{
				m_Output.ThrowError("No command prefix was set");
			}
			
			if(!m_IRCConnection.Connect(channel, username, password))
			{
				Abort("Could not connect to the chat");
				return false;
			}
			
			m_Output.ThrowInfo("Connected!");
			return true;
		}
		
		#region Plugins
		private void LoadPlugins()
		{
			//Inform user
			m_Output.ThrowInfo("Loading plugins");
			
			//Clear the current plugin list
			m_Plugins = new List<IModularPlugin>();
			
			//Get the location information from the app.config
			string pluginLocation = ConfigurationManager.AppSettings["PluginLocation"];
			
			//Check for existance of directory
			if(!Directory.Exists(pluginLocation))
			{
				m_Output.ThrowInfo("No plugin directory found");
				return;
			}
			
			//Get all of the files within the directory
			string startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			string[] pluginFiles = Directory.GetFiles(pluginLocation, "*.dll");
			
			try
			{
				var plugins = (
				    // From each file in the files.
				    from file in pluginFiles
				    // Load the assembly.
				    let asm = Assembly.LoadFile(startupPath + "\\" + file)
				    // For every type in the assembly that is visible outside of
				    // the assembly.
				    from type in asm.GetExportedTypes()
				    // Where the type implements the interface.
				    where typeof(ModularPlugin).IsAssignableFrom(type)
				    // Create the instance.
				    select (ModularPlugin) Activator.CreateInstance(type)
				).ToArray();
				
							
				m_Plugins.AddRange(plugins);
			}
			catch(Exception ex)
			{
				m_Output.ThrowError(ex.Message);
			}
		}
		
		private void InitPlugins()
		{
			foreach(ModularPlugin plugin in m_Plugins)
			{
				//Attempt to init the plugin
				try
				{
					plugin.SetOutput(m_Output);
					
					plugin.Init();
				}
				catch(Exception ex)
				{
					m_Output.ThrowPluginError(plugin.GetPluginName(), ex.Message);
				}
			}
		}
		
		private void StartPlugins()
		{
			foreach(ModularPlugin plugin in m_Plugins)
			{
				try
				{
					plugin.Start();
				}
				catch(Exception ex)
				{
					m_Output.ThrowPluginError(plugin.GetPluginName(), ex.Message);
				}
			}
		}
		
		private void StopPlugins()
		{
			foreach(ModularPlugin plugin in m_Plugins)
			{
				try
				{
					plugin.Stop();
				}
				catch(Exception ex)
				{
					m_Output.ThrowError(ex.Message);
				}
			}
		}
		#endregion
		
		private void ListenLoop()
		{
			bool blocking = true;
			string input = string.Empty;
			
			//Build an initial menu
			m_MenuBuilder.BuildMenu(input);
			do
			{
				input = Console.ReadLine();
				
				//Clear the current screen
				m_Output.ClearMessages();
				
				blocking = m_MenuBuilder.BuildMenu(input);
			} while(blocking);
			
			//Run the stop code
			StopPlugins();
			
			//Leave a final readkey before exiting
			m_Output.Pause();
		}
		
		private void Abort(string abortReason)
		{
			m_Output.ThrowError("Abort: " + abortReason);
			m_Output.Pause();
		}
	}
}
