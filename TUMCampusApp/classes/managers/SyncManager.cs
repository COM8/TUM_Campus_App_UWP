﻿using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Syncs;

namespace TUMCampusApp.Classes.Managers
{
    class SyncManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static SyncManager INSTANCE;
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public SyncManager()
        {
            dB.CreateTable<Sync>();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the current unix time in ms.
        /// </summary>
        /// <returns>Returns the current unix time in ms.</returns>
        public static long GetCurrentUnixTimestampMillis()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Converts the given ms from unix time to a DateTime obj and returns it.
        /// </summary>
        /// <param name="millis">Unix time in ms.</param>
        /// <returns>Returns the DateTime obj equal to the given ms.</returns>
        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis);
        }

        /// <summary>
        /// Returns the current unix time in s.
        /// </summary>
        /// <returns>Returns the current unix time in s.</returns>
        public static long GetCurrentUnixTimestampSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Converts the given seconds from unix time to a DateTime obj and returns it.
        /// </summary>
        /// <param name="seconds">Unix time in seconds.</param>
        /// <returns>Returns the DateTime obj equal to the given seconds.</returns>
        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Checks if a new sync is needed or if data is up-to-date
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public bool needSync(Object obj, int seconds)
        {
            return needSync(obj.GetType().Name, seconds);
        }

        /// <summary>
        /// Checks if a new sync is needed or if data is up-to-date
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public bool needSync(string id, int seconds)
        {
            try
            {
                List<Sync> list = dB.Query<Sync>("SELECT lastSync FROM Sync WHERE id = ?", id);
                if(list == null || list.Count <= 0)
                {
                    return true;
                }
                foreach (Sync s in list)
                {
                    if(s.lastSync + seconds < GetCurrentUnixTimestampSeconds())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (SQLiteException e)
            {
                Logger.Error("Unable to execute a querry for checking if the given object needs to sync again", e);
                return true;
            }
        }

        /// <summary>
        /// Replace or Insert a successful sync event in the database
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public void replaceIntoDb(Sync s)
        {
            dB.InsertOrReplace(s);
        }

        /// <summary>
        /// Removes all items from database
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public void deleteFromDb()
        {
            dB.DropTable<Sync>();
            dB.CreateTable<Sync>();
        }

        public async override Task InitManagerAsync()
        {
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
