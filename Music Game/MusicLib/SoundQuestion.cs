using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicLib
{
    public class SoundQuestion : AbstractQuestion
    {
        public SoundQuestion()
        {

        }
        
        public override string GetType()
        {
            return "Sound";
        }
    }
}
