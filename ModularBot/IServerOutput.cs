/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 11:18 AM
 * 
 */
using System;

namespace ModularBot
{
	/// <summary>
	/// Description of IServerOutput.
	/// </summary>
	public interface IServerOutput
	{
		void ThrowError(string errorMessage);
		void ThrowInfo(string infoMessage);
		void ThrowMenu(int menuOption, string menuMessage);
		void ThrowMessage(string messageType, string message);
		void ClearMessages();
	}
}
