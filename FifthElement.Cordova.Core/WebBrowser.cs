namespace FifthElement.Cordova.Core
{
    public abstract class WebBrowser
    {
        public abstract string InvokeScript(string scriptName, params string[] args);
        public abstract void Notify(string message);

        public abstract void AddCommandId(string commandId);
        public abstract void RemoveCommandId(string commandId);
    }
}