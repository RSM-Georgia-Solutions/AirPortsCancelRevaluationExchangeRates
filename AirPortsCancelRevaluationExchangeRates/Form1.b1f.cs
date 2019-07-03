using System;
using System.Collections.Generic;
using System.Xml;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Application = SAPbouiCOM.Framework.Application;

namespace AirPortsCancelRevaluationExchangeRates
{
    [FormAttribute("AirPortsCancelRevaluationExchangeRates.Form1", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        public Form1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            //this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Button Button0;

        private void OnCustomInitialize()
        {

        }

        //private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        //{
        //    Recordset recSet =
        //        (Recordset)DiManager.Company.GetBusinessObject(BoObjectTypes
        //            .BoRecordset);
        //    recSet.DoQuery(DiManager.QueryHanaTransalte($" select TransId from JDT1 where TransId in (SELECT TransId FROM JDT1 WHERE RefDate IN(SELECT   MAX(RefDate) FROM     JDT1 GROUP BY MONTH(RefDate), YEAR(RefDate)) AND(Account = '8180' OR Account = '8280')  AND(ContraAct in (SELECT CardCode FROM OCRD)) AND(RefDate >= '2017-10-31 00:00:00.000')) AND Ref3Line  LIKE N'%RC%' OR Ref3Line  LIKE N'%БО%' OR Ref3Line LIKE N'%ПР%' OR Ref3Line LIKE N'%РС%'"));

        //    while (recSet.EoF)
        //    {
        //        int transId = int.Parse(recSet.Fields.Item("TransId").Value.ToString());
        //        JournalEntries journalEntry =
        //            (JournalEntries)DiManager.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
        //        journalEntry.GetByKey(transId);

        //        bool isDpm = false;

        //        for (int i = 0; i < journalEntry.Lines.Count; i++)
        //        {
        //            journalEntry.SetCurrentLine(i);
        //            if (journalEntry.Lines.AdditionalReference.Contains("РС"))
        //            {
        //                isDpm = true;
        //            }
        //        }

        //        int res;

        //        if (isDpm)
        //        {
        //            journalEntry.UseAutoStorno = BoYesNoEnum.tYES;
        //            journalEntry.StornoDate = journalEntry.TaxDate;
        //            res = journalEntry.Update();
        //        }
        //        else
        //        {
        //            journalEntry.UseAutoStorno = BoYesNoEnum.tYES;
        //            journalEntry.StornoDate = new DateTime(2019, 01, 01);
        //            res = journalEntry.Update();
        //        }

        //        if (res != 0)
        //        {
        //            Application.SBO_Application.SetStatusBarMessage($"journal Entry : {journalEntry.Number}  {DiManager.Company.GetLastErrorDescription()}",
        //                BoMessageTime.bmt_Short, true);
        //        }

        //        recSet.MoveNext();
        //    }

        //}
    }
}