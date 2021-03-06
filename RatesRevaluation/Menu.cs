﻿using System;
using System.Collections.Generic;
using System.Text;
using RatesRevaluation.Initialization;
using SAPbouiCOM.Framework;

namespace RatesRevaluation
{
    class Menu
    {
        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "RatesRevaluation";
            oCreationPackage.String = "Rates Revaluation";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;
            oCreationPackage.Image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Media\\logo.png";



            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("RatesRevaluation");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "RatesRevaluation.Settings";
                oCreationPackage.String = "Settings";
                oMenus.AddEx(oCreationPackage);
                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "RatesRevaluation.Initial";
                oCreationPackage.String = "Initialization";
                oMenus.AddEx(oCreationPackage);
                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "RatesRevaluation.CancelByDate";
                oCreationPackage.String = "Reverse";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "RatesRevaluation.CancelByDate")
                {
                    CancelByDate activeForm = new CancelByDate();
                    activeForm.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "RatesRevaluation.Initial")
                {
                    Initialization.Initialization activeForm = new Initialization.Initialization();
                    activeForm.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "RatesRevaluation.Settings")
                {
                    Settings activeForm = new Settings();
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
