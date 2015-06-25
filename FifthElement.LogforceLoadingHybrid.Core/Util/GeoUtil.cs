using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using FifthElement.LogforceLoadingHybrid.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class GeoUtil
    {
        public enum GeometryType
        {
            Unknown = -1,
            Point = 0,
            LineString,
            Polygon
        };

        public static GeometryType GeometryTypeFromString(string typeAsString)
        {
            GeometryType result;
            return Enum.TryParse(typeAsString, false, out result) ? result : GeometryType.Unknown;
        }

        public static string OgcEstateFilter(List<string> estates)
        {
            if (estates.Count == 0) return null;
            var result = new XElement("Filter");
            var xml = (estates.Count > 1) ? new XElement("OR") : result;
            foreach (var item in estates)
            {
                var estate = (item.EndsWith("-0000")) ? item.Substring(0, item.Length - 5) : item;
                var newnode = new XElement("PropertyIsEqualTo", new XElement("PropertyName", "REG_UNIT_ID"));
                newnode.Add(new XElement("Literal", estate));
                xml.Add(newnode);
            }
            if (xml != result) result.Add(xml);
            return result.ToString(SaveOptions.DisableFormatting);
        }

        public static string CommaSeparatedEstateFilter(List<string> estates)
        {
            var quotedList = estates.Select(item => String.Format("'{0}'", item)).ToList();

            return String.Join(",", quotedList);
        }

        public static List<Decimal> GetBboxFromXmlFeatureCollection(string xmlString)
        {
            var xml = XElement.Parse(xmlString);
            var gml = xml.GetNamespaceOfPrefix("gml");

            //XNamespace gml = "http://www.opengis.net/gml";
            var node = xml.Element(gml + "boundedBy");
            if (node == null) return null;
            node = node.Element(gml + "Envelope");
            if (node == null) return null;

            var lowerCornerNode = node.Element(gml + "lowerCorner");
            if (lowerCornerNode == null) return null;

            var upperCornerNode = node.Element(gml + "upperCorner");
            if (upperCornerNode == null) return null;
            
            var corners = lowerCornerNode.Value + " " + upperCornerNode.Value;
            return corners.Split(' ').Select(stringValue => Convert.ToDecimal(stringValue, CultureInfo.InvariantCulture)).ToList();
        }  
        public static string GetDashedEstateId(string ktjId)
        {
            if (ktjId.Contains("-")) return ktjId;
            if (ktjId.Length < 14) return ktjId;
            if (ktjId.Length == 14) return ktjId.Insert(10, "-").Insert(6, "-").Insert(3, "-");
            if (ktjId.Length == 18) return ktjId.Insert(14, "-").Insert(10, "-").Insert(6, "-").Insert(3, "-");
            return ktjId;
        }



    }
}
