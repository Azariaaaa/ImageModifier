﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaders
{
    public class PixelData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Stride { get; set; }
        public byte[] Pixels { get; set; }
    }
}
