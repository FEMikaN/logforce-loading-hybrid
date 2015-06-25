using System;
using System.Collections.Generic;

namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid
{
    public class ExtensionManager
    {

        private static readonly Dictionary<Type, object> Dependencies = new Dictionary<Type, object>();
        private static readonly Dictionary<string, object> Config = new Dictionary<string, object>();

        /// <summary>
        /// Injects an implementation to the specific interface. Used to provide host-
        /// specific implementations for behaviours needed by the plugins
        /// </summary>
        public static void Inject<TInterface, TImplementation>()
        {
            Dependencies[typeof (TInterface)] = typeof(TImplementation);
        }

        /// <summary>
        /// Injects an instance to the specific interface. Used to provide host-
        /// specific implementations for behaviours needed by the plugins
        /// </summary>
        public static void Inject<TInterface>(TInterface instance)
        {
            Dependencies[typeof (TInterface)] = instance;
        }

        /// <summary>
        /// Resolves as a dependency by type
        /// </summary>
        public static T Resolve<T>()
        {
            object implementation;
            if (Dependencies.TryGetValue(typeof(T), out implementation))
            {
                if(implementation is Type)
                    return (T)Activator.CreateInstance((Type)implementation);        

                return (T) implementation;
                
            }

            throw new Exception("No dependency injected for type " + typeof (T).Name);
        }
        
        /// <summary>
        /// Sets a configuration value by key
        /// </summary>
        public static void SetConfig(string key, object value)
        {
            Config[key] = value;
        }

        /// <summary>
        /// Gets a configuration value by key
        /// </summary>
        public static T GetConfig<T>(string key) where T:class
        {
            object value;
            if(Config.TryGetValue(key, out value))
                return (T) value;

            return null;
        }
    }
}
