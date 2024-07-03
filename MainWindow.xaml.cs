using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using NiceLabel.SDK;
using Newtonsoft.Json.Linq;

namespace AutoGenLabel
{
    public partial class MainWindow : Window
    {
        /* Private Fields and Object Declarations */
        #region
        ArenaAPI arenaAPI;
        Connector connector;
        LoftwareAPI loftwareAPI;
        private string destinationPath = "N:\\MechanicalEngineering\\Packaging\\NiceLabel\\PDFLabels";
        private string unitLabelPath = "";
        private string unitLBLSize = "";
        private string intrmdtLabelPath = "";
        private string intrmdtLBLSize = "";
        private string masterLabelPath = "";
        private string masterLBLSize = "";
        #endregion
        public MainWindow()
        {
            this.InitializeComponent();

            arenaAPI = new ArenaAPI();
            connector = new Connector();
            loftwareAPI = new LoftwareAPI();

            PopulateUnitComboBox();
            PopulateShipComboBox();
            PopulateIntermediateComboBox();
        }
        public async void GetLabelDetails()
        {
            string itemID = txt_partNumber.Text;
            var itemGUID = await arenaAPI.GetGUID(itemID);
            var itemAddAtrr = await arenaAPI.GetLabelInfo(itemGUID, connector);
        }
        private void Cmb_unitSize_SelectionChanged(object sender, SelectionChangedEventArgs selectEv)
        {
            try
            {
                unitLBLSize = "";
                if (Cmb_unitSize.SelectedItem != null)
                {
                    string selectedItem = Cmb_unitSize.SelectedItem.ToString();
                    unitLBLSize = selectedItem;
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void PopulateUnitComboBox()
        {
            List<string> items = new List<string>
            {
                "N/A",
                "Unit 1.50\" x 1.00\"",
                "Unit 2.00\" x 1.00\"",
                "Unit 2.00\" x 2.00\"",
                "Unit 2.25\" x 2.00\"",
                "Unit 2.68\" x 2.00\"",
                "Unit 4.00\" x 1.33\"",
                "Unit 4.00\" x 2.50\"",
                "Unit 5.00\" x 3.50\""
            };
            Cmb_unitSize.ItemsSource = items;
            Cmb_unitSize.SelectedIndex = 0;
        }
        private void Cmb_shipSize_SelectionChanged(object sender, SelectionChangedEventArgs selectEv)
        {

        }
        private void PopulateShipComboBox()
        {
            List<string> items = new List<string>
            {
                "N/A",
                "Ship 5.00\" x 3.50\""
            };
            Cmb_shipSize.ItemsSource = items;
            Cmb_shipSize.SelectedIndex = 0;
        }
        private void Cmb_intermediateSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PopulateIntermediateComboBox()
        {
            List<string> items = new List<string>
            {
                "N/A",
                "Inr 2.68\" x 2.00\"",
                "Inr 2.75\" x 2.75\"",
                "Inr 4.00\" x 2.50\"",
                "Inr 4.00\" x 3.00\"",
                "Inr 5.00\" x 3.50\""
            };
            Cmb_intermediateSize.ItemsSource = items;
            Cmb_intermediateSize.SelectedIndex = 0;
        }
        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            GetLabelDetails();
            loftwareAPI.GetLabel(unitLabelPath); // setup path to label template
            loftwareAPI.PrintLabels(connector, destinationPath);
        }
        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            await arenaAPI.LogoutAsync();
            loftwareAPI.ShutDownPrintEngine();
        }
    }
}
