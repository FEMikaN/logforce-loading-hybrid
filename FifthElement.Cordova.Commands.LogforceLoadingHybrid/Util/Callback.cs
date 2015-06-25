namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util
{
    public class Callback
    {
        public bool Success { get; private set; }
        public object ReturnValue { get; private set; }

        public Callback(bool success)
        {
            Success = success;
            ReturnValue = null;
        }

        public Callback(bool success, object returnValue)
        {
            Success = success;
            ReturnValue = returnValue;
        }
    }
}