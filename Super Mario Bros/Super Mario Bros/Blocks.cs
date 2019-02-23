// Author: Eduard Varshavsky
// File Name: Blocks.cs
// Project Name: Super Mario Bros
// Creation Date: October 12, 2017
// Modified Date: October 16, 2017
// Description: This class is used as a a base class for all block types
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
    class Blocks
    {
        //Attributes

        //Store the initial image data for the block and coin
        protected Texture2D initialImage;
        protected Texture2D coinImage;

        //Stores the location and dimensions of all blocks related rectangles
        protected Rectangle coinDest;
        protected Rectangle destRec;
        protected Rectangle originalDestRec;
        protected Rectangle srcRec;
        protected Rectangle coinSrc;

        //Stores bools considering block states
        protected bool isBouncing;
        protected bool isHit;
        protected bool isCoinVisible;
        protected bool isDisabled;

        //Stores current yVelocity and constants  for the physics
        protected float yVelocity = 0f;
        protected const float INITIAL_BOUNCE = 8f;
        protected const float BOUNCEE_GRAVITY = 1.4f;
        protected const float COIN_GRAVITY = 0.7f;

        //Stores animation data for block related animations
        protected int blockFrames = 4;
        protected int blockFrameWidth;
        protected int blockFrameHeight;
        protected int currentBlockFrame = 0;
        protected int frameCount = 0;
        protected float blockFrameRepeat = 10;

        //Stores animation data for coin related animations
        protected int currentCoinFrame;
        protected int coinFrameCount;
        protected int coinFrameRepeat = 4;
        protected int coinWidth = 32;
        protected int coinHeight = 56;

        //Stores dimensions of animation sprite sheet
        protected int numRows = 1;
        protected int numCols = 4;

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Rectagle of the asked for rectangle
        //Desc: Used to provide the destination rectangle for location and dimension calculations
        public Rectangle GetDest()
        {
            return destRec;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Rectagle of the asked for rectangle
        //Desc: Used to provide the source rectangle for sprite sheet based animations
        public Rectangle GetSrc()
        {
            return srcRec;
        }

        public void SetInitialImage(Texture2D initialImage)
        {
            this.initialImage = initialImage;
            originalDestRec = destRec;
            blockFrameWidth = initialImage.Width / numCols;
            blockFrameHeight = initialImage.Height / numRows;
            srcRec = new Rectangle(GetSrcX(currentBlockFrame, blockFrameWidth, numCols), GetSrcY(currentBlockFrame, blockFrameHeight, numRows), blockFrameWidth, blockFrameHeight);
        }

        public Texture2D GetInitialImage()
        {
            return initialImage;
        }

        public Rectangle GetOriginalDest()
        {
            return originalDestRec;
        }

        public void SetDestRec(Rectangle newDest)
        {
            destRec = newDest;
        }

        public bool GetIsBouncing()
        {
            return isBouncing;
        }

        public void SetCoinImage(Texture2D coinImage)
        {
            this.coinImage = coinImage;
            coinSrc = new Rectangle(GetSrcX(currentCoinFrame, coinWidth, numCols), GetSrcY(currentCoinFrame, coinHeight, numRows), coinWidth, coinHeight);
            coinDest = new Rectangle(destRec.X + 16, destRec.Y + 4, coinWidth, coinHeight);
        }

        public Rectangle getCoinDest()
        {
            return coinDest;
        }

        public Rectangle getCoinSrc()
        {
            return coinSrc;
        }

        public Texture2D GetCoinImg()
        {
            return coinImage;
        }

        public bool GetIsCoinVisible()
        {
            return isCoinVisible;
        }

        public void BeginBounce()
        {
            isBouncing = true;
            isCoinVisible = true;
            yVelocity += -INITIAL_BOUNCE;
            destRec.Y += (int)yVelocity;
            coinDest.Y += (int)yVelocity * 20;
            isHit = true;
        }

        public void ResetYVelocity()
        {
            isBouncing = false;
            yVelocity = 0f;
        }

        public void ApplyGravity()
        {
            yVelocity += BOUNCEE_GRAVITY;
            destRec.Y += (int)yVelocity;
            coinDest.Y += -(int)INITIAL_BOUNCE;
        }

        public void ApplyCoinGravity()
        {
            yVelocity += COIN_GRAVITY;
            coinDest.Y += (int)yVelocity;

            if (yVelocity >= 12f)
            {
                isCoinVisible = false;
                ResetYVelocity();
            }
        }

        public void AnimateCoin()
        {
            //Recalculate the X, Y of the cookie cutter source rectangle based on the current frame number
            coinSrc.X = GetSrcX(currentCoinFrame, coinWidth, numCols);
            coinSrc.Y = GetSrcX(currentCoinFrame, coinHeight, numRows);

            //increment the number of frames passed since the last frame update
            coinFrameCount++;

            //If enough updates have passed, update the frame number
            if (coinFrameCount >= coinFrameRepeat)
            {
                //Increment the frame number
                currentCoinFrame = currentCoinFrame + 1;

                //If we reach the end of the animation, reset it back to 0 to loop the animation
                if (currentCoinFrame >= 4)
                {
                    currentCoinFrame = 0;
                }

                //reset the number of frames passed since the alst update back to 0
                coinFrameCount = 0;
            }
        }

        public void AnimateBlock()
        {
            //Recalculate the X, Y of the cookie cutter source rectangle based on the current frame number
            srcRec.X = GetSrcX(currentBlockFrame, blockFrameWidth, numCols);
            srcRec.Y = GetSrcX(currentBlockFrame, blockFrameHeight, numRows);

            //increment the number of frames passed since the last frame update
            frameCount++;

            //If enough updates have passed, update the frame number
            if (frameCount >= blockFrameRepeat)
            {
                //Increment the frame number
                currentBlockFrame = currentBlockFrame + 1;

                //If we reach the end of the animation, reset it back to 0 to loop the animation
                if (currentBlockFrame >= 4)
                {
                    currentBlockFrame = 0;
                }

                //reset the number of frames passed since the alst update back to 0
                frameCount = 0;
            }
        }

        //Pre: frameNum is the current frame the animation is on
        //     frameW is the pixel width of one frame in the source image
        //     numCols is the number of columns wide the animation grid is
        //Post: Return the x cooridinate on the source image of the current animation frame
        //Desc: Calculate the X coordinate of the current frame animation on the
        //      source image
        public int GetSrcX(int frameNum, int frameW, int numCols)
        {
            int result = 0;

            //Calculate which column the animation frame is in within the animation grid
            int col = (frameNum % numCols);

            //Calculate the x by multiplying the column by the width of one frame
            result = frameW * col;

            return result;
        }

        //Pre: frameNum is the current frame the animation is on
        //     frameH is the pixel height of one frame in the source image
        //     numCols is the number of columns wide the animation grid is
        //Post: Return the y cooridinate on the source image of the current animation frame
        //Desc: Calculate the y coordinate of the current frame animation on the
        //      source image
        public int GetSrcY(int frameNum, int frameH, int numCols)
        {
            int result = 0;

            //Calculate which row the animation frame is in within the animation grid
            int row = (frameNum / numCols);

            //Calculate the y by multiplying the row by the height of one frame
            result = frameH * row;

            return result;
        }
    }
}
