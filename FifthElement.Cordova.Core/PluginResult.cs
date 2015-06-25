using System;
using System.Diagnostics;
using System.Text;

namespace FifthElement.Cordova.Core
{
    /// <summary>
    /// Represents command execution result
    /// </summary>
    public class PluginResult : EventArgs
    {
        #region Status enum

        /// <summary>
        /// Possible command results status codes
        /// </summary>
        public enum Status
        {
            NO_RESULT = 0,
            OK,
            CLASS_NOT_FOUND_EXCEPTION,
            ILLEGAL_ACCESS_EXCEPTION,
            INSTANTIATION_EXCEPTION,
            MALFORMED_URL_EXCEPTION,
            IO_EXCEPTION,
            INVALID_ACTION,
            JSON_EXCEPTION,
            ERROR
        } ;

        #endregion

        /// <summary>
        /// Predefined resultant messages
        /// </summary>
        public static string[] StatusMessages = new[]
                                                    {
                                                        "No result",
                                                        "OK",
                                                        "Class not found",
                                                        "Illegal access",
                                                        "Instantiation error",
                                                        "Malformed url",
                                                        "IO error",
                                                        "Invalid action",
                                                        "JSON error",
                                                        "Error"
                                                    };

        /// <summary>
        /// Creates new instance of the PluginResult class.
        /// </summary>
        /// <param name="status">Execution result</param>
        public PluginResult(Status status)
            : this(status, StatusMessages[(int) status])
        {
        }

        /// <summary>
        /// Creates new instance of the PluginResult class.
        /// </summary>
        /// <param name="status">Execution result</param>
        /// <param name="message">The message</param>
        public PluginResult(Status status, object message)
        {
            Result = status;
            Message = JsonHelper.Serialize(message);
        }

        /// <summary>
        /// Creates new instance of the PluginResult class.
        /// </summary>
        /// <param name="status">Execution result</param>
        /// <param name="message">The message</param>
        /// <param name="cast">The cast parameter</param>
        /// 
        [Obsolete("Don't use Cast!!", false)]
        public PluginResult(Status status, object message, string cast)
        {
            Result = status;
            Message = JsonHelper.Serialize(message);
            Cast = cast;
        }

        public Status Result { get; private set; }
        public string Message { get; set; }
        public String Cast { get; private set; }

        public bool KeepCallback { get; set; }

        /// <summary>
        /// Whether command succeded or not
        /// </summary>
        public bool IsSuccess
        {
            get { return Result == Status.OK || Result == Status.NO_RESULT; }
        }

        public string ToJSONString()
        {
            string res = String.Format("\"status\":{0},\"message\":{1},\"keepCallback\":{2}",
                                       (int) Result,
                                       Message,
                                       KeepCallback.ToString().ToLower());

            res = "{" + res + "}";
            return res;
        }

        public string ToCallbackString(string callbackId, string successCallback, string errorCallback)
        {
            //return String.Format("{0}('{1}',{2});", successCallback, callbackId, this.ToJSONString());

            if (IsSuccess)
            {
                var buf = new StringBuilder("");
                if (Cast != null)
                {
                    Debug.WriteLine(callbackId + "this.Cast = " + Cast);
                    buf.Append("var temp = " + Cast + "(" + ToJSONString() + ");\n");
                    buf.Append(String.Format("{0}('{1}',temp);", successCallback, callbackId));
                }
                else
                {
                    buf.Append(String.Format("{0}('{1}',{2});", successCallback, callbackId, ToJSONString()));
                }
                return buf.ToString();
            }
            else
            {
                return String.Format("{0}('{1}',{2});", errorCallback, callbackId, ToJSONString());
            }
        }

        public override String ToString()
        {
            return ToJSONString();
        }
    }
}