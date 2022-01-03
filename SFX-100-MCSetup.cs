using SimFeedback.extension;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;
using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Text;
/* 10.10.2021 */
using System.IO;
using System.Xml.Linq;

namespace SFX100MCSetup
{
    /*
    public static class WinApi
    {
        /// <summary>TimeBeginPeriod(). See the Windows API documentation for details.</summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]

        public static extern uint TimeBeginPeriod(uint uMilliseconds);

        /// <summary>TimeEndPeriod(). See the Windows API documentation for details.</summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]

        public static extern uint TimeEndPeriod(uint uMilliseconds);
    }
    */

    public class FileOps
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public string path;
        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <param name="INIPath"></param>
        public FileOps(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <param name="Section"></param>
        /// Section name
        /// <param name="Key"></param>
        /// Key Name
        /// <param name="Value"></param>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }


    public class MCSetupExtension : AbstractSimFeedbackExtension
    {
        private SFBMCSetupControl _extCtrl;

        /// <summary>
        /// Programm-Desktopposition
        /// </summary>
        //public int LoadedProgramLastPositionX = -1;
        //public int LoadedProgramLastPositionY = -1;
        /// <summary>
        /// Extension's settings directory structure
        /// </summary>
        public static string rootPath { get; set; } = "extensions\\SFXMCSetupExtension";
        public static string portFile { get; set; } = rootPath + "\\" + "settings.ini";
        /// <summary>
        /// Extension's setting file
        /// </summary>
        public FileOps ExtensionSettingsFile = new FileOps(Application.StartupPath + "\\" + portFile);

        /// <summary>
        /// ID-Text in Label "ID"
        /// </summary>
        public string[] cIDStg = new string[] { "ID :" };

        /// <summary>
        /// String zur Idenfikation der COM-Ports (Vorangestellter Text bei Suche ueber .Net-Code verfuegbarer Ports
        /// </summary>
        public string[] cCOMStg = new string[] { "COM" };

        /// <summary>
        /// Parallel-Thread which checks for SFB main application to be completely started & ready for position change
        /// </summary>
        //public Thread DelayPosChangeThread = null;

        /// <summary>
        /// user-defined program position read from settings
        /// </summary>
        //public bool UserPositionFound = false;
        /// <summary>
        /// position update-State
        /// </summary>
        //public bool UserPositionSet = false;

        public struct MultiControllerInfo
        {
            public string controllerId;
            public string controllerType;
            public string controllerPort;
        }
        /// <summary>
        /// contains a record of information for each controller found in SFB configuration
        /// </summary>
        public MultiControllerInfo[] multiControllerInfo;

        /// <summary>
        /// if successfully accessing SFB configuration this var contains full path to config file
        /// </summary>
        public string SFBConfigPfad = string.Empty;

        /// <summary>
        /// shows actual amount of configured motion controllers in SFB
        /// </summary>
        public int CurrentMCCount = 0;

        /// <summary>
        /// if true => refresh of information on dialogue + logfile needed
        /// </summary>
        public bool MCCountRefresh = true;

        /// <summary>
        /// complete content of SFB XML configuration read at startup in advance of full running SFB + extension
        /// </summary>
        public string SFBXMLConfig = string.Empty;

        public MCSetupExtension()
        {
            Name = "SFX-100 Multicontroller-Setup";
            Info = "help configuring multiple controller";
            Version = Assembly.LoadFrom(Assembly.GetExecutingAssembly().Location).GetName().Version.ToString();
            Author = "Topy190675";
            HasControl = true /* false */;
        }
        /*
        public void SetIsRunning(bool status)
        {
            IsRunning = status;
        }
        
        private bool StartupDelayForMainApplForm()
        {
            const int millisToWait = 250;
            const int runCounts = 10;
            int i = 0;
            bool retVal = false;

            while ((i < runCounts) && (!retVal))
            {
                Thread.Sleep(millisToWait);
                i++;

                if (Form.ActiveForm != null)
                {
                    retVal = true; // = active form of main appl found
                }
                else
                    Log(Name + ": search for form - try " + Convert.ToString(i));
            }
            return retVal;
        }
        */
        public bool OriginalConfigRead()
        {
            bool RetVal = false;
            
            /* 10.10.2021 : Code taken from SFX-100-StreamDeck / thx @ daCujo for publishing */
            string pathSFbXml = Path.Combine(Directory.GetCurrentDirectory(), "SimFeedback.xml");
            Log(Name + ": " + pathSFbXml);

            try
            {
                // success : speichere den Pfad zur SFB-Config in Variable 
                SFBConfigPfad = pathSFbXml;

                RetVal = true;

                // Verfügbare Controller laden
                SFBXMLConfig = File.ReadAllText(pathSFbXml);

                var xmlControllerStartStringId = "<MotionControllerList>";
                var xmlControllerEndStringId = "</MotionControllerList>";
                int pFrom = SFBXMLConfig.IndexOf(xmlControllerStartStringId);
                int pTo = SFBXMLConfig.LastIndexOf(xmlControllerEndStringId) + xmlControllerEndStringId.Length;

                String result = SFBXMLConfig.Substring(pFrom, pTo - pFrom);
                /* old code - commented already @ daCujo */
                //GuiLoggerProvider.Instance.Log(result);
                /* Ende old code - commented already @ daCujo */
                var doc = XDocument.Parse(result);
                /* 10.10.2021 @ TPHW */
                //SimFeedbackInvoker.Instance.actionElements.ControllerIndexAssignment.Clear();
                /* Ende 10.10.2021  @ TPHW */

                CurrentMCCount = doc.Root.Elements().Count();
                multiControllerInfo = new MultiControllerInfo[5];
                int Counter = 0;
                foreach (var ctrlElem in doc.Root.Elements())
                {
                    /*
                    var ctrlName = ctrlElem.Element("type").Value;                    
                    var ctrlPort = ctrlElem.Element("comPort").Value;
                    var ctrlId = ctrlElem.Element("id").Value;
                    */
                    multiControllerInfo[Counter].controllerType = ctrlElem.Element("type").Value;
                    multiControllerInfo[Counter].controllerId = ctrlElem.Element("id").Value;
                    multiControllerInfo[Counter].controllerPort = ctrlElem.Element("comPort").Value;
                    Counter++;

                    /* old code - commented already @ daCujo */
                    //GuiLoggerProvider.Instance.Log("XML - Controller-Type found: " + controllerName);

                    //SimFeedbackInvoker.Instance.actionElements.Controllers.Add(controllerName, null);
                    /* Ende old - code - commented already @ dacCujo */
                    /* 10.10.2021 @ TPHW */
                    /*
                    SimFeedbackInvoker.Instance.actionElements.ControllerIndexAssignment.Add(controllerName);
                    */
                    /* Ende 10.10.2021 @ TPHW */
                }
            }
            catch (Exception ex)
            {
                /* 10.10.2021 @ TPHW */
                /*
                GuiLoggerProvider.Instance.Log("Error during Reading of: " + xmlFile + "Error:" + ex.Message);
                */
                /* Ende 10.10.2021 @ TPHW */
                Log(Name + ": " + Convert.ToString(pathSFbXml) + " " + ex.Message);
            }
            /* Ende 10.10.2021 */
            /*
            string tmpStg = ExtensionSettingsFile.IniReadValue("main", "ProgPos");
            if (!String.IsNullOrEmpty(tmpStg))
            {
                string[] Values = tmpStg.Split(new char[] { '/' }, StringSplitOptions.None);
                int len = Values.GetLength(0);
                if (len == 2)
                {
                    LoadedProgramLastPositionX = Convert.ToInt16(Values[0]);
                    LoadedProgramLastPositionY = Convert.ToInt16(Values[1]);

                    RetVal = true;
                }
            }
            */
            return RetVal;
        }
        /*
        public void CheckForMainApplFormLoaded()
        {
            const int millisToWait = 250;
            const int runCounts = 20; // 10 bis 03.05.2021
            int i = 0;
            bool ActiveFormFound = false;

            while ((i < runCounts) && (!ActiveFormFound))
            {
                Thread.Sleep(millisToWait);
                i++;
                LogDebug(Name + ": search for form - try " + Convert.ToString(i));
                if (Form.ActiveForm != null)
                {
                    ActiveFormFound = true; // = active form of main appl found
                }
            }

            if (ActiveFormFound)
            {
                PositionUpdate();
            }
            else
                Log(Name + ": active form cannot be access ! no position change possible !");

        }

        private void PositionUpdate()
        {
            if (Form.ActiveForm != null)
            {
                LogDebug(Name + ": change StartPos to manual");
                Form.ActiveForm.StartPosition = FormStartPosition.Manual;
                if (Form.ActiveForm.WindowState != FormWindowState.Minimized)
                {
                    Form.ActiveForm.DesktopLocation = new Point(LoadedProgramLastPositionX, LoadedProgramLastPositionY);
                    UserPositionSet = true;
                }
                Log(Name + ": Program-Position updated");
            }
            else
                Log(Name + ": ActiveForm = null");
        }
        */
       
        public override void Init(SimFeedbackExtensionFacade facade, ExtensionConfig config)
        {
            base.Init(facade, config);
            Log(Name + ":Initialize Extension");

            _extCtrl = new SFBMCSetupControl(this, facade);   /* GUI Version @ 24.04.2021 */
        }

        public override void Start()
        {
            if (!IsRunning)
            {
                Log(Name + ": Extension loaded");

                if (OriginalConfigRead())
                {
                    Log(Name + ": Original SFB XML read");                    
                }
                else
                {
                    Log("no SFB config possible to read. Kept everything original / untouched !");
                }
                _extCtrl.Start();   /* GUI Version @ 10.10.2021 */

                IsRunning = true;
            }
        }

        public override void Stop()
        {
            if (IsRunning)
            {
                _extCtrl.Stop();    /* GUI Version @ 10.10.2021 */

                Log(Name + ": stopping");
                //LoadedProgramLastPositionX = Form.ActiveForm.DesktopLocation.X;
                //LoadedProgramLastPositionY = Form.ActiveForm.DesktopLocation.Y;
                ExtensionSettingsFile.IniWriteValue("main", "ProgAppliedChanges", Convert.ToString(1) + "/" + Convert.ToString(2));
                LogDebug(Name + ": automatically loaded SFB config / provided user option to add multi-controller with GUI. Done all needed");
                IsRunning = false;

                Log(Name + ": Extension unloaded");
            }
        }

        public override Control ExtensionControl => _extCtrl;
    }
}
