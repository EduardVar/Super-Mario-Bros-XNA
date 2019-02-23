// Author: Eduard Varshavsky
// File Name: Brick.cs
// Project Name: Super Mario Bros
// Creation Date: October 12, 2017
// Modified Date: October 16, 2017
// Description: This class is used for affecting and manipulating Brick blocks
// code

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Super_Mario_Bros
{
    class Brick : Blocks
    {
        //Constructor

        public Brick(Rectangle destRec)
        {
            //Sets the destination rectangle based on the parameter provided
            this.destRec = destRec;
        }
    }
}
