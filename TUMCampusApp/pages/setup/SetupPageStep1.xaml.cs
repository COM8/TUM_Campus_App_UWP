﻿using System;
using System.Text.RegularExpressions;
using TUMCampusAppAPI.Managers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusAppAPI;
using TUMCampusApp.Classes;
using Data_Manager;

namespace TUMCampusApp.Pages.Setup
{
    public sealed partial class SetupPageStep1 : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/12/2016  Created [Fabian Sauter]
        /// </history>
        public SetupPageStep1()
        {
            this.InitializeComponent();
            string uId = Settings.getSettingString(SettingsConsts.USER_ID);
            studentID_tbx.Text = uId == null ? "" : uId;
            populateFacultiesComboBox();
            faculty_cbox.SelectedIndex = Settings.getSettingInt(SettingsConsts.FACULTY_INDEX);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Checks whether the current student id is valid.
        /// </summary>
        private bool isIdValid()
        {
            Regex reg = new Regex("[a-z]{2}[0-9]{2}[a-z]{3}");
            return reg.Match(studentID_tbx.Text.ToLower()).Success;
        }

        /// <summary>
        /// Adds all faculties to the faculty_cbox.
        /// </summary>
        private void populateFacultiesComboBox()
        {
            foreach (Faculties f in Enum.GetValues(typeof(Faculties))) {
                faculty_cbox.Items.Add(new ComboBoxItem()
                {
                    Content = UIUtils.getLocalizedString(f.ToString() + "_Text"),

                });
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void skip_btn_Click(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.TUM_ONLINE_ENABLED, false);
            Settings.setSetting(SettingsConsts.HIDE_WIZARD_ON_STARTUP, true);
            if (Window.Current.Content is Frame f)
            {
                f.Navigate(typeof(MainPage2));
            }
        }

        private async void next_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            next_btn.IsEnabled = false;
            if (!isIdValid())
            {
                MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("InvalidId_Text"))
                {
                    Title = UIUtils.getLocalizedString("Error_Text")
                };
                await message.ShowAsync();
            }
            else if(faculty_cbox.SelectedIndex < 0)
            {
                MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("SelectFaculty_Text"))
                {
                    Title = UIUtils.getLocalizedString("Error_Text")
                };
                await message.ShowAsync();
            }
            else
            {
                if(tumOnlineToken_tbx.Visibility == Visibility.Collapsed)
                {
                    string result = await TumManager.INSTANCE.reqestNewTokenAsync(studentID_tbx.Text.ToLower());
                    if (result == null)
                    {
                        MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("RequestNewTokenError_Text"))
                        {
                            Title = UIUtils.getLocalizedString("Error_Text")
                        };
                        await message.ShowAsync();
                    }
                    else if (result.Contains("Es wurde kein Benutzer zu diesen Benutzerdaten gefunden"))
                    {
                        MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("InvalidId_Text"))
                        {
                            Title = UIUtils.getLocalizedString("Error_Text")
                        };
                        await message.ShowAsync();
                    }
                    else
                    {
                        Settings.setSetting(SettingsConsts.FACULTY_INDEX, faculty_cbox.SelectedIndex);
                        Settings.setSetting(SettingsConsts.USER_ID, studentID_tbx.Text.ToLower());
                        if (Window.Current.Content is Frame f)
                        {
                            f.Navigate(typeof(SetupPageStep2));
                        }
                    }
                }
                else
                {
                    string token = tumOnlineToken_tbx.Text.ToUpper();
                    if (!TumManager.INSTANCE.isTokenValid(token))
                    {
                        MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("InvalidToken_Text"))
                        {
                            Title = UIUtils.getLocalizedString("Error_Text")
                        };
                        await message.ShowAsync();
                    }
                    else
                    {
                        Settings.setSetting(SettingsConsts.FACULTY_INDEX, faculty_cbox.SelectedIndex);
                        Settings.setSetting(SettingsConsts.USER_ID, studentID_tbx.Text.ToLower());
                        TumManager.INSTANCE.saveToken(token);
                        if (Window.Current.Content is Frame f)
                        {
                            f.Navigate(typeof(SetupPageStep2));
                        }
                    }
                }
            }
            next_btn.IsEnabled = true;
        }

        private void useExistingToken_btn_Click(object sender, RoutedEventArgs e)
        {
            if(tumOnlineToken_tbx.Visibility == Visibility.Collapsed)
            {
                tumOnlineToken_tbx.Visibility = Visibility.Visible;
                useExistingToken_btn.Content = UIUtils.getLocalizedString("SetupPage1DontUseExistingToken");
            }
            else
            {
                tumOnlineToken_tbx.Visibility = Visibility.Collapsed;
                useExistingToken_btn.Content = UIUtils.getLocalizedString("SetupPage1UseExistingToken");
            }
        }

        #endregion
    }
}
