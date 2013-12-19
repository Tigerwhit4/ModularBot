/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 4:01 PM
 * 
 */
using System;
using System.Collections.Generic;
using ModularBot;

namespace ModularBotServer
{
	/// <summary>
	/// Description of IServerIRCConnection.
	/// </summary>
	public interface IServerIRCConnection
	{
		bool Connect();
		bool Connect(string channel);
		bool Connect(string username, string password);
		bool Connect(string channel, string username, string password);
		
		void ResetConnection();
		void SetPlugins(List<IModularPlugin> plugins);
		void SetCommandPrefix(string commandPrefix);
	}
}
