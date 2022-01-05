using System;
using System.Diagnostics;
using System.Windows.Forms;
using SimFeedback.extension;
/* 10.10.2021 */
using System.IO.Ports;

namespace SFX100MCSetup
{
    public partial class SFBMCSetupControl : UserControl
    {
        MCSetupExtension tfExt;
        SimFeedbackExtensionFacade sfbFacade;

        //bool PositionChangeLogged = false;

        /// <summary>
        /// Array with available COM-Ports in used PC
        /// </summary>
        string[] AvailableComPorts;

        /// <summary>
        /// Mode parameter to decide if add or edit on multi-controller setting should be performed
        /// </summary>
        bool EditModeActive = false;
        /// <summary>
        /// Mode parameter to decide if during activated edit on multi-controller setting delete operation should be performed
        /// </summary>
        bool DeleteModeActive = false;

        public SFBMCSetupControl(MCSetupExtension mcSetupExtension, SimFeedback.extension.SimFeedbackExtensionFacade facade)
        {
            tfExt = mcSetupExtension;
            sfbFacade = facade;
            InitializeComponent();
        }

        internal void Log(string message)
        {
            if (logBox.Items.Count >= 500)
            {
                logBox.Items.RemoveAt(0);
            }

            logBox.Items.Add(/* DateTime.Now + ": " + */message); /* 12.10.2021 : removed day-time info for log output */
            logBox.SelectedIndex = logBox.Items.Count - 1;

            int visibleItems = logBox.ClientSize.Height / logBox.ItemHeight;
            logBox.TopIndex = Math.Max(logBox.Items.Count - visibleItems + 1, 0);

            sfbFacade.Log(message);
        }

        internal void DrawError()
        {
            //pictureBoxCurrentStatus.BackColor = Color.Red;
        }

        internal void DrawSuccess()
        {
            //pictureBoxCurrentStatus.BackColor = Color.Green;
        }

