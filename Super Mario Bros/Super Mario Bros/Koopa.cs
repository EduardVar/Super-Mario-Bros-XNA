// Author: Eduard Varshavsky
// File Name: Koopa.cs
// Project Name: Super Mario Bros
// Creation Date: October 9, 2017
// Modified Date: October 16, 2017
// Description: This class is used for affecting and manipulating koopas
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
    class Koopa : Characters
    {
        //Attributes

        //Stores animation of koopa in shell
        private Texture2D inShellAnimation;

        //Stores bools for shell related interactions
        private bool isInShell;
        private bool isSliding = false;

        //Stores integer for frame timers used in shell
        private int inShellTimer;

        //Stores dimensions for height of destination rectangle based on form
        private int outShellHeight = 92;
        private int inShellHeight = 60;

        //Constructor
        public Koopa(Rectangle destRec)
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
        //Post: Sets the images to the same named Texture2D s in this object and does calculations for hitboxes and srcRec
        //      based off the data calculated
        //Desc: Used to set the initial images and hitboxes to be later used with within the physics of this object
        public void SetKoopaImages(Texture2D runningAnimation, Texture2D inShellAnimation)
        {
            //Sets the Texture2Ds inside this object to the ones provided by the parameters
            this.runningAnimation = runningAnimation;
            this.inShellAnimation = inShellAnimation;

            //Does dimension calculations based off the image data provided
            int halfGoombaX = (runningAnimation.Width / numCols) / 2;
            int halfGoombaY = (runningAnimation.Height / numRows) / 2;
            int headSize = 32;
            int halfHead = headSize / 2;

            //Builds hitboxes for rectangles
            hitBoxes[0] = new Rectangle(GetDest().X, GetDest().Y, halfGoombaX * 2, halfHead / 2);
            hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfGoombaX + halfHead, halfHead / 2);
            hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2, halfGoombaX, GetDest().Height - headSize / 2);
            hitBoxes[3] = new Rectangle(GetDest().X + halfGoombaX, GetDest().Y + halfHead / 2, halfGoombaX, GetDest().Height - headSize / 2);

            //Calculates dimensions to be used in creation of source rectangle
            walkFrameWidth = runningAnimation.Width / numCols;
            walkFrameHeight = runningAnimation.Height / numRows;
            srcRec = new Rectangle(GetSrcX(currentWalkFrame, walkFrameWidth, numCols), GetSrcY(currentWalkFrame, walkFrameHeight, numRows), walkFrameWidth, walkFrameHeight);
        }

        //Pre:  Requires to be called by the MainGame class and be provide Texture2D s for runningAnimation and inShellAnimation
        //Post: Resets y parameters for related rectangles in this object
        //Desc: Used to set the initial images and hitboxes to be later used with within the physics of this object
        public new void SetCharacterY(int yPoint)
        {
            destRec.Y = yPoint;

            for (int i = 0; i < hitBoxes.Length; i++)
            {
                //Does dimension calculations based off the image data provided
                int halfCharacterX = (runningAnimation.Width / numCols) / 2;
                int halfCharacterY = (runningAnimation.Height / numRows) / 2;
                int headSize = 32;
                int halfHead = headSize / 2;

                if (isInShell == false)
                {
                    //Rebuilds hitboxes for rectangles based on not being in shell
                    hitBoxes[0] = new Rectangle(GetDest().X + 3, GetDest().Y, halfCharacterX + halfHead * 2 - 6, halfHead * 2);
                    hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfCharacterX + halfHead, halfHead / 2);
                    hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead, halfCharacterX, GetDest().Height - halfHead * 2 + 9);
                    hitBoxes[3] = new Rectangle(GetDest().X + halfCharacterX, GetDest().Y + halfHead, halfCharacterX, GetDest().Height - halfHead * 2 + 9);
                }
                else
                {
                    //Checks if the koopa is slidng
                    if (isSliding == false)
                    {
                        //Rebuilds hitboxes for rectangles based on being in shell
                        hitBoxes[0] = new Rectangle(GetDest().X + 3, GetDest().Y + 32, halfCharacterX + halfHead * 2 - 6, halfHead);
                        hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfCharacterX + halfHead, halfHead / 2);
                        hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2 + 32, halfCharacterX, inShellHeight - headSize / 2);
                        hitBoxes[3] = new Rectangle(GetDest().X + halfCharacterX, GetDest().Y + halfHead / 2 + 32, halfCharacterX, inShellHeight - headSize / 2);
                    }
                    else
                    {
                        //Rebuilds hitboxes for rectangles based on being in shell and sliding
                        hitBoxes[0] = new Rectangle(GetDest().X + 12, GetDest().Y + 32, GetDest().Width - 24, halfHead);
                        hitBoxes[1] = new Rectangle(GetDest().X + 12, GetDest().Y + GetDest().Height - halfHead / 2, GetDest().Width - 24, halfHead / 2);
                        hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2 + 32, halfCharacterX, inShellHeight - headSize / 2);
                        hitBoxes[3] = new Rectangle(GetDest().X + halfCharacterX, GetDest().Y + halfHead / 2 + 32, halfCharacterX, inShellHeight - headSize / 2);
                    }
                }
            }
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Texture2D of the asked for image
        //Desc: Used to provide a inShell image for when the koopa is in shell
        public Texture2D GetInShellImg()
        {
            //Gives the shell image
            return inShellAnimation;
        }

        //Behaviours

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles downwards
        //Desc: Used to animate the koopa based on the action performed in game, based off a source rectangles using
        //      a provided sprite sheet
        public void AnimateKoopa()
        {
            //Checks if 
            if (isInShell == true && isSliding == false)
            {
                inShellTimer++;
            }

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

                if (inShellTimer < 300 && isInShell == true)
                {
                    currentWalkFrame = 1;
                }

                if (inShellTimer >= 480)
                {
                    GoOutOfShell();
                    SetOutShellHeight();
                    inShellTimer = 0;
                    ChangeDirection();
                    ChangeDirection();
                }

                //reset the number of frames passed since the alst update back to 0
                frameCount = 0;
            }
        }

        public void GoIntoShell()
        {
            ResetXVelocity();
            isInShell = true;
        }

        public void GoOutOfShell()
        {
            isInShell = false;
        }

        public bool GetIsInShell()
        {
            return isInShell;
        }

        public void SetInShellHeight()
        {
            destRec.Height = outShellHeight;
        }

        public void SetOutShellHeight()
        {
            destRec.Height = outShellHeight;
        }

        public void KickShellRight()
        {
            xVelocity = -12f;
        }

        public void KickShellLeft()
        {
            xVelocity = 12f;
        }

        public void SetIsSliding(bool newBool)
        {
            isSliding = newBool;
        }

        public bool GetIsSliding()
        {
            return isSliding;
        }
    }
}
