using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaCleanerApp
{
    public static class FormHelpers
    {
        public static void UncheckAllItems(this System.Windows.Forms.CheckedListBox clb)
        {
            while (clb.CheckedIndices.Count > 0)
                clb.SetItemChecked(clb.CheckedIndices[0], false);
        }
    }
}
