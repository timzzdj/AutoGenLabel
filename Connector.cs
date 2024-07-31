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
        public string IntermediateUPC { get; set; }
        private string GUID_intUPC = "";
        public string BusinessUnit { get; set; }
        private string GUID_businessunit = "CUEX2SIHUM3L4N253I96";
        public string CountryOfOrigin { get; set; }
        private string GUID_COO = "P7RAF5VU7ZGYH0FIHBA3";
        public string MasterCartonQty { get; set; }
        private string GUID_masterqty = "CUEX2SIHUM3L4N2520VA";
        public string CatalogDescriptionENG1 { get; set; }
        private string GUID_catENG1 = "";
        public string CatalogDescriptionENG2 { get; set; }
        private string GUID_catENG2 = "";
        public string CatalogDescriptionSPN1 {  get; set; }
        private string GUID_catSPN1 = "";
        public string CatalogDescriptionSPN2 {  get; set; }
        private string GUID_catSPN2 = "";
        public string CatalogDescriptionFRN1 { get; set; }
        private string GUID_catFRN1 = "";
        public string CatalogDescriptionFRN2 { get; set; }
        private string GUID_catFRN2 = "";

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

                if (guid == GUID_unitUPC)
                    UnitUPC = value;
                else if (guid == GUID_shipUPC)
                    ShipUPC = value;
                else if (guid == GUID_businessunit)
                    BusinessUnit = value;
                else if (guid == GUID_COO)
                    CountryOfOrigin = value;
                else if (guid == GUID_masterqty)
                    MasterCartonQty = value;
            }

            CatalogNumber = parsedResponse["number"]?.ToString();
            CatalogDescription = parsedResponse["name"]?.ToString();

            return;
        }
        public ILabel SetLabelVariables(ILabel item_label)
        {
            item_label.Variables[0].SetValue(CatalogNumber);
            item_label.Variables[1].SetValue(CatalogDescription);
            item_label.Variables["COO"].SetValue(CountryOfOrigin);
            //item_label.Variables["unitLabelUPC"].SetValue(UnitUPC);
            item_label.Variables[9].SetValue(ShipUPC);
            item_label.Variables[10].SetValue(MasterCartonQty);

            return item_label;
        }
        #region
            // Indexes
            // catalogNumber = 0
            // descriptionLine1ENG = 1
            // descriptionLine2ENG = 2
            // descriptionLine1SPN = 3
            // descriptionLine2SPN = 4
            // descriptionLine1FRN = 5
            // descriptionLine2FRN = 6
            // colorENG = 7
            // colorSPN = 8
            // colorFRN = 9
            // unitLabelUPC = 10
            // intLabelUPC = 11
            // intQuantity = 12
            // shipLabelUPC = 13
            // shipQuantity = 14
            // businessUnit = 15
            // mfgLocation = 16
            // labelItemPN = 17
            // imgSource = 18
        #endregion
        ~Connector() { }
    }
}
