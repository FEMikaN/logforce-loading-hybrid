using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid
{
    public interface IWebBrowserService
    {
        void ClearCookies();

        void Reload();
    }
}
