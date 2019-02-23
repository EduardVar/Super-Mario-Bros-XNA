// Author: Eduard Varshavsky
// File Name: Character.cs
// Project Name: Super Mario Bros
// Creation Date: October 8, 2017
// Modified Date: October 15, 2017
// Description: This class is used as a base class for playale and non-playable characters
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
    class Characters
    {
        //Stores the running animation of the character
        protected Texture2D runningAnimation;

        //Stores the timers for death animations
        protected int deathTimer;
        protected int dTimerMax = 36;

        //Stores the max bounce height
        protected int bounceMax = 12;

        //Stores the animation frame data dimensions
        protected int walkFrames = 4;
        protected int walkFrameWidth;
        protected int walkFrameHeight;
        protected int numRows = 1;
        protected int numCols = 4;
        protected int currentWalkFrame = 0;
        protected int frameCount = 0;
        protected float walkFrameRepeat = 5;

        //Stores source and destination rectangles
        protected Rectangle srcRec;
        protected Rectangle destRec;

        //Stores current x and y velocities as well as constants for physics
        protected float xVelocity;
        protected float yVelocity;
        protected const int SCALE_CONST = 4;
        protected const float NEW_GRAVITY_MID = .6f * SCALE_CONST / 2;
        protected const float DEATH_JUMP = -16f;

        //Stores true/false data for state bools
        protected bool isGoingRight;
        protected bool isDead;
        protected bool isAlternateDead;

        //Made array of rectangles for hitboxes (0 = head, 1 = legs, 2 = left, 3 = right)
        protected Rectangle[] hitBoxes = new Rectangle[4];

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Rectagle of the asked for rectangele
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

        public Texture2D GetWalkImg()
        {
            return runningAnimation;
        }

        public int GetInterception(Rectangle potentialCollision)
        {
            if (potentialCollision.Intersects(destRec))
            {
                for (int i = 0; i < hitBoxes.Length; i++)
                { 
                    if (hitBoxes[i].Intersects(potentialCollision))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public void MoveXConstantly()
        {
            destRec.X += (int)xVelocity;

            SetHitXPos();
        }

        public void MoveYGravity()
        {
            yVelocity += NEW_GRAVITY_MID;

            destRec.Y += (int)yVelocity;

            SetHitYPos();
        }

        public void SetCharacterX(int xPoint)
        {
            destRec.X = xPoint;

            for (int i = 0; i < hitBoxes.Length; i++)
            {
                int halfCharacterX = (runningAnimation.Width / numCols) / 2;
                int halfCharacterY = (runningAnimation.Height / numRows) / 2;
                int headSize = 32;
                int halfHead = headSize / 2;

                hitBoxes[0] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y, halfCharacterX + halfHead, halfHead / 2);
                hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfCharacterX + halfHead, halfHead / 2);
                hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2, halfCharacterX, GetDest().Height - headSize / 2);
                hitBoxes[3] = new Rectangle(GetDest().X + halfCharacterX, GetDest().Y + halfHead / 2, halfCharacterX, GetDest().Height - headSize / 2);
            }
        }

        public void SetCharacterY(int yPoint)
        {
            destRec.Y = yPoint;

            for (int i = 0; i < hitBoxes.Length; i++)
            {
                int halfCharacterX = (runningAnimation.Width / numCols) / 2;
                int halfCharacterY = (runningAnimation.Height / numRows) / 2;
                int headSize = 32;
                int halfHead = headSize / 2;

                hitBoxes[0] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y, halfCharacterX + halfHead, halfHead / 2);
                hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfCharacterX + halfHead, halfHead / 2);
                hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2, halfCharacterX, GetDest().Height - headSize / 2);
                hitBoxes[3] = new Rectangle(GetDest().X + halfCharacterX, GetDest().Y + halfHead / 2, halfCharacterX, GetDest().Height - headSize / 2);
            }
        }

        public void ChangeDirection()
        {
            if (isGoingRight == false)
            {
                isGoingRight = true;
                xVelocity = 2f;
            }
            else
            {
                isGoingRight = false;
                xVelocity = -2f;
            }
        }

        public float GetXVelocity()
        {
            return xVelocity;
        }

        public float GetYVelocity()
        {
            return yVelocity;
        }

        public Rectangle GetHitboxRec(int i)
        {
            return hitBoxes[i];
        }

        public bool IsGoingRight()
        {
            return isGoingRight;
        }

        public void SetHitXPos()
        {
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].X += (int)xVelocity;
            }
        }

        public void SetHitYPos()
        {
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].Y += (int)yVelocity;
            }
        }

        public void ResetXVelocity()
        {
            xVelocity = 0f;
        }

        public void ResetYVelocity()
        {
            yVelocity = 0f;
        }

        public void SetIsDead()
        {
            isDead = true;
        }

        public bool GetIsDead()
        {
            return isDead;
        }

        public void SetIsAlternateDead()
        {
            isAlternateDead = true;
        }

        public bool GetIsAlternateDead()
        {
            return isAlternateDead;
        }

        public void DeathJump()
        {
            yVelocity = DEATH_JUMP;

            if (isGoingRight == true)
            {
                xVelocity = DEATH_JUMP / 2;
            }
            else
            {
                xVelocity = -DEATH_JUMP / 2;
            }
        }

        public void IncrimentDeathTimer()
        {
            deathTimer++;
        }

        public bool IsDeathTimerUp()
        {
            if (deathTimer >= dTimerMax)
            {
                return true;
            }

            return false;
        }

        public void ResetDeathTimer()
        {
            deathTimer = 0;
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
