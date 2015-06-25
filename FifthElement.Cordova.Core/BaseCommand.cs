﻿/*  
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at
	
	http://www.apache.org/licenses/LICENSE-2.0
	
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.Reflection;

namespace FifthElement.Cordova.Core
{
    public abstract class BaseCommand : IDisposable
    {
        /*
         *  All commands + plugins must extend BaseCommand, because they are dealt with as BaseCommands in PGView.xaml.cs
         *  
         **/

        #region IDisposable Members

        public void Dispose()
        {
            OnCommandResult = null;
        }

        #endregion

        public event EventHandler<PluginResult> OnCommandResult;

        public event EventHandler<ScriptCallback> OnCustomScript;

        /*
         *  InvokeMethodNamed will call the named method of a BaseCommand subclass if it exists and pass the variable arguments list along.
         **/

        public object InvokeMethodNamed(string methodName, params object[] args)
        {
            MethodInfo mInfo = GetType().GetMethod(methodName);

            if (mInfo != null)
            {
                // every function handles DispatchCommandResult by itself
                return mInfo.Invoke(this, args);
            }

            // actually methodName could refer to a property
            if (args == null || args.Length == 0 ||
                (args.Length == 1 && "undefined".Equals(args[0])))
            {
                PropertyInfo pInfo = GetType().GetProperty(methodName);
                if (pInfo != null)
                {
                    object res = pInfo.GetValue(this, null);

                    DispatchCommandResult(new PluginResult(PluginResult.Status.OK, res));

                    return res;
                }
            }

            throw new MissingMethodException(methodName);
        }


        public void InvokeCustomScript(ScriptCallback script)
        {
            if (OnCustomScript != null)
            {
                OnCustomScript(this, script);
            }
        }

        public void DispatchCommandResult()
        {
            DispatchCommandResult(new PluginResult(PluginResult.Status.NO_RESULT));
        }

        public void DispatchCommandResult(PluginResult result)
        {
            if (OnCommandResult != null)
            {
                OnCommandResult(this, result);

                if (!result.KeepCallback)
                {
                    Dispose();
                }
            }
        }
    }
}