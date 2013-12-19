/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 10:35 AM
 * 
 */
using System;

namespace ModularBotServer
{
	class Program
	{
		public static void Main(string[] args)
		{
			ModularBotServer server = new ModularBotServer();
			//Set server config
			server.Start();
		}
	}
}