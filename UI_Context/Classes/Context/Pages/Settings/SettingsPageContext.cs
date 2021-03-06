﻿using Logging.Classes;
using Storage.Classes;
using UI_Context.Classes.Templates.Pages.Settings;

namespace UI_Context.Classes.Context.Pages.Settings
{
    public class SettingsPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly SettingsPageTemplate MODEL = new SettingsPageTemplate();
        private int versionTappCount = 0;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void OnVersionTextTapped()
        {
            versionTappCount++;
            if (versionTappCount >= 5)
            {
                versionTappCount = 0;

                bool debugSettingsEnabled = !Storage.Classes.Settings.GetSettingBoolean(SettingsConsts.DEBUG_SETTINGS_ENABLED);
                Storage.Classes.Settings.SetSetting(SettingsConsts.DEBUG_SETTINGS_ENABLED, debugSettingsEnabled);
                MODEL.DebugSettingsEnabled = debugSettingsEnabled;
                if (debugSettingsEnabled)
                {
                    Logger.Info("Debug settings enabled.");
                }
                else
                {
                    Logger.Info("Debug settings disabled.");
                }
            }
        }

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
