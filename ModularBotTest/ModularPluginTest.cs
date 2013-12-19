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
				return new CommandResponse("This is the ModularBot Test Plugin. Type \"!info 1\" to continue", CommandResponse.ResponseAction.None);
			}
			if(command == "info" && args.Length == 1)
			{
				switch (args[0]) {
					case("1"):
						return new CommandResponse("This plugin comes default with ModularBot to test it.", CommandResponse.ResponseAction.None);
						break;
					default:
						break;
				}
			}
			return CommandResponse.None;
		}
	}
}
