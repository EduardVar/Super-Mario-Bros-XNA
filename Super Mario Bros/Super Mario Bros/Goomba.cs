// Author: Eduard Varshavsky
// File Name: Goomba.cs
// Project Name: Super Mario Bros
// Creation Date: October 8, 2017
// Modified Date: October 15, 2017
// Description: This class is used for affecting and manipulating goombas
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
    class Goomba : Characters
    {
        //Attributes

        //Stores the image data for death fram
        private Texture2D deathFrame;

        public Goomba(Rectangle destRec)
        {
            this.destRec = destRec;
            isGoingRight = false;

            walkFrames = 2;
            numRows = 1;
            numCols = 2;

            currentWalkFrame = 0;
            frameCount = 0;
            walkFrameRepeat = 15;

            xVelocity = -2f;
            yVelocity = .7f * SCALE_CONST / 2;
        }

        //Getters and Setters

        //Pre:  Requires to be called by the MainGame class and be provide Texture2D s for runningAnimation and inShellAnimation
        //Post: Resets y parameters for related rectangles in this object
        //Desc: Used to set the initial images and hitboxes to be later used with within the physics of this object
        public void SetGoombaImages(Texture2D runningAnimation, Texture2D deathFrame)
        {
            this.runningAnimation = runningAnimation;
            this.deathFrame = deathFrame;

            int halfGoombaX = (runningAnimation.Width / numCols) / 2;
            int halfGoombaY = (runningAnimation.Height / numRows) / 2;
            int headSize = 32;
            int halfHead = headSize / 2;

            hitBoxes[0] = new Rectangle(GetDest().X, GetDest().Y, halfGoombaX * 2, halfHead / 2);
            hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfGoombaX + halfHead, halfHead / 2);
            hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2, halfGoombaX, GetDest().Height - headSize / 2);
            hitBoxes[3] = new Rectangle(GetDest().X + halfGoombaX, GetDest().Y + halfHead / 2, halfGoombaX, GetDest().Height - headSize / 2);

            walkFrameWidth = runningAnimation.Width / numCols;
            walkFrameHeight = runningAnimation.Height / numRows;
            srcRec = new Rectangle(GetSrcX(currentWalkFrame, walkFrameWidth, numCols), GetSrcY(currentWalkFrame, walkFrameHeight, numRows), walkFrameWidth, walkFrameHeight);
        }

        public new void SetCharacterY(int yPoint)
        {
            destRec.Y = yPoint;

            for (int i = 0; i < hitBoxes.Length; i++)
            {
                int halfCharacterX = (runningAnimation.Width / numCols) / 2;
                int halfCharacterY = (runningAnimation.Height / numRows) / 2;
                int headSize = 32;
                int halfHead = headSize / 2;

                hitBoxes[0] = new Rectangle(GetDest().X + 3, GetDest().Y, halfCharacterX + halfHead * 2 - 6, halfHead * 2);
                hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfCharacterX + halfHead, halfHead / 2);
                hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead, halfCharacterX, GetDest().Height - headSize + 8);
                hitBoxes[3] = new Rectangle(GetDest().X + halfCharacterX, GetDest().Y + halfHead, halfCharacterX, GetDest().Height - headSize + 8);
            }
        }

        public Texture2D GetDeathImg()
        {
            return deathFrame;
        }

        //Behaviour

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles downwards
        //Desc: Used to animate goomba based on the action performed in game, based off a source rectangles using
        //      a provided sprite sheet
        public void AnimateGoomba()
        {
            //Recalculate the X, Y of the cookie cutter source rectangle based on the current frame number
            srcRec.X = GetSrcX(currentWalkFrame, walkFrameWidth, numCols);
            srcRec.Y = GetSrcX(currentWalkFrame, walkFrameHeight, numRows);

            //increment the number of frames passed since the last frame update
            frameCount++;

            //If enough updates have passed, update the frame number
            if (frameCount >= walkFrameRepeat)
            {
                //Increment the frame number
                currentWalkFrame = currentWalkFrame + 1;

                //If we reach the end of the animation, reset it back to 0 to loop the animation
                if (currentWalkFrame >= 4)
                {
                    currentWalkFrame = 2;
                }

                //reset the number of frames passed since the alst update back to 0
                frameCount = 0;
            }
        }
    }
}
