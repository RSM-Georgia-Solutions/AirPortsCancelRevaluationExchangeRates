using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPortsCancelRevaluationExchangeRates.Initialization
{
    class CreateTables : IRunnable
    {
        public void Run(DiManager diManager)
        {
            diManager.CreateTable("RSM_REVAL_SETTINGS", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
        }
    }
}
