// Author: Eduard Varshavsky
// File Name: CoinBlock.cs
// Project Name: Super Mario Bros
// Creation Date: October 12, 2017
// Modified Date: October 16, 2017
// Description: This class is used for affecting and manipulating CoinBlocks blocks
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
    class CoinBlock : Blocks
    {
        //Constructor
        public CoinBlock(Rectangle destRec)
        {
            //Sets the destination rectangle to the one provided
            this.destRec = destRec;
        }

        //Getter

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool GetIsHit and its associated value
        //Desc: Used to provide a isHit bool for when the block is potentially hit
        public bool GetIsHit()
        {
            return isHit;
        }
    }
}
