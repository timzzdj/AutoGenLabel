using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using NiceLabel.SDK;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AutoGenLabel
{
    public partial class MainWindow : Window
    {
        /* Private Fields and Object Declarations */
        #region
        ArenaAPI arenaAPI;
        Connector connector;
        LoftwareAPI loftwareAPI;
        private bool is_requestFinished = false;
        private string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\PDFs");
        private string unitLabelPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Labels");
        private string intrmdtLabelPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Labels");
        private string masterLabelPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Labels");
        private string unitLBLSize;
        private string intrmdtLBLSize;
        private string masterLBLSize;
        #endregion
        public MainWindow()
        {
            this.InitializeComponent();

            arenaAPI = new ArenaAPI();
            LoginArena();
            connector = new Connector();
            loftwareAPI = new LoftwareAPI();

            PopulateUnitComboBox();
            PopulateShipComboBox();
            PopulateIntermediateComboBox();
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
            catch (Exception ex)
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
                "Ship 5.00\" x 3.50\"",
                "Ship 2.68\" x 2.00\""
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
        private async void LoginArena()
        {
            var is_loginSuccessful = false;
            try
            {
                is_loginSuccessful = await arenaAPI.LoginAsync();
                Thread.Sleep(3000);
                if (!is_loginSuccessful)
                {
                    MessageBox.Show("Please See Credential File if your input is correct.", "Login to Arena failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
            is_requestFinished = is_loginSuccessful;
        }
        public async Task<string> GetGUID()
        {
            var itemGUID = "";
            try
            {
                string itemID = txt_partNumber.Text;
                itemGUID = await arenaAPI.GetGUID(itemID);
                await Task.Delay(3000);
                return itemGUID;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
            return itemGUID;
        }
        public async void GetLabelDetails(string GUID)
        {
            var itemAddAttr = "";
            try
            {
                itemAddAttr = await arenaAPI.GetLabelInfo(GUID, connector);
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
            return;
        }
        public async void GetLabelTemplate()
        {
            try
            {
                loftwareAPI.GetLabel(unitLabelPath); // setup path to label template
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
            return;
        }
        public async void PrintLabel()
        {
            try
            {
                loftwareAPI.PrintLabels(connector, destinationPath);
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        private async void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var itemGuid = await GetGUID();
                await Task.Delay(3000);
                GetLabelDetails(itemGuid);
                await Task.Delay(3000);
                GetLabelTemplate();
                await Task.Delay(3000);
                PrintLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            await arenaAPI.LogoutAsync();
            loftwareAPI.ShutDownPrintEngine();
        }
    }
}
