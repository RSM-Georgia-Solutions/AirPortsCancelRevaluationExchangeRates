using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Application = SAPbouiCOM.Framework.Application;

namespace AirPortsCancelRevaluationExchangeRates
{
    [FormAttribute("AirPortsCancelRevaluationExchangeRates.CancelByDate", "CancelByDate.b1f")]
    class CancelByDate : UserFormBase
    {
        public CancelByDate()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Button1.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button1_PressedAfter);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_7").Specific));
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
            Button1.Item.Visible = false;
        }

        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.Button Button0;

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            if (!DiManager.Company.InTransaction)
            {
                DiManager.Company.StartTransaction();
            }

            if (string.IsNullOrWhiteSpace(EditText0.Value) || string.IsNullOrWhiteSpace(EditText1.Value) || string.IsNullOrWhiteSpace(EditText2.Value))
            {
                Application.SBO_Application.SetStatusBarMessage("შეავსეთ თარიღები",
                    BoMessageTime.bmt_Short, true);
                return;
            }

            Recordset recSet =
                (Recordset)DiManager.Company.GetBusinessObject(BoObjectTypes
                    .BoRecordset);
            string query =
                $"select distinct JDT1.TransId from JDT1  JOIN OJDT on JDT1.TransId = OJDT.TransId where JDT1.TransId in (SELECT JDT1.TransId FROM JDT1 WHERE JDT1.RefDate IN(SELECT   MAX(JDT1.RefDate)    GROUP BY MONTH(JDT1.RefDate), YEAR(JDT1.RefDate))  AND (autostorno = 'N')   AND (JDT1.TransId NOT IN (SELECT T0.StornoToTr FROM OJDT T0 where t0.stornototr is not NULL)) AND(Account = '8180' OR Account = '8280')  AND(ContraAct in (SELECT CardCode FROM OCRD)) AND(JDT1.RefDate >= '{DateTime.ParseExact(EditText0.Value, "yyyyMMdd", CultureInfo.InvariantCulture):s}' AND JDT1.RefDate <= '{DateTime.ParseExact(EditText1.Value, "yyyyMMdd", CultureInfo.InvariantCulture):s}')) AND (Ref3Line  LIKE N'%RC%' OR Ref3Line  LIKE N'%БО%' OR Ref3Line LIKE N'%ПР%' OR Ref3Line LIKE N'%РС%')";
            recSet.DoQuery(DiManager.QueryHanaTransalte(query));

            int count = 0;
            int totalCont = recSet.RecordCount;

            while (!recSet.EoF)
            {
                int transId = int.Parse(recSet.Fields.Item("TransId").Value.ToString());
                JournalEntries journalEntry =
                    (JournalEntries)DiManager.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
                journalEntry.GetByKey(transId);

                bool isDpm = false;

                for (int i = 0; i < journalEntry.Lines.Count; i++)
                {
                    journalEntry.Lines.SetCurrentLine(i);
                    if (journalEntry.Lines.AdditionalReference.Contains("РС"))
                    {
                        isDpm = true;
                    }
                }

                int res;

                if (isDpm)
                { 
                    res = journalEntry.Cancel();
                }
                else
                {
                    journalEntry.UseAutoStorno = BoYesNoEnum.tYES;
                    journalEntry.StornoDate =
                        DateTime.ParseExact(EditText2.Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                    res = journalEntry.Update();
                }

                if (res != 0)
                {
                    Application.SBO_Application.SetStatusBarMessage($"journal Entry : {journalEntry.Number}  {DiManager.Company.GetLastErrorDescription()}",
                        BoMessageTime.bmt_Short, true);
                    if (DiManager.Company.InTransaction)
                    {
                        DiManager.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }
                    return;
                }
                recSet.MoveNext();
                count++;
                Application.SBO_Application.StatusBar.SetSystemMessage($"{count} Out Of {totalCont}", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            if (DiManager.Company.InTransaction)
            {
                DiManager.Company.EndTransaction(BoWfTransOpt.wf_Commit);
            }

        }

        private Button Button1;

        private void Button1_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!DiManager.Company.InTransaction)
            {
                DiManager.Company.StartTransaction();
            }

            if (string.IsNullOrWhiteSpace(EditText0.Value) || string.IsNullOrWhiteSpace(EditText0.Value))
            {
                Application.SBO_Application.SetStatusBarMessage("შეავსეთ თარიღები",
                    BoMessageTime.bmt_Short, true);
                return;
            }

            Recordset recSet =
                (Recordset)DiManager.Company.GetBusinessObject(BoObjectTypes
                    .BoRecordset);
            string query =
                $"select distinct TransId from JDT1 where TransId in (SELECT TransId FROM JDT1 WHERE RefDate IN(SELECT   MAX(RefDate) FROM     JDT1 GROUP BY MONTH(RefDate), YEAR(RefDate)) AND(Account = '8180' OR Account = '8280')  AND(ContraAct in (SELECT CardCode FROM OCRD where validfor = 'Y')) AND(RefDate >= '{DateTime.ParseExact(EditText0.Value, "yyyyMMdd", CultureInfo.InvariantCulture):s}' AND RefDate <= '{DateTime.ParseExact(EditText1.Value, "yyyyMMdd", CultureInfo.InvariantCulture):s}')) AND (Ref3Line  LIKE N'%RC%' OR Ref3Line  LIKE N'%БО%' OR Ref3Line LIKE N'%ПР%' OR Ref3Line LIKE N'%РС%')";
            recSet.DoQuery(DiManager.QueryHanaTransalte(query));
            int count = 0;
            int totalCont = recSet.RecordCount;
            while (!recSet.EoF)
            {
                int transId = int.Parse(recSet.Fields.Item("TransId").Value.ToString());
                JournalEntries journalEntry =
                    (JournalEntries)DiManager.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
                journalEntry.GetByKey(transId);

                bool isDpm = false;

                for (int i = 0; i < journalEntry.Lines.Count; i++)
                {
                    journalEntry.Lines.SetCurrentLine(i);
                    if (journalEntry.Lines.AdditionalReference.Contains("РС"))
                    {
                        isDpm = true;
                    }
                }

                int res;

                if (isDpm)
                {
                    res = 0;
                }
                else
                {
                    journalEntry.UseAutoStorno = BoYesNoEnum.tNO;
                    journalEntry.StornoDate = new DateTime(2019, 01, 01);
                    res = journalEntry.Update();
                }

                if (res != 0)
                {
                    Application.SBO_Application.SetStatusBarMessage($"journal Entry : {journalEntry.Number}  {DiManager.Company.GetLastErrorDescription()}",
                        BoMessageTime.bmt_Short, true);
                    if (DiManager.Company.InTransaction)
                    {
                        DiManager.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }
                    return;
                }
                recSet.MoveNext();
                count++;
                Application.SBO_Application.StatusBar.SetSystemMessage($"{count} Out Of {totalCont}", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            if (DiManager.Company.InTransaction)
            {
                DiManager.Company.EndTransaction(BoWfTransOpt.wf_Commit);
            }
        }

        private StaticText StaticText2;
        private EditText EditText2;
    }
}
