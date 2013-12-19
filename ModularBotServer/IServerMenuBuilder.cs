/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 11:26 AM
 * 
 */
using System;

namespace ModularBotServer
{
	/// <summary>
	/// Description of IServerMenuBuilder.
	/// </summary>
	public interface IServerMenuBuilder
	{
		bool BuildMenu(string input);
	}
}
