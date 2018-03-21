using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Text;
using System.Diagnostics;
using Tridion.ContentManager.CoreService.Client;
using System.Web.UI.WebControls;
using System.Threading;

namespace TridionAdminCheckIn.CoreService
{
    public class TridionCheckIn : IDisposable
    {
        private readonly SessionAwareCoreServiceClient _client;
        private static Label _label;
        private static EventLog _eventLog;

        public TridionCheckIn()
        {
            _eventLog = new EventLog();
            _eventLog.Source = "TridionAdminCheckIn";

            var netTcpBinding = new NetTcpBinding
            {
                MaxReceivedMessageSize = 2147483647,
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxStringContentLength = 2147483647,
                    MaxArrayLength = 2147483647
                }
            };
            
            var remoteAddress = new EndpointAddress("net.tcp://localhost:2660/CoreService/2011/netTcp");
            _client = new SessionAwareCoreServiceClient(netTcpBinding, remoteAddress);
        }

        public void Dispose()
        {
            if (this._client.State == CommunicationState.Faulted)
                this._client.Abort();
            else
                this._client.Close();
        }

        public void CheckInItem(string id, Label label)
        {
            if (label != null)
            {
                _label = label;
            }

            string tridionInstallPath = Environment.GetEnvironmentVariable("TRIDION_CM_HOME") + @"web\WebUI\Editors\AdminCheckIn\Config\";
            string filename = "admincheckincm.config";

            string scheduleFile = System.IO.Path.Combine(tridionInstallPath, filename);
            _eventLog.WriteEntry("PATH : " + scheduleFile, EventLogEntryType.Information);
            XmlDocument doc = new XmlDocument();
            _eventLog.WriteEntry("Test");
            doc.Load(scheduleFile);
            //CultureInfo provider = CultureInfo.InvariantCulture;
            if (doc == null)
            {
                _eventLog.WriteEntry("doc is null");
            }
            _eventLog.WriteEntry("After Test");
            // has trouble accessing by selectSingleNode.  
            XmlNodeList adminList = doc.GetElementsByTagName("checkInAdminUsername");
            XmlNodeList adminPasswordList = doc.GetElementsByTagName("checkInAdminPassword");
            XmlNode adminusername = null;
            XmlNode adminpassword = null;
            if (adminList != null && adminList.Count > 0 && adminPasswordList != null && adminPasswordList.Count > 0)
            {
                adminusername = adminList[0];
                adminpassword = adminPasswordList[0];
            }
            if (adminusername == null || adminpassword == null)
            {
                label.Text = "The admin username or password is empty.  Please ensure you specified the username and password in the admincheckincm.config";
            }
            _eventLog.WriteEntry("Username: " + adminusername.InnerText, EventLogEntryType.Information);
            _eventLog.WriteEntry("PW: " + adminpassword.InnerText, EventLogEntryType.Information);
            String userName = adminusername.InnerText;
            String password = adminpassword.InnerText;

            var credentials = CredentialCache.DefaultNetworkCredentials;
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                credentials = new NetworkCredential(userName, password);
            }
            _client.ChannelFactory.Credentials.Windows.ClientCredential = credentials;
            try
            {
                VersionedItemData obj = _client.Read(id, new ReadOptions()) as VersionedItemData;
                
                if (obj is ComponentData || obj is PageData)
                {
                    BluePrintInfo blueprintInfo = null;
                    FullVersionInfo info = null;
                    if (obj is ComponentData)
                    {
                        ComponentData comp = obj as ComponentData;
                        info = comp.VersionInfo as FullVersionInfo;
                        blueprintInfo = comp.BluePrintInfo as BluePrintInfo;
                    }
                    if (obj is PageData)
                    {
                        PageData page = obj as PageData;
                        info = page.VersionInfo as FullVersionInfo;
                        blueprintInfo = page.BluePrintInfo;
                    }
                    if (info.LockType != LockType.CheckedOut)
                    {
                        _label.Text = "Item is Not Checked Out";
                        return;
                    }
                    if (blueprintInfo.IsLocalized == false && blueprintInfo.IsShared == true)
                    {
                        _label.Text = "Item is Shared Item. Please Checkin from the Owning Publication";
                        return;
                    }
                }
                if (obj is ComponentData || obj is PageData)
                {
                    _label.Text = "Checking in item";
                    this._client.CheckInCompleted += new EventHandler<CheckInCompletedEventArgs>(TridionCheckIn.CheckInCallback);
                    this._client.CheckInAsync(id, new ReadOptions());
                    _eventLog.WriteEntry("Checking in item", EventLogEntryType.Information);
                }
                else
                {
                    _label.Text = "Can not check in. Item is not Component or Page!";
                    return;
                }
            }
            catch (Exception e)
            {
                _label.Text = "Username and Password provided is invalid";
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
            }
        }

        private static void CheckInCallback(object sender, CheckInCompletedEventArgs e)
        {
            _label.Text = "Completed Check In for " + "\"" + e.Result.Title + "\"" + " (" + e.Result.Id.ToString() + ")";
        }
    }
}