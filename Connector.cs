using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using NiceLabel.SDK;

namespace AutoGenLabel
{
    public class Connector
    {
        /* GUID from BSC Workspace */
        public string UnitUPC { get; set; }
        private string GUID_unitUPC = "2K4NSI87KCTBUDSVSQK0";
        public string ShipUPC { get; set; }
        private string GUID_shipUPC = "4M6PUKA9MEVDWFUXUSM6";
        public string BusinessUnit { get; set; }
        private string GUID_businessunit = "CUEX2SIHUM3L4N253I96";
        public string CountryOfOrigin { get; set; }
        private string GUID_COO = "P7RAF5VU7ZGYH0FIHBA3";
        public string MasterCartonQty { get; set; }
        private string GUID_masterqty = "CUEX2SIHUM3L4N2520VA";
        public string CatalogNumber { get; set; }
        public string CatalogDescription { get; set; }
        public Connector() { }
        public void ProcessItemInfo(JObject parsedResponse)
        {
            var additionalAttributes = parsedResponse["additionalAttributes"] as JArray;

            foreach (var attributes in additionalAttributes)
            {
                var guid = attributes["guid"]?.ToString();
                var value = attributes["value"].ToString();

                if(guid == GUID_unitUPC)
                    UnitUPC = value;
                else if(guid == GUID_shipUPC)
                    ShipUPC = value;
                else if(guid == GUID_businessunit)
                    BusinessUnit = value;
                else if(guid == GUID_COO)
                    CountryOfOrigin = value;
                else if(guid == GUID_masterqty) 
                    MasterCartonQty = value;
            }

            CatalogNumber = parsedResponse["number"]?.ToString();
            CatalogDescription = parsedResponse["name"]?.ToString();

            return;
        }
        public ILabel SetLabelVariables(ILabel item_label)
        {
            item_label.Variables["Catalog Number"].SetValue(CatalogNumber);
            item_label.Variables["Description"].SetValue(CatalogDescription);
            item_label.Variables["COO"].SetValue(CountryOfOrigin);
            item_label.Variables["Unit UPC"].SetValue(UnitUPC);
            item_label.Variables["Master UPC"].SetValue(ShipUPC);

            return item_label;
        }
        ~Connector() { }
    }
}