        /* 12.10.2021 */
        private int Add_Update_SFBConfig(int ControllerIdToAdd, bool Update, /* 04.01.2022 */ bool Delete)
        {
            int RetVal = 0;
            try
            {
                // Verfügbare Controller laden
                // string xmlContentsSfbXml = File.ReadAllText(SFBConfigPfad); /* 12.10.2021 */

                var xmlControllerStartStringId = "<MotionControllerConfigData>"; /* "<MotionControllerList>" */
                var xmlControllerEndStringId = "</MotionControllerConfigData>"; /* "</MotionControllerList>" */
                /* 22.11.2021 */
                int pFrom = 0;
                // int pLastFrom = 0;
                int pTo = 0;
                string result = String.Empty;

                /* 04.01.2022 */

                if (Update)
                {
                    bool ControllerFound = false;
                    /* neu 24.11.2021 */
                    int pUpdateFrom = 0;
                    int pUpdateTo = 0;
                    /* Ende neu 24.11.2021 */

                    /* complete MotionControllerConfigData */
                    pFrom = tfExt.SFBXMLConfig.IndexOf(xmlControllerStartStringId);
                    //pLastFrom = tfExt.SFBXMLConfig.LastIndexOf(xmlControllerStartStringId);
                    pTo = tfExt.SFBXMLConfig.LastIndexOf(xmlControllerEndStringId) + xmlControllerEndStringId.Length;
                    try
                    { 
                        result = tfExt.SFBXMLConfig.Substring(pFrom, pTo - pFrom);
                    }
                    catch (Exception ex1)
                    {
                        Log(Name + ": cannot access MotionControllerList ! " + ex1.Message);
                        return RetVal;
                    }

                    string PartResult = result;
                    do
                    {
                        int ControllerIdToEdit = ControllerIdToAdd + 1;
                        // XML-data for ID 
                        var xmlIDStart = "<id>";
                        var xmlIDEnd = "</id>";

                        int IndexIDStart = PartResult.LastIndexOf(xmlIDStart); // controllerParameter.
                        int IndexIDEnd = PartResult.LastIndexOf(xmlIDEnd); // controllerParameter

                        string ControllerID = String.Empty;
                        try
                        {
                            ControllerID = PartResult.Substring(IndexIDStart + xmlIDStart.Length, IndexIDEnd - IndexIDStart - xmlIDStart.Length); // controllerParameter.
                        }
                        catch (Exception ex4)
                        {
                            /* 10.10.2021 @ TPHW */
                            /*
                            GuiLoggerProvider.Instance.Log("Error during Reading of: " + xmlFile + "Error:" + ex.Message);
                            */
                            /* Ende 10.10.2021 @ TPHW */
                            Log(Name + ": cannot access from PartResult ! " + ex4.Message);
                        }

                        if (String.Compare(ControllerID, Convert.ToString(ControllerIdToEdit)) == 0)
                        {
                            ControllerFound = true;
                            pUpdateFrom = PartResult.LastIndexOf(xmlControllerStartStringId);
                            pUpdateTo = PartResult.LastIndexOf(xmlControllerEndStringId) + xmlControllerEndStringId.Length;
                            result = PartResult.Substring(pUpdateFrom, pUpdateTo - pUpdateFrom);
                            /* 24.11.2021 */
                            /*
                            pFrom = tfExt.SFBXMLConfig.IndexOf(xmlControllerStartStringId);                            
                            pTo = tfExt.SFBXMLConfig.IndexOf(xmlControllerEndStringId) + xmlControllerEndStringId.Length;
                            */
                            /* 04.01.2022 */
                            if (Delete)
                            {
                                pFrom = pUpdateFrom;
                                pTo = pUpdateTo;
                            }
                        }
                        else
                        {
                            pUpdateFrom = PartResult.IndexOf(xmlControllerStartStringId);
                            pUpdateTo = PartResult.LastIndexOf(xmlControllerStartStringId);
                            
                            PartResult = PartResult.Substring(pUpdateFrom, pUpdateTo /* - 1 */ - pUpdateFrom);
                        }
                    }
                    while ((!ControllerFound) & (PartResult.Length > 0));
                }
                else
                { 
                    pFrom = tfExt.SFBXMLConfig.LastIndexOf(xmlControllerStartStringId);
                    pTo = tfExt.SFBXMLConfig.LastIndexOf(xmlControllerEndStringId) + xmlControllerEndStringId.Length;
                    
                    try
                    {
                        result = tfExt.SFBXMLConfig.Substring(pFrom, pTo - pFrom);
                    }
                    catch (Exception ex1)
                    {
                        Log(Name + ": cannot access MotionControllerList ! " + ex1.Message);
                    }
                }

                /* old code - commented already @ daCujo */
                //GuiLoggerProvider.Instance.Log(result);
                /* Ende old code - commented already @ daCujo */
                if (!String.IsNullOrEmpty(result))
                {
                    var doc = System.Xml.Linq.XDocument.Parse(result);
                    /* 10.10.2021 @ TPHW */
                    //SimFeedbackInvoker.Instance.actionElements.ControllerIndexAssignment.Clear();
                    /* Ende 10.10.2021  @ TPHW */

                    /* 12.10.2021 : copy of original MotionControllerList (single entry in case of already several controllers added */
                    /*
                    if (CurrentMCCount > 0)
                    {
                        XmlNode NodeToCopy = doc.SelectSingleNode("Servers/" + ServerToCopy);
                    }
                    */
                    String NewResult = result;
                    /*
                    var xmlMotorListStartStringId = "<MotorList>";
                    var xmlMotorListEndStringId = "/MotorList>";

                    int pFromMotorList = result.IndexOf(xmlMotorListStartStringId);
                    int pToMotorList = result.IndexOf(xmlMotorListEndStringId) + xmlMotorListEndStringId.Length;

                    // XML-Data for existing controller's motorlist 
                    string MotorList = result.Substring(pFromMotorList, pToMotorList - pFromMotorList);

                    try
                    {
                        string controllerParameter = result.Remove(pFromMotorList, pToMotorList - pFromMotorList);
                        // Log(Name + ": " + MotorList);
                    }
                    catch (Exception ex2)
                    {
                        Log(Name + ": cannot access + remove MotorList from result ! " + ex2.Message);
                    }            
                    */
                    // XML-data for ID 
                    var xmlIDStart = "<id>";
                    var xmlIDEnd = "</id>";

                    int IndexIDStart = NewResult.LastIndexOf(xmlIDStart); // controllerParameter.
                    int IndexIDEnd = NewResult.LastIndexOf(xmlIDEnd); // controllerParameter

                    string ControllerID = String.Empty;
                    try
                    {
                        ControllerID = NewResult.Substring(IndexIDStart + xmlIDStart.Length, IndexIDEnd - IndexIDStart - xmlIDStart.Length); // controllerParameter.
                    }
                    catch (Exception ex4)
                    {
                        /* 10.10.2021 @ TPHW */
                        /*
                        GuiLoggerProvider.Instance.Log("Error during Reading of: " + xmlFile + "Error:" + ex.Message);
                        */
                        /* Ende 10.10.2021 @ TPHW */
                        Log(Name + ": cannot access from NewResult ! " + ex4.Message);
                    }
                    int pFromControllerID = NewResult.LastIndexOf(xmlIDStart);
                    int pToControllerID = NewResult.LastIndexOf(xmlIDEnd) + xmlIDEnd.Length;

                    string ControllerIDXMLStg = string.Empty;
                    string NewControllerIDXMLStg = string.Empty;
                    try
                    {
                        ControllerIDXMLStg = NewResult.Substring(pFromControllerID, pToControllerID - pFromControllerID /* + xmlIDEnd.Length */);
                        NewControllerIDXMLStg = ControllerIDXMLStg.Replace(ControllerID, tfExt.multiControllerInfo[ControllerIdToAdd].controllerId);
                        NewResult = NewResult.Replace(ControllerIDXMLStg, NewControllerIDXMLStg);
                    }
                    catch (Exception ex5)
                    {
                        /* 10.10.2021 @ TPHW */
                        /*
                        GuiLoggerProvider.Instance.Log("Error during Reading of: " + xmlFile + "Error:" + ex.Message);
                        */
                        /* Ende 10.10.2021 @ TPHW */
                        Log(Name + ": cannot change / replace controller-id string from NewResult ! " + ex5.Message);
                    }


                    //controllerParameter.CopyTo(IndexIDStart + xmlIDStart.Length, XMLValue, 0, IndexIDEnd - IndexIDStart);

                    /* XML-data for COM */
                    var xmlCOMStart = "<comPort>";
                    var xmlCOMEnd = "</comPort>";

                    int IndexCOMStart = NewResult.LastIndexOf(xmlCOMStart); // controllerParameter
                    int IndexCOMEnd = NewResult.LastIndexOf(xmlCOMEnd); // controllerParameter
                    string ControllerComPort = String.Empty;
                    try
                    {
                        ControllerComPort = NewResult.Substring(IndexCOMStart + xmlCOMStart.Length, IndexCOMEnd - IndexCOMStart - xmlCOMStart.Length); // controllerParameter
                    }
                    catch (Exception ex6)
                    {
                        Log(Name + ": cannot access controller ComPort string from NewResult ! " + ex6.Message);
                    }

                    int pFromControllerCOM = NewResult.LastIndexOf(xmlCOMStart);
                    int pToControllerCOM = NewResult.LastIndexOf(xmlCOMEnd) + xmlCOMEnd.Length;
                    string ControllerCOMXMLStg = string.Empty;
                    string NewControllerCOMXMLStg = string.Empty;
                    try
                    {
                        ControllerCOMXMLStg = NewResult.Substring(pFromControllerCOM, pToControllerCOM - pFromControllerCOM /* + xmlCOMEnd.Length */);
                        NewControllerCOMXMLStg = ControllerCOMXMLStg.Replace(ControllerComPort, tfExt.multiControllerInfo[ControllerIdToAdd].controllerPort);
                        NewResult = NewResult.Replace(ControllerCOMXMLStg, NewControllerCOMXMLStg);
                    }
                    catch (Exception ex7)
                    {
                        Log(Name + ": cannot change / replace controller ComPort string in NewResult ! " + ex7.Message);
                    }

                    /* XML-data for Controller-Name / Type */
                    var xmlTYPEStart = "<type>";
                    var xmlTYPEEnd = "</type>";

                    int IndexTYPEStart = NewResult.LastIndexOf(xmlTYPEStart); // controllerParameter
                    int IndexTYPEEnd = NewResult.LastIndexOf(xmlTYPEEnd); // controllerParameter
                    string ControllerTYPE = string.Empty;
                    try
                    {
                        ControllerTYPE = NewResult.Substring(IndexTYPEStart + xmlTYPEStart.Length, IndexTYPEEnd - IndexTYPEStart - xmlTYPEStart.Length); // controllerParameter
                    }
                    catch (Exception ex8)
                    {
                        Log(Name + ": cannot access controller type string in NewResult ! " + ex8.Message);
                    }

                    int pFromControllerTYPE = NewResult.LastIndexOf(xmlTYPEStart);
                    int pToControllerTYPE = NewResult.LastIndexOf(xmlTYPEEnd) + xmlTYPEEnd.Length;
                    string ControllerTYPEXMLStg = String.Empty;
                    string NewControllerTYPEXMLStg = string.Empty;
                    try
                    {
                        ControllerTYPEXMLStg = NewResult.Substring(pFromControllerTYPE, pToControllerTYPE - pFromControllerTYPE /* + xmlTYPEEnd.Length */);
                        NewControllerTYPEXMLStg = ControllerTYPEXMLStg.Replace(ControllerTYPE, tfExt.multiControllerInfo[ControllerIdToAdd].controllerType);
                        NewResult = NewResult.Replace(ControllerTYPEXMLStg, NewControllerTYPEXMLStg);
                    }
                    catch (Exception ex9)
                    {
                        Log(Name + ": cannot change / replace controller type string in NewResult ! " + ex9.Message);
                    }
                    string newResultDebug = ControllerIDXMLStg + " | " + ControllerCOMXMLStg + " | " + ControllerTYPEXMLStg + "\n" +
                        NewControllerIDXMLStg + " | " + NewControllerCOMXMLStg + " | " + NewControllerTYPEXMLStg;

                    /* 22.11.2021 */
                    if (Update)
                    {
                        if (!Delete)
                            /* Replace-Mode at the defined range in XML-section */
                            tfExt.SFBXMLConfig = tfExt.SFBXMLConfig.Replace(result, NewResult);
                        /* 04.01.2022 */
                        else
                            tfExt.SFBXMLConfig = tfExt.SFBXMLConfig.Replace(result, string.Empty); // (pFrom, pTo - pFrom);

                        /* Ende 04.01.2022 */
                    }
                    else
                        /* Add-Mode at the end of XML-section */
                        tfExt.SFBXMLConfig = tfExt.SFBXMLConfig.Insert(pTo + 1, NewResult);

                    /* 23.11.2021 : DEBUG */
                    /* 04.01.2022 : Zusatz fuer DELETE-Funktion */
                    string GUI_Message = string.Empty;

                    if (!Delete)
                        GUI_Message = "Overtake new MC-configuration ?";
                    else
                        GUI_Message = "Overtake new MC-configuration with deleted controller ?";


                    if (MessageBox.Show(GUI_Message, "SFB Multi-Controller Setup - GUI config", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.IO.File.WriteAllText(tfExt.SFBConfigPfad, tfExt.SFBXMLConfig);
                        RetVal = 1;  // Code success overtaking setting
                    }
                    else
                        RetVal = -1; // Code for theoretical success but user-intervention => not saved                    
                }
            }
            catch (Exception ex)
            {
                /* 10.10.2021 @ TPHW */
                /*
                GuiLoggerProvider.Instance.Log("Error during Reading of: " + xmlFile + "Error:" + ex.Message);
                */
                /* Ende 10.10.2021 @ TPHW */
                Log(Name + ": " + Convert.ToString(tfExt.SFBConfigPfad) + " cannot be updated ! " + ex.Message);
            }
            return RetVal;
        }
        /* Ende 12.10.2021 */

        public void GetAvailableControllerPorts()
        {
            if (tfExt.IsRunning)
            {
                // hole Liste im System verfuegbarer COMs
                AvailableComPorts = SerialPort.GetPortNames();

                comboBoxCOMs.Items.Clear();

                foreach (string ComPort in AvailableComPorts)
                {
                    for (int n = 0; n < tfExt.CurrentMCCount; n++)
                    {
                        string[] SplitStg = ComPort.Split(tfExt.cCOMStg, StringSplitOptions.None);

                        string PortNoOnly = SplitStg[1];
                        if (!(String.Compare(PortNoOnly /* ComPort */, tfExt.multiControllerInfo[n].controllerPort) == 0))
                        {
                            if (!comboBoxCOMs.Items.Contains(ComPort))
                                comboBoxCOMs.Items.Add(ComPort);
                        }
                    }
                }
                comboBoxCOMs.SelectedIndex = 0;
            }
        }

        internal void Start()
        {           
            timerRefresh.Enabled = true;
            /* 12.10.2021 : moved to TimerEvent */
            /*
            if (tfExt.IsRunning)                 
            {
                if (tfExt.CurrentMCCount > 0)
                {
                    labelCtrlCount.Enabled = true;
                    labelCtrlCount.Text = Convert.ToString(tfExt.CurrentMCCount - 1); // GetItemText
                    labelCtrlCount.Enabled = false;
                    Log("controller found");
                    buttonShowCurrent_Click(null, null);
                }
                else
                    Log("no controller found configured in SFB !");
            }           
            */
        }

        internal void Stop()
        {

        }

        /*
        private void btnCheckTimer_Click(object sender, EventArgs e)
        {
            TimerUpgradeNecessary();
        }
        */
        /*
        private void btnApplyFix_Click(object sender, EventArgs e)
        {
            Start();
        }
        */

        private void labelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(labelLink.Text);
            Process.Start(sInfo);
        }

