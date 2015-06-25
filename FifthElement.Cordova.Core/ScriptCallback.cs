using System;

namespace FifthElement.Cordova.Core
{
    /// <summary>
    /// Represents client script function to execute
    /// </summary>
    public class ScriptCallback : EventArgs
    {
        /// <summary>
        /// Creates new instance of a ScriptCallback class.
        /// </summary>
        /// <param name="function">The scripting function to execute</param>
        /// <param name="args">A variable number of strings to pass to the function as parameters</param>
        public ScriptCallback(string function, string[] args)
        {
            ScriptName = function;
            Args = args;
        }

        /// <summary>
        /// Creates new instance of a ScriptCallback class.
        /// </summary>
        /// <param name="function">The scripting function to execute</param>
        /// <param name="id">The id argument</param>
        /// <param name="msg">The message argument</param>
        /// <param name="value">The value argument</param>
        public ScriptCallback(string function, string id, object msg, object value)
        {
            ScriptName = function;

            String arg = String.Format("{{\"id\": {0}, \"msg\": {1}, \"value\": {2}}}",
                                       JsonHelper.Serialize(id), JsonHelper.Serialize(msg), JsonHelper.Serialize(value));

            Args = new[] {arg};
        }

        /// <summary>
        /// The scripting function to execute.
        /// </summary>
        public string ScriptName { get; private set; }

        /// <summary>
        /// A variable number of strings to pass to the function as parameters.
        /// </summary>
        public string[] Args { get; private set; }
    }
}