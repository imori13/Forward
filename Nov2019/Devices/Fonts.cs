using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    class Fonts
    {
        public static SpriteFont Font12_32 { get; private set; }
        public static SpriteFont Font10_128 { get; private set; }
        public static SpriteFont Font10_256 { get; private set; }
        public static SpriteFont FontCica_32 { get; private set; }

        public static void LoadFonts(ContentManager content)
        {
            Font12_32 = content.Load<SpriteFont>("Fonts/pixelMplus12_size32");
            Font10_128 = content.Load<SpriteFont>("Fonts/pixelMplus10_size128");
            Font10_256 = content.Load<SpriteFont>("Fonts/pixelMplus10_size256");
            FontCica_32 = content.Load<SpriteFont>("Fonts/cica_size32");
        }
    }
}
