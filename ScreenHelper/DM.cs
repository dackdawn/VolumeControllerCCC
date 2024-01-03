using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenHelper
{
    public partial class ScreenHelper
    {
        /// <summary>
        /// Selects duplex or double-sided printing for printers capable of duplex printing.
        /// </summary>
        private enum DMDUP : short
        {
            /// <summary>
            /// Unknown setting.
            /// </summary>
            DMDUP_UNKNOWN = 0,

            /// <summary>
            /// Normal (nonduplex) printing.
            /// </summary>
            DMDUP_SIMPLEX = 1,

            /// <summary>
            /// Long-edge binding, that is, the long edge of the page is vertical.
            /// </summary>
            DMDUP_VERTICAL = 2,

            /// <summary>
            /// Short-edge binding, that is, the long edge of the page is horizontal.
            /// </summary>
            DMDUP_HORIZONTAL = 3,
        }
    }
}
