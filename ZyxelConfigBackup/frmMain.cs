using PrimS.Telnet;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace ZyxelConfigBackup
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public void SetState(string status)
        {
            txtStatus.Text = status;
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            lbxDevices.Items.Clear();
        }

        private void btnReadList_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog DevCSVDialog = new OpenFileDialog();
                DevCSVDialog.Filter = "Device Files |*.csv";
                DevCSVDialog.Title = "Select a Device File";

                if (DevCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var reader = new StreamReader(DevCSVDialog.FileName))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(';');

                            lbxDevices.Items.Add(values[0]);
                        }
                        txtStatus.Text = "Die Datei wurde erfolgreich eingelesen!";
                        lblItemCounter.Text = lbxDevices.Items.Count.ToString();
                    }
                }
                
            }
            catch(Exception ex)
            {
                txtStatus.Text = "Es ist ein Fehler aufgetreten: " + ex.Message;
            }
        }

        private void btnStartDownload_Click(object sender, EventArgs e)
        {
            //tbxLog.Text = "";
            InsertLog("Suche lokale IP...");
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            InsertLog("Verwende als LAN-IP folgende Adresse:" + localIP);
            InsertLog("Download wird initialisiert...");
            string tempPath = System.IO.Path.GetTempPath() + "configFiles";
            Directory.CreateDirectory(tempPath);
            InsertLog("Der Sicherungspfad wurde auf " + tempPath + " festgelegt");
            Process.Start("TFTPServer.exe");
            InsertLog("Der TFTP-Server wurde gestartet");
            foreach (string ip in lbxDevices.Items)
            {
                try
                {
                    Client client = new Client(ip, 23, CancellationToken.None);
                    InsertLog("Verbinde zu Client " + ip);
                    if (client.IsConnected)
                    {
                        InsertLog("Verbunden!");
                        client.WriteLine(tbxUsername.Text);
                        client.WriteLine(tbxPassword.Text);
                        client.WriteLine("copy running-config tftp " + localIP + " " + ip + ".conf");
                        InsertLog("Die Konfiguration wurde übertragen");
                    }
                    else
                    {
                        InsertLog("Verbindung konnte nicht hergestellt werden!");
                    }
                }
                catch (Exception ex)
                {
                    InsertLog("Verbindung konnte nicht hergestellt werden! IP-Adresse: " + ip);
                }
            }
            InsertLog("Bitte das TFTP-Serverfenster schliessen!");
            //InsertLog("Der Server wird gestoppt, bitte warten...");
            //System.Threading.Thread.Sleep(4000);
            //Process.Start("C:\\Windows\\System32\\taskkill.exe", "-f -im TFTPServer.exe");
            Thread worker = new Thread(KillTFTP);
            worker.IsBackground = true;
            worker.Start();
            //InsertLog("Der TFTP-Server wurde gestoppt!");

            Process.Start(tempPath);
        }

        public void InsertLog(string text)
        {
            tbxLog.Text += text + "\r\n";
            this.Refresh();
            tbxLog.ScrollToCaret();
        }
        public void KillTFTP()
        {
            Thread.Sleep(5000);
            Process.Start("C:\\Windows\\System32\\taskkill.exe", "-f -im TFTPServer.exe");
        }

    }

}
