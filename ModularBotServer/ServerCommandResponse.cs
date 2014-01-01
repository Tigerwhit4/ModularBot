/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 11:24 PM
 * 
 */
using System;
using ModularBot;

namespace ModularBotServer
{
	/// <summary>
	/// Description of ServerCommandResponse.
	/// </summary>
	public class ServerCommandResponse : CommandResponse
	{
		public IModularPlugin Plugin;
		
		public ServerCommandResponse(ResponseAction action) : base(action){}
		public ServerCommandResponse(string message, ResponseAction action) : base(message, action) {}
		
		public static ServerCommandResponse ParseServerCommandResponse(CommandResponse response, IModularPlugin plugin)
		{
			if(response == null) return null;
			ServerCommandResponse returnResponse = new ServerCommandResponse(response.Message, response.Action);
			returnResponse.Plugin= plugin;
			return returnResponse;			
		}
	}
}
