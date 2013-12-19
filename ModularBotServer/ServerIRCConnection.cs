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
		
		private List<string> _moderators, _users, _subscribers, _admins;
		private bool _hasMod;
		
		private const string IRCHOST = "irc.twitch.tv";
		
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
			_irc = new Connection(connArgs);
			
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
			
			_irc.Listener.OnRegistered += IrcRegistered;
            _irc.Listener.OnNames += IrcOnNames;
            _irc.Listener.OnJoin += IrcJoined;
            _irc.Listener.OnPart += IrcPart;
            _irc.Listener.OnPublic += IrcPublic;
            _irc.Listener.OnPrivate += IrcPrivate;
            _irc.Listener.OnChannelModeChange += OnChannelModeChange;
			
			//todo: Write the IRC Connection
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
		
		private void IrcPublic(UserInfo user, string channel, string message)
        {
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
				
				_plugins.ForEach(s => s.OnCommand(user.User, command, args));
			}
			
			_plugins.ForEach(s => s.OnPublic(user.User, channel, message));
        }

        void IrcPrivate(UserInfo user, string message)
        {
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
            _plugins.ForEach(s => s.OnPrivate(user.User, message));
        }

        private void IrcJoined(UserInfo user, string channel)
        {
            //Check to see if the connecting user is the bot
            if (user.User.ToLower() == _username)
                _irc.Sender.Raw("JTVCLIENT"); //Allows the bot to receive OP/Mod and sub info

            _users.Add(user.User);
            _plugins.ForEach(s => s.OnJoined(user.User, channel));
        }

        void IrcPart(UserInfo user, string channel, string reason)
        {
            _users.Remove(user.User);

            if (_moderators.Contains(user.User)) _moderators.Remove(user.User);
            _plugins.ForEach(s => s.OnPart(user.User, channel, reason));
        }

        void IrcOnNames(string channel, string[] nicks, bool last)
        {
            //Add list of names to the current user list.
            _users.AddRange(nicks);
            _plugins.ForEach(s => s.OnNames(channel, nicks, last));
        }

        private void IrcRegistered()
        {
            //Turn off the Identd Server
            if(Identd.IsRunning()) Identd.Stop();

            //Join the corresponding channel
            _irc.Sender.Join("#" + _channel.ToLower());
            _plugins.ForEach(s => s.OnRegistered());
        }

        void OnChannelModeChange(UserInfo who, string channel, ChannelModeInfo[] modes)
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
	}
}
