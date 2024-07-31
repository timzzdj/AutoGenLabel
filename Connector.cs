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
                    { GUID_catNumber, value => CatalogNumber = value}
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
                if (CatalogNumber.Equals(string.Empty))
                    CatalogNumber = parsedResponse["number"]?.ToString();
                else if (CatalogDescriptionENG1.Equals(string.Empty))
                    CatalogDescriptionENG1 = parsedResponse["name"]?.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }

            return;
        }
        public ILabel SetLabelVariables(ILabel item_label)
        {
            item_label.Variables[0].SetValue(CatalogNumber);
            item_label.Variables[1].SetValue(CatalogDescriptionENG1);
            item_label.Variables[2].SetValue(CatalogDescriptionENG2);
            item_label.Variables[3].SetValue(CatalogDescriptionSPN1);
            item_label.Variables[4].SetValue(CatalogDescriptionSPN2);
            item_label.Variables[5].SetValue(CatalogDescriptionFRN1);
            item_label.Variables[6].SetValue(CatalogDescriptionFRN2);
            item_label.Variables[7].SetValue(ColorEnglish);
            item_label.Variables[8].SetValue(ColorSpanish);
            item_label.Variables[9].SetValue(ColorFrench);
            /*
            item_label.Variables[10].SetValue(UnitUPC);
            item_label.Variables[11].SetValue(IntermediateUPC);
            item_label.Variables[12].SetValue();
            item_label.Variables[13].SetValue();
            item_label.Variables[14].SetValue();
            item_label.Variables[15].SetValue();
            item_label.Variables[16].SetValue(CountryOfOrigin);
            item_label.Variables[17].SetValue();
            item_label.Variables[18].SetValue();
            */
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
