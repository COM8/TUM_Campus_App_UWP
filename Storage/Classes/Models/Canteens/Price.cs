﻿using System.ComponentModel.DataAnnotations;

namespace Storage.Classes.Models.Canteens
{
    public class Price
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Key]
        public int PriceId { get; set; }
        public string BasePrice { get; set; }
        public string PerUnit { get; set; }
        public string Unit { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override string ToString()
        {
            if (PerUnit is null || Unit is null)
            {
                return BasePrice + '€';
            }

            if (double.TryParse(BasePrice, out double bp) && bp <= 0)
            {
                return PerUnit + '/' + Unit + '€';
            }
            return BasePrice + "€ + " + PerUnit + '/' + Unit + '€';
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
