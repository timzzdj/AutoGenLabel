using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NiceLabel.SDK;

namespace AutoGenLabel
{
    public class LoftwareAPI
    {
        private IPrintEngine print_engine = PrintEngineFactory.PrintEngine;
        private ILabel new_label;
        public LoftwareAPI()
        {
            // Start Initialization of SDK Print Engine
            InitializePrintEngine();
        }
        public void InitializePrintEngine()
        {
            // Get SDK File Path
            string sdkFilesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\SDKFiles");
            // Checks if SDK file path exists
            if (Directory.Exists(sdkFilesPath))
                PrintEngineFactory.SDKFilesPath = sdkFilesPath;
            // SDK print engine initialization
            try
            {
                print_engine.Initialize();
            }
            catch (SDKException sdkEx)
            {
                MessageBox.Show($"{sdkEx.ErrorCode}: {sdkEx.Message}\n{sdkEx.DebugMessage}", $"SDK Exception: {sdkEx.DetailedErrorCode}");
            }
        }
        public ILabel GetLabel(string label_template)
        {
            new_label = print_engine.OpenLabel(label_template);
            return new_label;
        }
        public void PrintLabels(Connector connector, string destination_path)
        {
            new_label = connector.SetLabelVariables(new_label);

            IPrintSettings print_settings = new_label.PrintSettings;
            destination_path = "C:\\Users\\dejesust\\source\\repos\\AutoGenLabel\\";

            print_settings.PrinterName = "Microsoft Print to PDF";
            print_settings.PrintToPdf = true;
            print_settings.OutputFileName = $"{destination_path}{Path.GetFileNameWithoutExtension(new_label.FileName)}.pdf";

            new_label.Print(1);
        }
        public void OpenTraceFile()
        {
            // Get application's path
            string appPath = Assembly.GetExecutingAssembly().Location;

            // Get just application's directory from the path;
            string appDir = Path.GetDirectoryName(appPath);

            // create trace file path
            string traceFile = appDir + "\\PrintEngineTrace.txt";

            // Set a value for the TraceFile property to enable tracing.
            print_engine.TraceFile = traceFile;

            // If trace file was created, open it with default .txt editor.
            if (File.Exists(traceFile))
                System.Diagnostics.Process.Start(traceFile);
        }
        public void ShutDownPrintEngine()
        {
            print_engine.Shutdown();
        }
        ~LoftwareAPI() { }
    }
}
