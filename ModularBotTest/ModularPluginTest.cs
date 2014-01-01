/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 10:39 AM
 * 
 */
using System;
using System.ComponentModel.Composition;
using ModularBot;

namespace ModularBotTest
{
	/// <summary>
	/// Description of ModularPluginTest.
	/// </summary>
	[Export(typeof(ModularPlugin))]
	public class ModularPluginTest : ModularPlugin
	{		
		public string name = "ModularTestPlugin";
		
		public override void Stop()
		{
			m_Output.ThrowPluginInfo(this, "Successfully Stopped");
		}
		
		public override void Start()
		{
			m_Output.ThrowPluginInfo(this, "Successfully Started");
		}
		
		public override void Init()
		{
			m_Output.ThrowPluginInfo(this, "Loaded!");
		}
		
		public override string GetPluginName()
		{
			return name;
		}
		
		public override CommandResponse OnCommand(string user, string command, string[] args)
		{
			if(command == "info" && args.Length == 0)
			{
				return new CommandResponse("This is the ModularBot Test Plugin. Type \"!info 1\" to continue.", CommandResponse.ResponseAction.None);
			}
			else if(command == "info" && args.Length == 1)
			{
				switch (args[0]) {
					case("1"):
						return new CommandResponse("This plugin comes default with ModularBot to test it. Type \"!info 2\" to continue.", CommandResponse.ResponseAction.None);
					case("2"):
						return new CommandResponse("For more information, visit the source at https://github.com/Redback93/ModularBot", CommandResponse.ResponseAction.None);
					default:
						break;
				}
			}
			else if(command == "purge")
			{
				return new CommandResponse("Purged [" + user + "]", CommandResponse.ResponseAction.Purge);
			}
			return CommandResponse.None;
		}
	}
}
