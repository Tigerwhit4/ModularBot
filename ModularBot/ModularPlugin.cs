/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 1:38 AM
 * 
 */
using System;
using ModularBot;

namespace ModularBot
{
	/// <summary>
	/// Description of ModularPlugin.
	/// </summary>
	public abstract class ModularPlugin : IModularPlugin
	{
		protected IServerOutput m_output;
		
		public void SetOutput(IServerOutput output)
		{
			m_output = output;
		}
		
		public abstract string GetPluginName();
		public abstract void Init();
		public abstract void Start();
		public abstract void Stop();
		
		#region IRC Commands
		public virtual CommandResponse OnRegistered()
		{
			return null;
		}
		
		public virtual CommandResponse OnNames(string channel, string[] nicks, bool last)
		{
			return null;
		}
		
		public virtual CommandResponse OnJoined(string user, string channel)
		{
			return null;
		}
		
		public virtual CommandResponse OnPart(string user, string channel, string reason)
		{
			return null;
		}
		
		public virtual CommandResponse OnPublic(string user, string channel, string message)
		{
			return null;
		}
		
		public virtual CommandResponse OnPrivate(string user, string message)
		{
			return null;
		}
		
		public virtual CommandResponse OnChannelModeChange(string user, string channel, string mode)
		{
			return null;
		}
		
		public CommandResponse OnCommand(string user, string command, string[] args)
		{
			return null;
		}
		#endregion
	}
}
