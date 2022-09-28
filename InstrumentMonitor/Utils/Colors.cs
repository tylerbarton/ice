using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentMonitor.Utils
{
    /// <summary>
    /// Standard colors used in the application.
    /// </summary>
    internal class Colors
    {
        public static readonly Color NEGATIVE_RED = Color.FromArgb(255, 52, 42);
        public static readonly Color POSITIVE_GREEN = Color.FromArgb(46, 192, 79);
        public static readonly Color BACKROUND_GRAY = Color.FromArgb(28, 28, 28);
        public static readonly Color FOREGROUND_GRAY = Color.FromArgb(40, 36, 41);
        public static readonly Color SELECTED_GRAY = Color.FromArgb(52, 50, 56);
    }
}
