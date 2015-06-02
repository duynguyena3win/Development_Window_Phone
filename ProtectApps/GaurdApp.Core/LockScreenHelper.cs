using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GaurdApp.Core
{
    public class LockScreenHelper
    {
        public LockScreenHelper()
        {

        }

        public static async Task HelloWP()
        {
            Debug.WriteLine("Work at: " + DateTime.Now.ToLongTimeString() );
        }
    }
}
