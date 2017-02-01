﻿using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Classes.UserDatas;

namespace TUMCampusApp.Classes.Caches
{
    class Cache
    {//--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Unique]
        public string url { get; set; }
        public byte[] data { get; set; }
        public string validity { get; set; }
        public string max_age { get; set; }
        public int type { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public Cache()
        {

        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/01/2017  Created [Fabian Sauter]
        /// </history>
        public Cache(string url, byte[] data, string validity, int max_age, int type)
        {
            this.url = url;
            this.data = data;
            this.validity = validity;
            DateTime date = DateTime.Now.AddSeconds(max_age);
            this.max_age = date.Year.ToString() + '-' + date.Month.ToString() + '-' + date.Day.ToString() + ' ' + date.Hour.ToString() + ':' + date.Minute.ToString() + ':' + date.Second.ToString();
            this.type = type;
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