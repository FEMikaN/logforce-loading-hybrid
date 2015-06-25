namespace FifthElement.Kotka.Core.mockdata
{
    public static class MockData
    {
        public const string ActivitiesJson = @"[
{
            ""activityId"": 1001,
            ""activityClass"": 1,
            ""ownedById"": 222,
            ""createdById"": 222,
            ""startDateTime"": ""2013-02-02T00:00:00.000Z"",
            ""endDateTime"": ""2013-03-02T00:00:00.000Z"",
            ""title"": ""Yhteydenotto"",
            ""customerId"": 111,
            ""description"": ""Sovittiin, etta palataan metsähoitotarpeisiin kevattalvella."",
            ""customerFirstName"": ""Matti"",
            ""customerLastName"": ""Mallikas""
},
{
            ""activityId"": 1002,
            ""activityClass"": 1,
            ""ownedById"": 222,
            ""createdById"": 222,
            ""startDateTime"": ""2013-02-10T00:00:00.000Z"",
            ""endDateTime"": ""2013-03-10T00:00:00.000Z"",
            ""title"": ""Yhteydenotto"",
            ""customerId"": 111,
            ""description"": ""Sovittiin, etta palataan metsähoitotarpeisiin kevattalvella."",
            ""customerFirstName"": ""Matti"",
            ""customerLastName"": ""Mallikas""
},
{
            ""activityId"": 1003,
            ""activityClass"": 1,
            ""ownedById"": 222,
            ""createdById"": 222,
            ""startDateTime"": ""2013-02-02T00:00:00.000Z"",
            ""endDateTime"": ""2013-03-02T00:00:00.000Z"",
            ""title"": ""Yhteydenotto"",
            ""customerId"": 111,
            ""description"": ""Sovittiin, etta palataan metsähoitotarpeisiin kevattalvella."",
            ""customerFirstName"": ""Matti"",
            ""customerLastName"": ""Mallikas""
},
{
            ""activityId"": 1004,
            ""activityClass"": 1,
            ""ownedById"": 222,
            ""createdById"": 222,
            ""startDateTime"": ""2013-07-03T00:00:00.000Z"",
            ""endDateTime"": ""2013-07-11T00:00:00.000Z"",
            ""title"": ""Yhteydenotto"",
            ""customerId"": 222,
            ""description"": ""Sovittiin, että otetaan yhteyttä loppusyksystä ja keskustellaan Jaskalan tilan 15 puukaupasta."",
            ""customerFirstName"": ""Maija"",
            ""customerLastName"": ""Mallikas""
}]"; 

        public const string CustomersJson = @"[
        {
            ""customerId"":111,
            ""firstName"":""Matti"",
            ""lastName"":""Mallikas"",
            ""address"": ""Kiteentie 125"",
            ""postalCode"": ""82500"",
            ""postOffice"": ""Kitee""
        },
        {
            ""customerId"":222,
            ""firstName"":""Maija"",
            ""lastName"":""Mallikas"",
            ""address"": ""Kiteentie 125"",
            ""postalCode"": ""82500"",
            ""postOffice"": ""Kitee""
        }
]";

        public const string UsersettingsJson = @"
        {
            ""userId"": ""domain\\userid"",
            ""organization"": ""myynti"",
            ""tjNumber"": 500,
            ""hilaDate"": ""2011-07-06T00:00:00.000Z"",
            ""lastMapUpdateDate"": ""2011-07-06T00:00:00.000Z"",
            ""defaultMapCoordinate"": ""X,Y"",
            ""userInfo"": ""muuta tietoa"",
            ""sapUserName"": ""sapKayttaja"",
            ""sapPassword"": ""sapsalasana""
        }";

        public const string OfferGetList = @"{""status"":1,""message"":[
             {
             ""offerId"":543173012,
             ""customerId"":7639303,
             ""customerFirstName"":""MAATALOUSYHTYMÄ"",
             ""customerLastName"":""HANDOLIN MATTI JA PAAVO"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""false"",
             ""title"":""PAKOJA"",
             ""municipalities"":""NURMIJÄRVI"",
             ""offerDate"":""2013-02-21T00:00:00.000Z"",
             ""offerValidDate"":""2013-02-04T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543172094,
             ""customerId"":7739293,
             ""customerFirstName"":""Terhi"",
             ""customerLastName"":""Rämö"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""false"",
             ""title"":"""",
             ""municipalities"":""????"",
             ""offerDate"":""2013-02-21T00:00:00.000Z"",
             ""offerValidDate"":""2012-11-13T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543173015,
             ""customerId"":3885464,
             ""customerFirstName"":""JUKKA"",
             ""customerLastName"":""ESKOLA"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""false"",
             ""title"":""ESKOLA"",
             ""municipalities"":""NURMIJÄRVI"",
             ""offerDate"":""2013-02-19T00:00:00.000Z"",
             ""offerValidDate"":""2013-02-25T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543172101,
             ""customerId"":7739476,
             ""customerFirstName"":""Veli Artturi KP"",
             ""customerLastName"":""Virtanen"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""true"",
             ""title"":"""",
             ""municipalities"":""????"",
             ""offerDate"":""2013-02-04T00:00:00.000Z"",
             ""offerValidDate"":""2012-11-27T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543172102,
             ""customerId"":7739478,
             ""customerFirstName"":""Jari Veli"",
             ""customerLastName"":""Virtanen"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""true"",
             ""title"":"""",
             ""municipalities"":""????"",
             ""offerDate"":""2013-02-04T00:00:00.000Z"",
             ""offerValidDate"":""2012-11-27T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543173013,
             ""customerId"":3961927,
             ""customerFirstName"":""PEKKA"",
             ""customerLastName"":""PEURA"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""true"",
             ""title"":""SATULI"",
             ""municipalities"":""NURMIJÄRVI"",
             ""offerDate"":""2013-02-01T00:00:00.000Z"",
             ""offerValidDate"":""2013-02-06T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543173002,
             ""customerId"":7740503,
             ""customerFirstName"":""Kerttu Alice Kp."",
             ""customerLastName"":""Havulehto"",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""true"",
             ""title"":"""",
             ""municipalities"":""????"",
             ""offerDate"":""2013-01-25T00:00:00.000Z"",
             ""offerValidDate"":""2013-01-23T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ,
             {
             ""offerId"":543172108,
             ""customerFirstName"":""MTY Heikkilä"",
             ""customerLastName"":"""",
             ""hasWoodTradeOffer"":""true"",
             ""hasSilvicWorkOffer"":""true"",
             ""title"":"""",
             ""municipalities"":""????"",
             ""offerDate"":""2013-01-24T00:00:00.000Z"",
             ""offerValidDate"":""2012-12-20T00:00:00.000Z"",
             ""infoText"":""tähän tulee infotekstiä""
             }
         ],""keepCallback"":""false""}";
    }
}