        private void lblDesc_Click(object sender, EventArgs e)
        {

        }

        private void logBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            /*
            if ((tfExt.UserPositionFound) && (!tfExt.UserPositionSet))
            {
                PositionChangeLogged = false;

                Log(Name + ": user-defined program position found");
                PositionChangeLogged = true;
            }
            if (tfExt.UserPositionSet)
            {
                PositionChangeLogged = false;

                Log(Name + ": user-defined program position set");
                PositionChangeLogged = true;
                DrawSuccess();
                timerRefresh.Enabled = false;
            }
            else
            {
                DrawError();
                if (!PositionChangeLogged)
                {
                    Log("no user-info found. Keeping original program position");
                    PositionChangeLogged = true;
                }
            }
            */
            if ((tfExt.IsRunning) && (tfExt.MCCountRefresh))
            {
                if (tfExt.CurrentMCCount > 0)
                {                   
                    labelCtrlCount.Text = Convert.ToString(tfExt.CurrentMCCount); /* GetItemText */
                    Log("controller found");
                    buttonShowCurrent_Click(null, null);
                }
                else
                    Log("no controller found configured in SFB !");
            }
            /* 12.10.2021 */
            tfExt.MCCountRefresh = false;
            timerRefresh.Interval = 5000; /* ms */
            /* Ende 12.10.2021 */
        }        

