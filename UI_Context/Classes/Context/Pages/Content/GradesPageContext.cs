﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Controls.Grades;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class GradesPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly GradesPageDataTemplate MODEL = new GradesPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradesPageContext()
        {
            Task.Run(async () => await LoadGradesAsync(false));
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadGradesAsync(true));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadGradesAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            try
            {
                IEnumerable<Grade> grades = await GradesManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
                AddSortGrades(grades);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to load grades!", e);
            }
            MODEL.HasGrades = MODEL.GRADE_COLLECTIONS.Count > 0;
            MODEL.IsLoading = false;
        }

        private void AddSortGrades(IEnumerable<Grade> grades)
        {
            // Cache them in lists to prevent UI thread interrupts for each grade:
            Dictionary<string, List<Grade>> tmp = new Dictionary<string, List<Grade>>();
            foreach (Grade grade in grades)
            {
                if (!tmp.ContainsKey(grade.LectureSemester))
                {
                    tmp[grade.LectureSemester] = new List<Grade>();
                }
                tmp[grade.LectureSemester].Add(grade);
            }

            // Add them to the actual collections:
            List<GradesDataTemplate> gradesList = new List<GradesDataTemplate>();
            foreach (List<Grade> gList in tmp.Values)
            {
                gradesList.Add(new GradesDataTemplate(gList));
            }
            gradesList.Sort();
            if (gradesList.Count > 0)
            {
                gradesList[0].expanded = true;
            }
            MODEL.GRADE_COLLECTIONS.Clear();
            MODEL.GRADE_COLLECTIONS.AddRange(gradesList);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
