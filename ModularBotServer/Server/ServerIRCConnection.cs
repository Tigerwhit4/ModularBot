/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 3:59 PM
 * 
 */
using System;
using System.Collections.Generic;
using ModularBot;
using Sharkbite.Irc;

namespace ModularBotServer
{
	/// <summary>
	/// Description of ServerIRCConnection.
	/// </summary>
	public class ServerIRCConnection : IServerIRCConnection
	{
		private string _channel, _username, _password;
		private string _commandPrefix;
		private Connection _irc;
		private List<IModularPlugin> _plugins = new List<IModularPlugin>();
		private IServerOutput _output;
		
		private List<string> _moderators, _users, _subscribers, _admins;
		private bool _hasMod;
		
		private const string IRCHOST = "irc.twitch.tv";
		
		public ServerIRCConnection(IServerOutput output)
		{
			_output = output;
		}
		
		public void SetPlugins(List<IModularPlugin> plugins)
		{
			_plugins = plugins;
		}
		
		public void SetCommandPrefix(string commandPrefix)
		{
			_commandPrefix = commandPrefix;
		}
		
		public void ResetConnection()
		{
			//Reset the IRC connection
			if(_irc != null && _irc.Connected) _irc.Disconnect("Force quit");
			
			//Create the new IRC connection
			ConnectionArgs connArgs = new ConnectionArgs(_username, IRCHOST);
			connArgs.ServerPassword = _password;
			_irc = new Connection(connArgs, false, false);
			
			//Reset the lists
			_moderators = new List<string>();
			_users = new List<string>();
			_subscribers = new List<string>();
			_admins = new List<string>();
		}
		
		#region ConnectOverrides
		public bool Connect()
		{
			//Bottom of the barrel
			ResetConnection();
			
			if(!Identd.IsRunning()) Identd.Start(_username);
			
			_irc.Listener.OnRegistered += IrcRegistered;
            _irc.Listener.OnNames += IrcOnNames;
            _irc.Listener.OnJoin += IrcJoined;
            _irc.Listener.OnPart += IrcPart;
            _irc.Listener.OnPublic += IrcPublic;
            _irc.Listener.OnPrivate += IrcPrivate;
            _irc.Listener.OnChannelModeChange += OnChannelModeChange;
			
			try
            {
                //Attempt a connection the server
                _irc.Connect();
            }
            catch(Exception ex)
            {
                return false;
            }
            
            return true;
		}
		
		public bool Connect(string channel)
		{
			_channel = channel;
			return Connect();
		}
		
		public bool Connect(string username, string password)
		{
			_username = username;
			_password = password;
			return Connect();
		}
		
		public bool Connect(string channel, string username, string password)
		{
			_channel = channel;
			return Connect(username, password);
		}
		#endregion
		
		#region IRC Actions
		public void IrcPublic(UserInfo user, string channel, string message)
        {
			List<ServerCommandResponse> responses = new List<ServerCommandResponse>();
			
			//Check for command
			if(message.StartsWith(_commandPrefix))
			{
				//Take off the command prefix
				string cutMessage = message.Remove(0, _commandPrefix.Length);
				
				string[] commandSplit = cutMessage.Split(' ');
				var command = commandSplit[0];
				var args = new string[(commandSplit.Length > 1 ? commandSplit.Length - 1 : 0)];
				for(int i = 0; i < args.Length; i++)
					args[i] = commandSplit[i+1];
				
				_plugins.ForEach(s => responses.Add(ServerCommandResponse.ParseServerCommandResponse(s.OnCommand(user.User, command, args), s)));
			}
			_plugins.ForEach(s => responses.Add(ServerCommandResponse.ParseServerCommandResponse(s.OnPublic(user.User, channel, message), s)));
			CommandRespond(responses, user.User);
        }

        public void IrcPrivate(UserInfo user, string message)
        {
        	List<ServerCommandResponse> responses = new List<ServerCommandResponse>();
            //Split into parts
            var messageSplit = message.Split(' ');
            //Check for empty array
            if (messageSplit.Length < 3) return;

            //Determine if it is a new sub or admin
            //eg: SPECIALUSER redback93 subscriber/admin
            if (messageSplit[0].ToUpper() != "SPECIALUSER") return;
            if (messageSplit[2].ToUpper() == "SUBSCRIBER")
                _subscribers.Add(messageSplit[1]);
            else if (messageSplit[2].ToUpper() == "ADMIN")
            {
                _admins.Add(messageSplit[1]);
            }
            _plugins.ForEach(s => responses.Add(ServerCommandResponse.ParseServerCommandResponse(s.OnPrivate(user.User, message), s)));
            CommandRespond(responses, user.User);
        }

