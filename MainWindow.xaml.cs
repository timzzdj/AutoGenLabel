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
        private string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), @"PDFs\");
        private string unitLabelPath = Path.Combine(Directory.GetCurrentDirectory(), @"Labels\");
        private string intrmdtLabelPath = Path.Combine(Directory.GetCurrentDirectory(), @"Labels\");
        private string masterLabelPath = Path.Combine(Directory.GetCurrentDirectory(), @"Labels\");
        public string unitLBLPN = "LABEL_UNIT_";
        public string intLBLPN = "LABEL_INNER_";
        public string shipLBLPN = "LABEL_SHIP_";
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

        private void Cmb_unitSize_SelectionChanged(object unitSender, SelectionChangedEventArgs selectEv)
        {
            try
            {
                ComboBox comboBox = unitSender as ComboBox;
                switch(comboBox.SelectedIndex)
                {
                    case 0:
                        unitLBLSize = unitLabelPath + "Unit Label Format 4.00 x 2.50.nlbl";
                        break;
                    case 1:
                        unitLBLSize = unitLabelPath + "Unit Label Format 1.50 x 1.00.nlbl";
                        break;
                    case 2:
                        unitLBLSize = unitLabelPath + "Unit Label Format 2.00 x 1.00.nlbl";
                        break;
                    case 3:
                        unitLBLSize = unitLabelPath + "Unit Label Format 2.00 x 2.00.nlbl";
                        break;
                    case 4:
                        unitLBLSize = unitLabelPath + "Unit Label Format 2.25 x 2.00.nlbl";
                        break;
                    case 5:
                        unitLBLSize = unitLabelPath + "Unit Label Format 2.68 x 2.00.nlbl";
                        break;
                    case 6:
                        unitLBLSize = unitLabelPath + "Unit Label Format 4.00 x 1.33.nlbl";
                        break;
                    case 7:
                        unitLBLSize = unitLabelPath + "Unit Label Format 4.00 x 2.50.nlbl";
                        break;
                    case 8:
                        unitLBLSize = unitLabelPath + "Unit Label Format 5.00 x 3.50.nlbl";
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
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
        private void Cmb_shipSize_SelectionChanged(object shipSender, SelectionChangedEventArgs selectEv)
        {
            try
            {
                ComboBox comboBox = shipSender as ComboBox;
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        masterLBLSize = masterLabelPath + "Ship Label Format 5.00 x 3.50.nlbl";
                        break;
                    case 1:
                        masterLBLSize = masterLabelPath + "Ship Label Format 5.00 x 3.50.nlbl";
                        break;
                    case 2:
                        masterLBLSize = masterLabelPath + "Ship Label Format 2.68 x 2.00.nlbl";
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
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
        private void Cmb_intermediateSize_SelectionChanged(object intSender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox comboBox = intSender as ComboBox;
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        intrmdtLBLSize = intrmdtLabelPath + "Inr Label Format 4.00 x 2.50.nlbl";
                        break;
                    case 1:
                        intrmdtLBLSize = intrmdtLabelPath + "Inr Label Format 2.68 x 2.00.nlbl";
                        break;
                    case 2:
                        intrmdtLBLSize = intrmdtLabelPath + "Inr Label Format 4.00 x 2.50.nlbl";
                        break;
                    case 3:
                        intrmdtLBLSize = intrmdtLabelPath + "Inr Label Format 5.00 x 3.50.nlbl";
                        break;
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        private void PopulateIntermediateComboBox()
        {
            List<string> items = new List<string>
            {
                "N/A",
                "Inr 2.68\" x 2.00\"",
                "Inr 4.00\" x 2.50\"",
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
        public async void GetLabelTemplate(string selectedLabel_size)
        {
            try
            {
                loftwareAPI.GetLabel(selectedLabel_size); // setup path to label template
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
            return;
        }
        public async void PrintLabel(string itemPN, string lblPath)
        {
            try
            {
                loftwareAPI.PrintLabels(connector, destinationPath, itemPN, lblPath);
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
                EvaluateLabelSize();
                await Task.Delay(3000);
                PrintUnitLabels();
                await Task.Delay(3000);
                PrintIntermediateLabel();
                await Task.Delay(3000);
                PrintShipLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        private void PrintUnitLabels()
        {
            try
            {
                unitLBLPN = unitLBLPN + txt_itemNumberUnit.Text;
                if (Cmb_unitSize.SelectedIndex != 0 && unitLBLPN != "")
                {
                    GetLabelTemplate(unitLBLSize);
                    PrintLabel(txt_itemNumberUnit.Text, unitLBLPN);
                }
                unitLBLPN = "LABEL_UNIT_";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        private void PrintIntermediateLabel()
        {
            try
            {
                intLBLPN = intLBLPN + txt_itemNumberIntermediate.Text;
                if (Cmb_intermediateSize.SelectedIndex != 0 && intLBLPN != "")
                {
                    GetLabelTemplate(intrmdtLBLSize);
                    PrintLabel(txt_itemNumberIntermediate.Text, intLBLPN);
                }
                intLBLPN = "LABEL_INNER_";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        private  void PrintShipLabel()
        {
            try
            {
                shipLBLPN = shipLBLPN + txt_itemNumberShip.Text;
                if (Cmb_shipSize.SelectedIndex != 0 && shipLBLPN != "")
                {
                    GetLabelTemplate(masterLBLSize);
                    PrintLabel(txt_itemNumberShip.Text, shipLBLPN);
                }
                shipLBLPN = "LABEL_SHIP_";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }
        private void EvaluateLabelSize()
        {
            try
            {
                if((unitLBLPN == "" && intLBLPN == "" && shipLBLPN == "")&&
                   (Cmb_unitSize.SelectedIndex == 0 && Cmb_intermediateSize.SelectedIndex == 0 && Cmb_shipSize.SelectedIndex == 0))
                {
                    MessageBox.Show("Error Message: Label PN / Label Size is missing, \nPlease select a label size for each label PN entered.\nPlease enter a label PN for each label size selected.", "Label Information Issue");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
            return;
        }
        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            loftwareAPI.ShutDownPrintEngine();
            await arenaAPI.LogoutAsync();
        }
    }
}
