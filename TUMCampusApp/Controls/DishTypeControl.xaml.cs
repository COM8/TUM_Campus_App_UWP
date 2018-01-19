﻿using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class DishTypeControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string dishType;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2018 Created [Fabian Sauter]
        /// </history>
        public DishTypeControl(CanteenDishTable dish)
        {
            this.dishType = dish.dish_type;
            this.InitializeComponent();
            addDish(dish);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Adds the given dish to the dishes_stckp.
        /// </summary>
        /// <param name="dish">The dish, that should get added.</param>
        public void addDish(CanteenDishTable dish)
        {
            if (Equals(dishType, dish.dish_type))
            {
                dishes_stckp.Children.Add(new CanteenDishControl(dish));
            }
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            dishType_tbx.Text = Utillities.translateDishType(dishType) + ':';
        }

        #endregion

    }
}
