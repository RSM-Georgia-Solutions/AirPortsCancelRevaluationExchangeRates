using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace RatesRevaluation.Initialization
{
    class CreateFields : IRunnable
    {
        public void Run(DiManager diManager)
        {
            diManager.AddField("RSM_REVAL_SETTINGS", "GainAccount", "მოგების ანგარიში", BoFieldTypes.db_Alpha, 50, false);
            diManager.AddField("RSM_REVAL_SETTINGS", "LossAccount", "ზარალის ანგარიში", BoFieldTypes.db_Alpha, 50, false);
        }
    }
}
