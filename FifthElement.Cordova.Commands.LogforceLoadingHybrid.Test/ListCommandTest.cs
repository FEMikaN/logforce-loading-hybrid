using FifthElement.Cordova.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid.Test
{
    [TestClass]
    public class ListCommandTest
    {

        //before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //create new in-memory database connection
            FifthElement.LogforceLoadingHybrid.Database.DbConnectionManager.Instance.SetDefaultDatabase(":memory:", "1.0.0");
            var connection = FifthElement.LogforceLoadingHybrid.Database.DbConnectionManager.Instance.Connection;
        }


    }
}
