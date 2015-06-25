using System;
using System.Linq;

namespace FifthElement.Cordova.Core
{
    /// <summary>
    /// Represents Cordova native command call: action callback, etc
    /// </summary>
    public class CordovaCommandCall
    {
        /// <summary>
        /// Private ctr to disable class creation.
        /// New class instance must be initialized via CordovaCommandCall.Parse static method.
        /// </summary>
        private CordovaCommandCall()
        {
        }

        public String Service { get; private set; }
        public String Action { get; private set; }
        public String CallbackId { get; private set; }
        public String Args { get; private set; }

        /// <summary>
        /// Retrieves command call parameters and creates wrapper for them
        /// </summary>
        /// <param name="commandStr">Command string in the form 'service/action/callback/args'</param>
        /// <returns>New class instance or null of string does not represent Cordova command</returns>
        public static CordovaCommandCall Parse(string commandStr)
        {
            if (string.IsNullOrEmpty(commandStr))
            {
                return null;
                //throw new ArgumentNullException("commandStr");
            }

            string[] split = commandStr.Split('/');
            if (split.Length < 3)
            {
                return null;
            }

            var commandCallParameters = new CordovaCommandCall();

            commandCallParameters.Service = split[0];
            commandCallParameters.Action = split[1];
            commandCallParameters.CallbackId = split[2];
            commandCallParameters.Args = split.Length <= 3 ? String.Empty : String.Join("/", split.Skip(3));

            // sanity check for illegal names
            // was failing with ::
            // CordovaCommandResult :: 1, Device1, {"status":1,"message":"{\"name\":\"XD.....
            if (commandCallParameters.Service.IndexOfAny(new[] {'@', ':', ',', '!', ' '}) > -1)
            {
                return null;
            }


            return commandCallParameters;
        }
    }
}