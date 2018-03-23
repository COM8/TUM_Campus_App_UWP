﻿using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Events;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;

namespace TUMCampusApp.Classes.Helpers
{
    class EduroamHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public const string EDUROAM_SSID = "eduroam";

        private WiFiAdapter adapter = null;
        private DispatcherTimer timer = null;
        private bool stopRequested;

        public delegate void EduroamNetworkFoundEventHandler(WiFiAdapter adapter, EduroamNetworkFoundEventArgs args);

        public event EduroamNetworkFoundEventHandler EduroamNetworkFound;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 21/03/2018 Created [Fabian Sauter]
        /// </history>
        public EduroamHelper()
        {
            this.stopRequested = false;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        public async Task<WiFiConnectionResult> connectAsync(WiFiAvailableNetwork network, WiFiReconnectionKind wiFiReconnectionKind, PasswordCredential passwordCredential)
        {
            return await adapter.ConnectAsync(network, wiFiReconnectionKind, passwordCredential);
        }

        public async Task installCertificateAsync()
        {
            StorageFile certFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Resources/Telekom_root_cert_eduroam.cer"));
            byte[] certBytes;

            using (Stream stream = await certFile.OpenStreamForReadAsync())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    certBytes = memoryStream.ToArray();
                }
            }

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                X509Certificate2 cert = new X509Certificate2(certBytes);

                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
            }
        }

        public async Task<WiFiAccessStatus> requestAccessAsync()
        {
            return await WiFiAdapter.RequestAccessAsync();
        }

        public async Task<WiFiAdapter> loadAdapterAsync()
        {
            DeviceInformationCollection adapterResult = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
            if (adapterResult.Count >= 1)
            {
                return await WiFiAdapter.FromIdAsync(adapterResult[0].Id);
            }
            else
            {
                return null;
            }
        }

        public async Task startSearchingAsync()
        {
            WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                // No access:
            }
            else
            {
                DeviceInformationCollection adapterResult = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (adapterResult.Count >= 1)
                {
                    adapter = await WiFiAdapter.FromIdAsync(adapterResult[0].Id);
                    adapter.AvailableNetworksChanged += Adapter_AvailableNetworksChanged;
                    await adapter.ScanAsync();
                }
                else
                {
                    // No wifi adapter
                }
            }

            await adapter.ScanAsync();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 10)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void stopSearching()
        {
            adapter.AvailableNetworksChanged -= Adapter_AvailableNetworksChanged;
            stopRequested = true;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Adapter_AvailableNetworksChanged(WiFiAdapter sender, object args)
        {
            foreach (WiFiAvailableNetwork n in sender.NetworkReport.AvailableNetworks)
            {
                if (string.Equals(n.Ssid, EDUROAM_SSID) && n.SecuritySettings.NetworkEncryptionType == NetworkEncryptionType.Ccmp && n.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Rsna)
                {
                    stopSearching();
                    EduroamNetworkFound?.Invoke(sender, new EduroamNetworkFoundEventArgs(n));
                    return;
                }
            }
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (stopRequested)
            {
                timer.Stop();
                return;
            }
            await adapter.ScanAsync();
        }

        #endregion
    }
}
