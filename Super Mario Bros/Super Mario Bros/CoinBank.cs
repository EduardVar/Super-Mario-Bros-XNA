// Author: Eduard Varshavsky
// File Name: CoinBank.cs
// Project Name: Super Mario Bros
// Creation Date: October 13, 2017
// Modified Date: October 16, 2017
// Description: This class is used for affecting and manipulating CoinBank blocks
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
    class CoinBank : Blocks
    {
        //Attributes

        //Created ints to store timer data
        private int coinBankTimer;
        private int coinTimerMax = 300;

        //Stores if timer is running
        private bool isTimerRunning;

        //Stores the original location and dimensions of the coin inside the block
        private Rectangle coinOriginalDest;

        //Constructor

        public CoinBank(Rectangle destRec)
        {
            //Sets the destination rectangle to the one provided
            this.destRec = destRec;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets Texture bool inside this object to the one provided and do dimension calculations for certain aspects of it
        //      and sets up a new srcRec for the animating this block
        //Desc: Used to set a isDying bool for when the player is potentially dying
        public void SetInitialBankImage(Texture2D initialImage)
        {
            //Applies the image to the one stored inside here and does calculations for dimensions in this object
            this.initialImage = initialImage;
            originalDestRec = destRec;
            coinOriginalDest = originalDestRec;
            blockFrameWidth = initialImage.Width / numCols;
            blockFrameHeight = initialImage.Height / numRows;

            //Sets the srcRec with the provided values
            srcRec = new Rectangle(GetSrcX(currentBlockFrame, blockFrameWidth, numCols), GetSrcY(currentBlockFrame, blockFrameHeight, numRows), blockFrameWidth, blockFrameHeight);
        }

        public void ApplyCoinBankGravity()
        {
            isTimerRunning = true;

            if (coinBankTimer >= coinTimerMax)
            {
                isDisabled = true;
            }
            
            yVelocity += COIN_GRAVITY;
            coinDest.Y += (int)yVelocity;

            if (yVelocity >= 12f)
            {
                isCoinVisible = false;
                ResetYVelocity();
                coinDest.Y = originalDestRec.Y;
            }
        }

        public void ResetEverything()
        {
            destRec = originalDestRec;
            ResetYVelocity();
            coinDest.Y = originalDestRec.Y;
        }

        public bool GetIsDisabled()
        {
            return isDisabled;
        }

        public bool GetTimerRunning()
        {
            return isTimerRunning;
        }

        public void IncrimentTimer()
        {
            coinBankTimer++;
        }
    }
}
