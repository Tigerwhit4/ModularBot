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
			
			//Start the plugins
			StartPlugins();
			
			//Wait for user input
			Thread listenThread = new Thread(ListenLoop);
			listenThread.Start();
			
			m_MenuBuilder = new ServerMenuBuilder(m_Plugins, m_Output);
		}
		
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
					m_Output.ThrowError(ex.Message);
				}
			}
		}
		
		private void StartPlugins()
		{
			
		}
		
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
		}
	}
}
