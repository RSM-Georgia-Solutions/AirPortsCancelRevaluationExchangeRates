using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesRevaluation.Initialization
{
    public class Initial : IRunnable
    {
        public void Run(DiManager diManager)
        {
            IEnumerable<IRunnable> objects = new List<IRunnable>()
            {
                new CreateTables(),
                new CreateFields()
            };
            foreach (IRunnable item in objects)
            {
                item.Run(diManager);
            }
        }
    }
}