        private void btnAddController_Click(object sender, EventArgs e)
        {
            if (tfExt.IsRunning)
            {
                groupBoxNewControllerConfig.Enabled = true;
                labelMCId.Text = tfExt.cIDStg[0] + Convert.ToString(tfExt.CurrentMCCount + 1);
                GetAvailableControllerPorts();

                btnEditController.Enabled = false;
                btnAddController.Enabled = false;
                /* 04.01.2022 : DELETE Button */
                btnDeleteController.Enabled = false;
            }
        }

        private void BackupConfig()
        {
            // erstelle Backup der SFB-Config
            string BackupConfigPath = System.IO.Path.GetDirectoryName(tfExt.SFBConfigPfad);
            string BackupConfigFile = System.IO.Path.GetFileNameWithoutExtension(tfExt.SFBConfigPfad) + "-bkp";
            string BackupConfigFileExt = System.IO.Path.GetExtension(tfExt.SFBConfigPfad);
            int BkpFileCounter = 0;
            string SFBConfigFileBackup = string.Empty;

            do
            {
                SFBConfigFileBackup = System.IO.Path.Combine(BackupConfigPath, BackupConfigFile + Convert.ToString(BkpFileCounter) + BackupConfigFileExt);
                BkpFileCounter++;
            }
            while (System.IO.File.Exists(SFBConfigFileBackup));

            System.IO.File.Copy(tfExt.SFBConfigPfad, SFBConfigFileBackup);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (tfExt.IsRunning)
            {
                // erstelle Backup der SFB-Config
                BackupConfig(); /* 04.01.2022 : in eigene Funktion verschoben */

                /*
                string BackupConfigPath = System.IO.Path.GetDirectoryName(tfExt.SFBConfigPfad);
                string BackupConfigFile = System.IO.Path.GetFileNameWithoutExtension(tfExt.SFBConfigPfad) + "-bkp";
                string BackupConfigFileExt = System.IO.Path.GetExtension(tfExt.SFBConfigPfad);
                int BkpFileCounter = 0;
                string SFBConfigFileBackup = string.Empty;

                do
                {
                    SFBConfigFileBackup = System.IO.Path.Combine(BackupConfigPath, BackupConfigFile + Convert.ToString(BkpFileCounter) + BackupConfigFileExt);
                    BkpFileCounter++;
                }
                while (System.IO.File.Exists(SFBConfigFileBackup));

                System.IO.File.Copy(tfExt.SFBConfigPfad, SFBConfigFileBackup);
                */

                // speichere neue Multi-Controller-Configuration anhand User-Settings
                MCSetupExtension.MultiControllerInfo NewCtrlToAdd;
                string tempStg = string.Empty;
                tempStg = labelMCId.Text;

                string[] SubStgs = tempStg.Split(tfExt.cIDStg, StringSplitOptions.None);

                // Label fuer neuen Controller ist bereits beim "Add"-Befehl aktualisiert worden !
                if (SubStgs.Length > 1)
                    NewCtrlToAdd.controllerId = SubStgs[1];
                else
                // Erhoehe die neue Controller-ID um 1 weil bisher ja noch Wert auf aktueller Config fixiert ist !
                    NewCtrlToAdd.controllerId = Convert.ToString(tfExt.CurrentMCCount + 1);

                if (textBoxControllerName.Lines.Length != 0)
                    NewCtrlToAdd.controllerType = textBoxControllerName.Lines[0].ToString();
                else
                    NewCtrlToAdd.controllerType = "Ctrl-" + NewCtrlToAdd.controllerId;

                if (comboBoxCOMs.SelectedItem != null)
                {
                    tempStg = comboBoxCOMs.GetItemText(comboBoxCOMs.SelectedItem);
                    SubStgs = tempStg.Split(tfExt.cCOMStg, StringSplitOptions.None);
                    if (SubStgs.Length > 1)
                        NewCtrlToAdd.controllerPort = SubStgs[1];
                    else
                        NewCtrlToAdd.controllerPort = "nv";
                }
                else
                    NewCtrlToAdd.controllerPort = "nv";

                // Uebernahme falls kein undefinierter Port verwendet wurde !
                string RecognizedComPortStg = tfExt.cCOMStg[0] + NewCtrlToAdd.controllerPort;
                if (comboBoxCOMs.FindStringExact(RecognizedComPortStg) != -1)
                {
                    /* 22.11.2021 */
                    int NewCtrlId = 0; // Init
                    if (!EditModeActive)
                    {
                        tfExt.CurrentMCCount++;
                        NewCtrlId = tfExt.CurrentMCCount - 1;
                    }
                    else
                        NewCtrlId = Convert.ToByte(NewCtrlToAdd.controllerId) - 1;
                    /* Ende 22.11.2021 */
                    tfExt.multiControllerInfo[NewCtrlId].controllerId = NewCtrlToAdd.controllerId;
                    tfExt.multiControllerInfo[NewCtrlId].controllerType = NewCtrlToAdd.controllerType;
                    tfExt.multiControllerInfo[NewCtrlId].controllerPort = NewCtrlToAdd.controllerPort;

                    // alle nun konfigurierten Controller mit Settings im Log anzeigen
                    /* 12.10.2021 : moved to TimerEvent */
                    /*
                    buttonShowCurrent_Click(null, null);
                    */
                    tfExt.MCCountRefresh = true;
                    /* 23.11.2021 : moved to ResetGUI Function */
                    /*
                    groupBoxNewControllerConfig.Enabled = false;
  
                    btnAddController.Enabled = true;
                    btnEditController.Enabled = true;

                    // 22.11.2021
                    if (!EditModeActive)
                        labelCtrlCount.Text = Convert.ToString(tfExt.CurrentMCCount - 1);

                    // 22.11.2021 
                    if (EditModeActive)
                    {
                        comboBoxID.Enabled = false;
                        EditModeActive = false;
                    }
                    */

                    int SaveAfterAdd_Edit = Add_Update_SFBConfig(NewCtrlId , EditModeActive, /* 04.01.2022 */ DeleteModeActive);

                    if (SaveAfterAdd_Edit > 0)
                        ResetGUIModeAfterAdd_Cancel();
                    else
                        if (SaveAfterAdd_Edit != -1)
                        Log("something went wrong while adding new defined multi-controller config ! ");     
                }
                else
                    Log("unavailable Controller-Port used. New controller cannot be added to config !");
            }
        }

