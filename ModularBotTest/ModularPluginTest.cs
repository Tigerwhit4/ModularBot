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
			throw new NotImplementedException();
		}
		
		public override void Start()
		{
			throw new NotImplementedException();
		}
		
		public override void Init()
		{
			m_output.ThrowInfo(string.Format("[{0}] Loaded!", name));
		}
		
		public override string GetPluginName()
		{
			return name;
		}
	}
}
