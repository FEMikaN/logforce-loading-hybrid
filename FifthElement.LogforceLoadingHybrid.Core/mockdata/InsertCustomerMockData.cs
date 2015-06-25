using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FifthElement.Kotka.Core.mockdata
{
    [DataContract]
    public class InsertCustomerMockData
    {
        [DataMember(Name = "customerId")]
        public int? CustomerId { get; set; } 
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
        [DataMember(Name = "address")]
        public string Address { get; set; }
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
        [DataMember(Name = "postOffice")]
        public string PostOffice { get; set; }

        //public DynamicParameters InsertParameters
        //{
        //    get
        //    {
        //        var values = new DynamicParameters();
        //        values.Add("CustomerId", CustomerId, DbType.Int16);
        //        values.Add("FirstName", FirstName, DbType.String);
        //        values.Add("LastName", LastName, DbType.String);
        //        values.Add("Address", Address, DbType.String);
        //        values.Add("PostalCode", PostalCode, DbType.String);
        //        values.Add("PostOffice", PostOffice, DbType.String);

        //        var json = JsonConvert.SerializeObject(this);
        //        values.Add("JSON",json,DbType.String);
        //        return values;
        //    }
        //}
        //public string Sql
        //{
        //    get
        //    {
        //        var insertFields = new List<string>();
        //        var insertValues = new List<string>();
        //        foreach (var paramName in InsertParameters.ParameterNames)
        //        {
        //            insertFields.Add(paramName);
        //            insertValues.Add(":" + paramName);
        //        }
        //        return String.Format("insert into customer ({0}) values ({1})", String.Join(",", insertFields), String.Join(",", insertValues));
        //    }
        //}

    }
}
