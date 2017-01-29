﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Classes.Managers;
using TUMCampusApp.Classes.Tum;
using TUMCampusApp.Classes.Tum.Exceptions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Pages
{
    public sealed partial class TuitionFeesPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/01/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeesPage()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private async Task downloadAndShowFeesAsync(bool forceRedownload)
        {
            try
            {
                await TuitionFeeManager.INSTANCE.downloadFeesAsync(forceRedownload);
            }
            catch (BaseTUMOnlineException e)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask().Wait();
                return;
            }
            List<TUMTuitionFee> list = new List<TUMTuitionFee>();
            list = TuitionFeeManager.INSTANCE.getFees();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showFees(list);
            }).AsTask().Wait();
        }

        private void showNoAccess(BaseTUMOnlineException e)
        {
            noFees_grid.Visibility = Visibility.Collapsed;
            fees_grid.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Visible;

            if (e is InvalidTokenTUMOnlineException)
            {
                noData_tbx.Text = "Your token is not activated yet!";
            }
            else if(e is NoAccessTUMOnlineException)
            {
                noData_tbx.Text = "No access on your tuition fee status!";
            }
            else
            {
                noData_tbx.Text = "Unknown exception!\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void showFees(List<TUMTuitionFee> list)
        {
            noData_grid.Visibility = Visibility.Collapsed;
            if (list == null || list.Count <= 0)
            {
                noFees_grid.Visibility = Visibility.Visible;
                fees_grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                fees_grid.Visibility = Visibility.Visible;
                noFees_grid.Visibility = Visibility.Collapsed;
                outsBalance_tbx.Text = list[0].money + "€";
                semester_tbx.Text = list[0].semesterDescripion;
                DateTime deadLine = DateTime.Parse(list[0].deadline);
                TimeSpan tS = deadLine.Subtract(DateTime.Now);
                deadline_tbx.Text = deadLine.Day + "." + deadLine.Month + "." + deadLine.Year + " ==> " + Math.Round(tS.TotalDays) + " Days left!";
                if (tS.TotalDays <= 30)
                {
                    main_grid.Background = new SolidColorBrush(Windows.UI.Colors.DarkRed);
                }
            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void HyperlinkButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Utillities.launchBrowser(new Uri(@"https://www.tum.de/en/studies/advising/student-financial-aid/"));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => downloadAndShowFeesAsync(false));
        }
        #endregion
    }
}
