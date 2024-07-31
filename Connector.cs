using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using NiceLabel.SDK;
using System.Windows;

namespace AutoGenLabel
{
    public class Connector
    {
        /* GUIDs from BSC Workspace */
        public string UnitUPC { get; set; }
        private string GUID_unitUPC = "2K4NSI87KCTBUDSVSQK0";
        public string ShipUPC { get; set; }
        private string GUID_shipUPC = "4M6PUKA9MEVDWFUXUSM6";
        public string IntermediateUPC { get; set; }
        private string GUID_intUPC = "0I2LQG65IAR9SBQTQOIA";
        public string BusinessUnit { get; set; }
        private string GUID_businessunit = "CUEX2SIHUM3L4N253I96";
        public string CountryOfOrigin { get; set; }
        private string GUID_COO = "P7RAF5VU7ZGYH0FIHBA3";
        public string IntermediateQuantity {  get; set; }
        private string GUID_intQuantity = "3L5OTJ98LDUCVETWTRM6";
        public string MasterCartonQty { get; set; }
        private string GUID_masterqty = "CUEX2SIHUM3L4N2520VA";
        public string CatalogDescriptionENG1 { get; set; }
        private string GUID_catENG1 = "R9TCH7XW91I0J2HKFF40";
        public string CatalogDescriptionENG2 { get; set; }
        private string GUID_catENG2 = "UCWFKA0ZC4L3M5KNII7Y";
        public string CatalogDescriptionSPN1 {  get; set; }
        private string GUID_catSPN1 = "TBVEJ9ZYB3K2L4JMHH6W";
        public string CatalogDescriptionSPN2 {  get; set; }
        private string GUID_catSPN2 = "WEYHMC21E6N5O7MPKK9T";
        public string CatalogDescriptionFRN1 { get; set; }
        private string GUID_catFRN1 = "SAUDI8YXA2J1K3ILGG5B";
        public string CatalogDescriptionFRN2 { get; set; }
        private string GUID_catFRN2 = "VDXGLB10D5M4N6LOJJ8T";
        public string ColorEnglish { get; set; }
        private string GUID_colorENG = "O6Q9E4UT6YFXGZEHCC1E";
        public string ColorSpanish {  get; set; }
        private string GUID_colorSPN = "Q8SBG6WV80HZI1GJEE36";
        public string ColorFrench {  get; set; }
        private string GUID_colorFRN = "P7RAF5VU7ZGYH0FIDD2V";
        public string CatalogNumber { get; set; }
        private string GUID_catNumber = "N5P8D3TS5XEWFYDGF988";
        public string MFGLocation {  get; set; }
        private string GUID_mfgloc = "GYI16WMLYQ7P8R68PI7B";
        public Connector() { }
        public void ProcessItemInfo(JObject parsedResponse)
        {
            try
            {
                var guidMapping = new Dictionary<string, Action<string>>()
                {
                    { GUID_unitUPC, value => UnitUPC = value},
                    { GUID_shipUPC, value => ShipUPC = value},
                    { GUID_intUPC, value => IntermediateUPC = value},
                    { GUID_businessunit, value => BusinessUnit = value},
                    { GUID_COO, value => CountryOfOrigin = value},
                    { GUID_intQuantity, value => IntermediateQuantity = value},
                    { GUID_masterqty, value => MasterCartonQty = value},
                    { GUID_catENG1, value => CatalogDescriptionENG1 = value},
                    { GUID_catENG2, value => CatalogDescriptionENG2 = value},
                    { GUID_catSPN1, value => CatalogDescriptionSPN1 = value},
                    { GUID_catSPN2, value => CatalogDescriptionSPN2 = value},
                    { GUID_catFRN1, value => CatalogDescriptionFRN1 = value},
                    { GUID_catFRN2, value => CatalogDescriptionFRN2 = value},
                    { GUID_colorENG, value => ColorEnglish = value},
                    { GUID_colorSPN, value => ColorSpanish = value},
                    { GUID_colorFRN, value => ColorFrench = value},
                    { GUID_catNumber, value => CatalogNumber = value},
                    { GUID_mfgloc, value => MFGLocation = value}
                };
                var additionalAttributes = parsedResponse["additionalAttributes"] as JArray;

                foreach (var attributes in additionalAttributes)
                {
                    var guid = attributes["guid"]?.ToString();
                    var value = attributes["value"].ToString();

                    if (guidMapping.TryGetValue(guid, out var action))
                    {
                        action(value);
                    }
                }
                if (CatalogNumber == "")
                    CatalogNumber = parsedResponse["number"]?.ToString();
                else if (CatalogDescriptionENG1 == "" || CatalogDescriptionENG1 == null)
                    CatalogDescriptionENG1 = parsedResponse["name"]?.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }

            return;
        }
        public ILabel SetLabelVariables(ILabel item_label, string itemPN)
        {
            EvaluateAttributes(item_label.Variables[0].SetValue, CatalogNumber, "");
            EvaluateAttributes(item_label.Variables[1].SetValue, CatalogDescriptionENG1, "");
            EvaluateAttributes(item_label.Variables[2].SetValue, CatalogDescriptionENG2, "");
            EvaluateAttributes(item_label.Variables[3].SetValue, CatalogDescriptionSPN1, "");
            EvaluateAttributes(item_label.Variables[4].SetValue, CatalogDescriptionSPN2, "");
            EvaluateAttributes(item_label.Variables[5].SetValue, CatalogDescriptionFRN1, "");
            EvaluateAttributes(item_label.Variables[6].SetValue, CatalogDescriptionFRN2, "");
            EvaluateAttributes(item_label.Variables[7].SetValue, ColorEnglish, "");
            EvaluateAttributes(item_label.Variables[8].SetValue, ColorSpanish, "");
            EvaluateAttributes(item_label.Variables[9].SetValue, ColorFrench, "");
            EvaluateAttributes(item_label.Variables[10].SetValue, UnitUPC, "");
            EvaluateAttributes(item_label.Variables[11].SetValue, IntermediateUPC, "");
            EvaluateAttributes(item_label.Variables[12].SetValue, IntermediateQuantity, "");
            EvaluateAttributes(item_label.Variables[13].SetValue, ShipUPC, "");
            EvaluateAttributes(item_label.Variables[14].SetValue, MasterCartonQty, "");
            EvaluateAttributes(item_label.Variables[15].SetValue, BusinessUnit, "");
            EvaluateAttributes(item_label.Variables[16].SetValue, CountryOfOrigin, "");
            item_label.Variables[17].SetValue(itemPN);
            item_label.Variables[18].SetValue("");
            MFGLocation = MFGLocation == "Creation" ? "Vaughn ON" : "Carlsbad, CA";
            EvaluateAttributes(item_label.Variables[19].SetValue, MFGLocation, "");
            return item_label;
        }
        private void EvaluateAttributes<T>(Action<T> setter, T value, T defaultValue)
        {
            setter(value != null ? value : defaultValue);
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
            // countryOfOrigin = 16
            // labelItemPN = 17
            // imgSource = 18
            // mfgLocation = 19
        #endregion
        ~Connector() { }
    }
}
