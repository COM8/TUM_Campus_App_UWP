﻿using Shared.Classes;
using Shared.Classes.Collections;
using UI_Context.Classes.Templates.Controls.Calendar;
using Windows.UI.Xaml.Data;

namespace UI_Context.Classes.Templates.Pages.Content
{
    public class CalendarPageDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CollectionViewSource EVENTS_VIEW = new CollectionViewSource();
        public readonly CustomObservableCollection<CalendarEventGroupDataTemplate> EVENTS_COLLECTIONS = new CustomObservableCollection<CalendarEventGroupDataTemplate>(true);

        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }

        private bool _HasEvents;
        public bool HasEvents
        {
            get => _HasEvents;
            set => SetProperty(ref _HasEvents, value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CalendarPageDataTemplate()
        {
            EVENTS_VIEW.Source = EVENTS_COLLECTIONS;
            EVENTS_VIEW.IsSourceGrouped = true;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