        private void buttonShowCurrent_Click(object sender, EventArgs e)
        {
           //ShowControllerInfo();
           if (tfExt.IsRunning)
            {
                if (tfExt.CurrentMCCount != 0)
                {
                    for (int ControllerNo = 0; ControllerNo < tfExt.CurrentMCCount; ControllerNo++)
                    {
                        string LogText = "ID= " + tfExt.multiControllerInfo[ControllerNo].controllerId + " | Name= " + tfExt.multiControllerInfo[ControllerNo].controllerType + " | configured @ COM" +
                            tfExt.multiControllerInfo[ControllerNo].controllerPort;
                        Log(LogText);
                    }
                }
                else
                    Log("no controller found in SFB ! No operation here possible ");
           }
        }

        private void logBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            String doubleClickedText = logBox.Items[logBox.SelectedIndices[0]].ToString();            
        }

        private void btnAddController_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void comboBoxID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEditController_Click(object sender, EventArgs e)
        {
            if (tfExt.IsRunning)
            {
                EditModeActive = true;

                btnEditController.Enabled = false;
                btnAddController.Enabled = false;
                /* 04.01.2022 : DELETE Button */
                btnDeleteController.Enabled = true;


                groupBoxNewControllerConfig.Enabled = true;
                GetAvailableControllerPorts();

                comboBoxID.Items.Clear();
                if (tfExt.CurrentMCCount != 0)
                {
                    for (int ControllerNo = 0; ControllerNo < tfExt.CurrentMCCount; ControllerNo++)
                    {
                        string IDText = tfExt.multiControllerInfo[ControllerNo].controllerId;
                        comboBoxID.Items.Add(IDText);
                    }
                }
                comboBoxID.Enabled = true;
                comboBoxID.Visible = true;

                comboBoxID.SelectedIndex = -1;
            }
        }

        private void comboBoxID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            /* 22.11.2021 */
            int SelectedControllerID = Convert.ToByte(comboBoxID.Text) - 1;

            labelMCId.Text = tfExt.cIDStg[0] + comboBoxID.Text;

            textBoxControllerName.Text = tfExt.multiControllerInfo[SelectedControllerID].controllerType;
            string ComPortStg = tfExt.cCOMStg + tfExt.multiControllerInfo[SelectedControllerID].controllerPort;
            int FoundCOMPortIndex = comboBoxCOMs.FindStringExact(ComPortStg);
            if (FoundCOMPortIndex != -1)
                comboBoxCOMs.SelectedIndex = FoundCOMPortIndex;
            else
                comboBoxCOMs.SelectedIndex = 0;
        }

        private void ResetGUIModeAfterAdd_Cancel()
        {
            groupBoxNewControllerConfig.Enabled = false;

            if (EditModeActive)
            {
                comboBoxID.Enabled = false;
                EditModeActive = false;
            }

            btnAddController.Enabled = true;
            btnEditController.Enabled = true;
            /* 04.01.2022 : DELETE Button */
            btnDeleteController.Enabled = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ResetGUIModeAfterAdd_Cancel();            
        }

        private void labelLink_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(labelLink.Text);
            Process.Start(sInfo);
        }

        private void btnDeleteController_Click(object sender, EventArgs e)
        {
            if (tfExt.IsRunning)
            {
                //EditModeActive = true;
                DeleteModeActive = true;
                btnAddController.Enabled = false;
                btnEditController.Enabled = false;

                if (comboBoxID.SelectedIndex != -1)
                {
                    int SelectedControllerID = Convert.ToByte(comboBoxID.Text) - 1;
                    groupBoxNewControllerConfig.Enabled = false;

                    //if (MessageBox.Show("Delete selected MC-configuration now?", "SFB Multi-Controller Setup - GUI config", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BackupConfig();
                        int SaveAfterAdd_Edit = Add_Update_SFBConfig(SelectedControllerID, EditModeActive, /* 04.01.2022 */ DeleteModeActive);

                        if (SaveAfterAdd_Edit > 0)
                        {
                            tfExt.CurrentMCCount--;
                            tfExt.MCCountRefresh = true;

                            ResetGUIModeAfterAdd_Cancel();                            
                            
                            /* 04.01.2022 : Reload neue XML-Config */
                            MessageBox.Show("Please restart SimFeedback now ! Updated configuration will be loaded and shown on restart", "SFB Multi-Controller Setup - GUI config", MessageBoxButtons.OK);
                        }
                        else
                        {
                            if (SaveAfterAdd_Edit != -1)
                                Log("something went wrong while adding new defined multi-controller config ! ");
                        }
                        EditModeActive = false;
                        DeleteModeActive = false;

                        /* 04.01.2022 : DELETE Button */
                        btnDeleteController.Enabled = false;
                    }
                }
                else
                    MessageBox.Show("To use delete function first select corresponding controller's no. (none selected yet)", "SFB Multi-Controller Setup - GUI config", MessageBoxButtons.OK);

                DeleteModeActive = false;
            }
        }
    }
}