        public void IrcJoined(UserInfo user, string channel)
        {
            //Check to see if the connecting user is the bot
            if (user.User.ToLower() == _username)
                _irc.Sender.Raw("JTVCLIENT"); //Allows the bot to receive OP/Mod and sub info

            _users.Add(user.User);
            List<ServerCommandResponse> responses = new List<ServerCommandResponse>();
            _plugins.ForEach(s => responses.Add(ServerCommandResponse.ParseServerCommandResponse(s.OnJoined(user.User, channel), s)));
            CommandRespond(responses, user.User);
        }

        public void IrcPart(UserInfo user, string channel, string reason)
        {
            _users.Remove(user.User);

            if (_moderators.Contains(user.User)) _moderators.Remove(user.User);
            List<ServerCommandResponse> responses = new List<ServerCommandResponse>();
            _plugins.ForEach(s => responses.Add(ServerCommandResponse.ParseServerCommandResponse(s.OnPart(user.User, channel, reason), s)));
            CommandRespond(responses, user.User);
        }

        public void IrcOnNames(string channel, string[] nicks, bool last)
        {
            //Add list of names to the current user list.
            _users.AddRange(nicks);
            _plugins.ForEach(s => s.OnNames(channel, nicks, last));
        }

        public void IrcRegistered()
        {
            //Turn off the Identd Server
            if(Identd.IsRunning()) Identd.Stop();

            //Join the corresponding channel
            _irc.Sender.Join("#" + _channel.ToLower());
            _plugins.ForEach(s => s.OnRegistered());
        }

        public void OnChannelModeChange(UserInfo who, string channel, ChannelModeInfo[] modes)
        {
            foreach (var mode in modes)
            {
                //Parameter contains the user's name
                var user = mode.Parameter;

                //Determine if the mode is changing operators (mods)
                if (mode.Mode == ChannelMode.ChannelOperator)
                {
                    //Check to see if it's the bot
                    if (user.ToUpper() == _username.ToUpper())
                    {
                        //Determine if the mode is adding or removing
                        _hasMod = (mode.Action == ModeAction.Add);
                    }
                    if (mode.Action == ModeAction.Add)
                    {
                        //Add to moderator list
                        _moderators.Add(user);
                    }
                    else
                    {
                        //Otherwise remove them
                        if (_moderators.Contains(user))
                            _moderators.Remove(user);
                    }
                }
            }
            
            foreach(var mode in modes)
            	_plugins.ForEach(s => s.OnChannelModeChange(who.User, channel,mode.Mode.ToString()));
        }
        #endregion
        
                
        private void CommandRespond(List<ServerCommandResponse> responses, string user)
        {
        	foreach(var response in responses) if(response != null) CommandRespond(response, user);
        }
        
        private void CommandRespond(ServerCommandResponse response, string user)
        {
        	//Check to see if there was any response
            if (response == null) return;

            //Check for response message
            if(!string.IsNullOrWhiteSpace(response.Message))
            {
                //Sends the required message
                SendMessage(response.Message);
                _output.ThrowPluginInfo(response.Plugin.GetPluginName(), "Sent: " + response.Message);
            }

            //Create an action response only if you have moderator
            if(response.Action != CommandResponse.ResponseAction.None)
            {
                //Make sure it has the permissions
                if (!_hasMod) return;

                //Message containing action info
                var message = "";
                switch(response.Action)
                {
                    case(CommandResponse.ResponseAction.Timeout):
                        message = "/timeout " + user;
                        break;
                    case (CommandResponse.ResponseAction.Purge):
                        message = "/timeout " + user + " 1";
                        break;
                    case (CommandResponse.ResponseAction.Ban):
                        message = "/ban " + user;
                        break;
                }

                //Send the response message
                if(!string.IsNullOrWhiteSpace(message))
                {
                    SendMessage(message);
                    _output.ThrowPluginInfo(response.Plugin.GetPluginName(), "Sent: " + message);
                }
            }
        }
		
        private void SendMessage(string message)
        {
            //Sends the public message
            _irc.Sender.PublicMessage("#" + _channel.ToLower(), message);
        }
        
        public void Disconnect()
        {
        	if(_irc != null && _irc.Connected) _irc.Disconnect("Stopping");
        }
        
        public bool IsConnected()
        {
        	return _irc.Connected;
        }
	}
}
