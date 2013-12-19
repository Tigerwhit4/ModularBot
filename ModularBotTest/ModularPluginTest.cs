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
		
		public override CommandResponse OnPublic(string user, string channel, string message)
		{
			CommandResponse response = CommandResponse.None;
			response.Message = user + " said " + message;
			return response;
		}
	}
}
