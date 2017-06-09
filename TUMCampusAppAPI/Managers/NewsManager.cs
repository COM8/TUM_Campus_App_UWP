﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Syncs;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.UserDatas;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml.Media.Imaging;

namespace TUMCampusAppAPI.Managers
{
    public class NewsManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static NewsManager INSTANCE;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public NewsManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--       
        /// <summary>
        /// Returns the most current news id from the db.
        /// </summary>
        private string getLastNewsId()
        {
            string lastId = "";
            List<News.News> list = dB.Query<News.News>("SELECT id FROM News ORDER BY id DESC LIMIT 1");
            if(list != null && list.Count > 0)
            {
                lastId += list[0].id;
            }
            return lastId;
        }

        /// <summary>
        /// Returns all news sources from the db.
        /// </summary>
        /// <returns>A list of NewsSource elements.</returns>
        public List<News.NewsSource> getAllNewsSourcesFormDb()
        {
            return dB.Query<News.NewsSource>("SELECT * FROM NewsSource");
        }

        /// <summary>
        /// Returns all news from the db in descending order by date.
        /// Also only returns these, where the news source is not disabled.
        /// </summary>
        /// <returns>A list of News elements.</returns>
        public List<News.News> getAllNewsFormDb()
        {
            return dB.Query<News.News>("SELECT * FROM News n, NewsSource s WHERE n.src LIKE s.src AND s.enabled=1 ORDER BY date DESC");
        }

        public News.NewsSource getNewsSource(string id)
        {
            List<News.NewsSource> sources = dB.Query<News.NewsSource>("SELECT * FROM NewsSource WHERE src LIKE ?", id);
            return sources.Count > 0 ? sources[0] : null;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<News.News>();
            dB.CreateTable<News.NewsSource>();
        }

        /// <summary>
        /// Downloads the image from the given source if needed and caches it.
        /// </summary>
        /// <param name="src">The image source.</param>
        public async Task<BitmapImage> downloadNewsImage(string src)
        {
            return await NetUtils.downloadImageAsync(new Uri(src));
        }

        /// <summary>
        /// Trys to download news if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all news.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadNewsAsync(bool force)
        {
            if (!DeviceInfo.isConnectedToWifi() || !force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING))
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync("News", CacheManager.VALIDITY_ONE_DAY).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
            {
                try
                {
                    JsonArray jsonArray = await NetUtils.downloadJsonArrayAsync(new Uri(Const.NEWS_URL + getLastNewsId()));
                    if (jsonArray == null)
                    {
                        return;
                    }

                    List<News.News> list = new List<News.News>();
                    foreach (JsonValue val in jsonArray)
                    {
                        try
                        {
                            list.Add(new News.News(val.GetObject()));
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Caught an exception during parsing news!", e);
                        }
                    }
                    cleanupDb();
                    dB.DeleteAll<News.News>();
                    dB.InsertOrReplaceAll(list);
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("News"));
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("News", SyncResult.STATUS_ERROR_UNKNOWN, e.ToString()));
                }
            }
        }

        /// <summary>
        /// Trys to download news sources if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all news sources.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadNewsSourcesAsync(bool force)
        {
            if (!DeviceInfo.isConnectedToWifi() || !force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING))
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync("NewsSource", CacheManager.VALIDITY_ONE_MONTH).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
            {
                try
                {
                    JsonArray jsonArray = await NetUtils.downloadJsonArrayAsync(new Uri(Const.NEWS_SOURCES_URL));
                    if (jsonArray == null)
                    {
                        return;
                    }

                    List<News.NewsSource> list = new List<News.NewsSource>();
                    foreach (JsonValue val in jsonArray)
                    {
                        try
                        {
                            list.Add(new News.NewsSource(val.GetObject()));
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Caught an exception during parsing news sources!", e);
                        }
                    }
                    dB.DeleteAll<News.NewsSource>();
                    dB.InsertAll(list);
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("NewsSource"));
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("NewsSource", SyncResult.STATUS_ERROR_UNKNOWN, e.ToString()));
                }
            }
        }

        /// <summary>
        /// Updates the disabled status for a specific news source.
        /// </summary>
        /// <param name="id">The id of that specific news source.</param>
        /// <param name="enabled">Whether to enable it.</param>
        public void updateNewsSourceStatus(int id, bool enabled)
        {
            dB.Execute("UPDATE NewsSource SET enabled = ? WHERE id = ?", new object[] {enabled, id});
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Removes all old items (older than 3 months) from the db.
        /// </summary>
        private void cleanupDb()
        {
            dB.Execute("DELETE FROM News WHERE date < date('now','-3 month')");
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
