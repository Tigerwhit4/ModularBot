/*
 * User: Redback
 * Date: 19/12/2013
 * Time: 1:30 AM
 * 
 */
using System;
using ModularBot;

namespace ModularBot
{
	/// <summary>
	/// The main interface for running a module within the bot.
	/// </summary>
	public interface IModularPlugin
	{
		//Implementable by the user
		void Init();
		
		void Start();
		void Stop();
		
		string GetPluginName();
		
		//Implementable by the server
		void SetOutput(IServerOutput output);
		
		//IRC Actions
		CommandResponse OnRegistered();
		CommandResponse OnNames(string channel, string[] nicks, bool last);
		CommandResponse OnJoined(string user, string channel);
		CommandResponse OnPart(string user, string channel, string reason);
		CommandResponse OnPublic(string user, string channel, string message);
		CommandResponse OnPrivate(string user, string message);
		CommandResponse OnChannelModeChange(string user, string channel, string mode);
		CommandResponse OnCommand(string user, string command, string[] args);
	}
}
