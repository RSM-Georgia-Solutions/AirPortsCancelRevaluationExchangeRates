using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace RatesRevaluation.Initialization
{
    [FormAttribute("RatesRevaluation.Initialization.Settings", "Forms/Settings.b1f")]
    class Settings : UserFormBase
    {
        public Settings()
        {
        }

        public Form _paramsForm { get; set; }
        public void FillCflGainAccount()
        {
            _paramsForm.DataSources.UserDataSources.Item("UD_0").ValueEx = GainAccount;
        }
        public void FillCflLossAccount()
        {
            _paramsForm.DataSources.UserDataSources.Item("UD_1").ValueEx = LossAccount;
        }

        public string GainAccount { get; set; }
        public string LossAccount { get; set; }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.EditText0.ChooseFromListBefore += new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.EditText0_ChooseFromListBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.EditText1.ChooseFromListBefore += new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.EditText1_ChooseFromListBefore);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.VisibleAfter += new VisibleAfterHandler(this.Form_VisibleAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            StaticText0.Item.FontSize = 10;
            StaticText1.Item.FontSize = 10;
            Button0.Item.FontSize = 10;
        }

        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.Button Button0;

        private void Form_VisibleAfter(SBOItemEventArg pVal)
        {
            if (SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Title == "Settings")
            {
                _paramsForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
                Recordset recSet = (Recordset)DiManager.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recSet.DoQuery(DiManager.QueryHanaTransalte($"Select * From [@RSM_REVAL_SETTINGS]"));
                if (!recSet.EoF)
                {
                    GainAccount = recSet.Fields.Item("U_GainAccount").Value.ToString();
                    LossAccount = recSet.Fields.Item("U_LossAccount").Value.ToString();

                    _paramsForm.DataSources.UserDataSources.Item("UD_0").ValueEx = GainAccount;
                    _paramsForm.DataSources.UserDataSources.Item("UD_1").ValueEx = LossAccount;
                }
            }
        }

        private void EditText0_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = false;
            ListOfAccounts list = new ListOfAccounts(this, "GainAccount");
            list.Show();
        }

        private void EditText1_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = false;
            ListOfAccounts list = new ListOfAccounts(this, "LossAccount");
            list.Show();
        }

        private void Button0_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            Recordset recSet = (Recordset)DiManager.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            recSet.DoQuery(DiManager.QueryHanaTransalte($"Select * From [@RSM_REVAL_SETTINGS]"));
            if (recSet.EoF)
            {
                recSet.DoQuery(DiManager.QueryHanaTransalte($"INSERT INTO  [@RSM_REVAL_SETTINGS] (U_GainAccount, U_LossAccount) VALUES (N'{GainAccount}',N'{LossAccount}')"));
            }
            else
            {
                recSet.DoQuery(DiManager.QueryHanaTransalte($"UPDATE [@RSM_REVAL_SETTINGS] SET U_GainAccount = N'{GainAccount}', U_LossAccount = N'{LossAccount}'"));
            }

            SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetSystemMessage("Success", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
        }
    }
}
