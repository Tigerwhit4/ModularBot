/*
 * User: Nick
 * Date: 19/12/2013
 * Time: 3:44 PM
 * 
 */
using System;

namespace ModularBot
{
	/// <summary>
    /// Class for passing command responses
    /// </summary>
    public class CommandResponse
    {
        //Declare Variables
        public string Message;
        public ResponseAction Action;

        //Default none response
        public static CommandResponse None { get { return new CommandResponse(ResponseAction.None); } }

        /// <summary>
        /// Constructor for default response (without custom message)
        /// </summary>
        /// <param name="action">The response action</param>
        public CommandResponse(ResponseAction action)
        {
            Action = action;
        }

        /// <summary>
        /// Constructor for default response
        /// </summary>
        /// <param name="message">The message for the chat</param>
        /// <param name="action">The reponse action</param>
        public CommandResponse(string message, ResponseAction action)
        {
            Message = message;
            Action = action;
        }

        /// <summary>
        /// Enum for handling the actions for responding
        /// </summary>
        public enum ResponseAction
        {
            None = 0,
            Warning = 1,
            Timeout = 2,
            Ban = 3,
            Purge = 4
        }   
    }
}
