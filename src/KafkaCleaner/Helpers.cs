using System;

namespace KafkaCleanerApp
{
    public static class Helpers
    {
        /// <summary>
        /// Uncheck all items in <see cref="UncheckAllItems(System.Windows.Forms.CheckedListBox)."/>
        /// </summary>
        public static void UncheckAllItems(this System.Windows.Forms.CheckedListBox clb)
        {
            if (clb is null)
            {
                throw new ArgumentNullException(nameof(clb));
            }

            while (clb.CheckedIndices.Count > 0)
            {
                clb.SetItemChecked(clb.CheckedIndices[0], false);
            }
        }
    }
}
