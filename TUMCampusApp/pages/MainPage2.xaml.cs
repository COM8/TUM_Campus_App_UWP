﻿using Data_Manager;
using System;
using System.Collections.ObjectModel;
using TUMCampusApp.Classes;
using TUMCampusApp.DataTemplates;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Pages
{
    public sealed partial class MainPage2 : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private ObservableCollection<MainPageSplitViewItemTemplate> splitViewItems;
        private Type requestedPage;
        private object requestedPageArgs;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 07/04/2018 Created [Fabian Sauter]
        /// </history>
        public MainPage2() : this(typeof(HomePage), null)
        {
        }

        public MainPage2(Type page, string arguments)
        {
            this.requestedPage = page;
            this.splitViewItems = new ObservableCollection<MainPageSplitViewItemTemplate>();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage2_BackRequested;
            UIUtils.mainPage = this;
            loadSplitViewItems();
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Navigates to the given page with the given arguments.
        /// </summary>
        /// <param name="page">The target page.</param>
        /// <param name="args">The navigation arguments.</param>
        public void navigateToPage(Type page, object args)
        {
            if (page != null)
            {
                for (int i = 0; i < splitViewItems.Count; i++)
                {
                    if (splitViewItems[i] is MainPageSplitViewItemButtonTemplate buttonTemplate && buttonTemplate.page == page)
                    {
                        mainFrame.Navigate(page, args);
                        return;
                    }
                }
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Sets the faculty_img source based on the selected faculty.
        /// </summary>
        private void setImage()
        {
            int facultyIndex = Settings.getSettingInt(SettingsConsts.FACULTY_INDEX);
            switch (facultyIndex)
            {
                case 0:
                case 3:
                case 5:
                    faculty_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/im.png"));
                    break;
                case 1:
                case 2:
                    faculty_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/mw.png"));
                    break;
                default:
                    faculty_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/wear_tuition_fee1.png"));
                    break;
            }
        }

        /// <summary>
        /// Adds all items to the splitViewItems.
        /// </summary>
        private void loadSplitViewItems()
        {
            if (Settings.getSettingBoolean(SettingsConsts.TUM_ONLINE_ENABLED))
            {
                splitViewItems.Add(new MainPageSplitViewItemDescriptionTemplate() { text = UIUtils.getLocalizedString("MainPageMyTUMItem_Text") });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UIUtils.getLocalizedString("MainPageCalendarItem_Text"),
                    icon = "\uE787",
                    page = typeof(MyCalendarPage)
                });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UIUtils.getLocalizedString("MainPageLecturesItem_Text"),
                    icon = "\uEF16",
                    page = typeof(MyLecturesPage)
                });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UIUtils.getLocalizedString("MainPageGradesItem_Text"),
                    icon = "\uEADF",
                    page = typeof(MyGradesPage)
                });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UIUtils.getLocalizedString("MainPageTuitionFeesItem_Text"),
                    icon = "$",
                    page = typeof(TuitionFeesPage),
                    iconFont = new FontFamily("Segoe UI"),
                    iconMargin = new Thickness(5, -3, 0, 0),
                    textMargin = new Thickness(5, 0, 0, 0)
                });
            }

            splitViewItems.Add(new MainPageSplitViewItemDescriptionTemplate() { text = UIUtils.getLocalizedString("MainPageGeneralTUMItem_Text") });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageHomeItem_Text"),
                icon = "\uE80F",
                page = typeof(HomePage)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageCanteensItem_Text"),
                icon = "\uD83C\uDF74",
                page = typeof(CanteensPage2),
                iconFont = new FontFamily("Segoe UI Symbol"),
                iconMargin = new Thickness(0, -3, 0, 0)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageNewsItem_Text"),
                icon = "\uE701",
                page = typeof(NewsPage)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageTransportItem_Text"),
                icon = "\uE7C0",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPagePlansItem_Text"),
                icon = "\uE826",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageRoomfinderItem_Text"),
                icon = "\uE816",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageStudyRoomItem_Text"),
                icon = "\uE7BC",
                page = typeof(StudyRoomPage)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageOpeningHoursItem_Text"),
                icon = "\uE823",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageStudyPlansItem_Text"),
                icon = "\uE762",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UIUtils.getLocalizedString("MainPageSettingsItem_Text"),
                icon = "\uE713",
                page = typeof(SettingsPage)
            });
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void openSplitView_hbtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage_spv.IsPaneOpen = !mainPage_spv.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (main_lb.SelectedItem != null && main_lb.SelectedItem is MainPageSplitViewItemButtonTemplate buttonTemplate)
            {
                navigateToPage(buttonTemplate.page, null);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigated -= MainFrame_Navigated;
            mainFrame.Navigated += MainFrame_Navigated;

            if (requestedPage != null)
            {
                navigateToPage(requestedPage, requestedPageArgs);
                requestedPage = null;
                requestedPageArgs = null;
            }

            setImage();
        }

        /// <summary>
        /// Update the current page name and the selected page.
        /// </summary>
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content != null)
            {
                Type contentType = e.Content.GetType();
                for (int i = 0; i < splitViewItems.Count; i++)
                {
                    if (splitViewItems[i] is MainPageSplitViewItemButtonTemplate buttonTemplate && buttonTemplate.page == contentType)
                    {
                        main_lb.SelectedIndex = i;
                        pageName_tbx.Text = splitViewItems[i].text ?? "";
                        return;
                    }
                }
            }
        }

        private void MainPage2_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack && !e.Handled)
            {
                e.Handled = true;
                mainFrame.GoBack();
            }
        }

        /// <summary>
        /// Workaround for disabling ListBoxItems:
        /// Source: https://social.technet.microsoft.com/wiki/contents/articles/33548.uwp-disabling-selection-of-items-in-a-listview.aspx
        /// </summary>
        private void ItemDescriptionTextBlock_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MainPageSplitViewItemDescriptionTemplate template)
            {
                var x = main_lb.ContainerFromItem(template);
                if (x is ListBoxItem item)
                {
                    item.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Workaround for disabling ListBoxItems:
        /// Source: https://social.technet.microsoft.com/wiki/contents/articles/33548.uwp-disabling-selection-of-items-in-a-listview.aspx
        /// </summary>
        private void ItemButtonStackPanel_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MainPageSplitViewItemButtonTemplate template)
            {
                var x = main_lb.ContainerFromItem(template);
                if (x is ListBoxItem item)
                {
                    item.IsEnabled = template.isEnabled;
                }
            }
        }

        #endregion
    }
}
