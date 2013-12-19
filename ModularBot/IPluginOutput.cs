/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 4:33 PM
 * 
 */
using System;

namespace ModularBot
{
	/// <summary>
	/// Description of IPluginOutput.
	/// </summary>
	public interface IPluginOutput
	{
		void ThrowPluginInfo(IModularPlugin plugin, string infoMessage);
		void ThrowPluginError(IModularPlugin plugin, string errorMessage);
	}
}
