using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenHelper
{
    public partial class ScreenHelper
    {
        public enum ScreenOrientation
        {
            /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle0"]/*' />
            /// <devdoc>
            ///    <para>
            ///       The screen is oriented at 0 degrees
            ///    </para>
            /// </devdoc>
            Angle0 = 0,

            /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle90"]/*' />
            /// <devdoc>
            ///    <para>
            ///       The screen is oriented at 90 degrees
            ///    </para>
            /// </devdoc>
            Angle90 = 1,

            /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle180"]/*' />
            /// <devdoc>
            ///    <para>
            ///       The screen is oriented at 180 degrees.
            ///    </para>
            /// </devdoc>
            Angle180 = 2,

            /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle270"]/*' />
            /// <devdoc>
            ///    <para>
            ///       The screen is oriented at 270 degrees.
            ///    </para>
            /// </devdoc>
            Angle270 = 3,
        }
    }
}
