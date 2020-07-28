using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RatesRevaluation.Initialization;
using SAPbouiCOM.Framework;

namespace RatesRevaluation
{
    [FormAttribute("RatesRevaluation.ListOfAccounts", "Forms/ListOfAccounts.b1f")]
    class ListOfAccounts : UserFormBase
    {
        private readonly Settings _exciseParams;
        private readonly string _AccName;
        public ListOfAccounts(Settings exciseParams, string accName)
        {
            _exciseParams = exciseParams;
            _AccName = accName;
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.EditText0.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.EditText0_KeyDownAfter);
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_2").Specific));
            this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            Grid0.DataTable.ExecuteQuery(DiManager.QueryHanaTransalte($"SELECT AcctCode, AcctName FROM OACT WHERE Postable = 'Y'"));
        }

        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.Grid Grid0;

        private void EditText0_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            Grid0.DataTable.ExecuteQuery(DiManager.QueryHanaTransalte($"SELECT AcctCode, AcctName FROM OACT WHERE Postable = 'Y' AND (AcctCode Like N'%{EditText0.Value}%' OR AcctName Like N'%{EditText0.Value}%')"));
        }

        private void Grid0_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.Row == -1)
            {
                return;
            }
            string acc = Grid0.DataTable.GetValue("AcctCode", Grid0.GetDataTableRowIndex(pVal.Row)).ToString();

            if (_AccName == "GainAccount")
            {
                _exciseParams.GainAccount = acc;
                _exciseParams.FillCflGainAccount();
            }
            else if (_AccName == "LossAccount")
            {
                _exciseParams.LossAccount = acc;
                _exciseParams.FillCflLossAccount();
            }
            UIAPIRawForm.Close();
        }
    }
}
