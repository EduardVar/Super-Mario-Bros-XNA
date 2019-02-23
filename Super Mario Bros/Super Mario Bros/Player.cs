// Author: Eduard Varshavsky
// File Name: Player.cs
// Project Name: Super Mario Bros
// Creation Date: October 7, 2017
// Modified Date: October 15, 2017
// Description: This class is used for affecting and manipulating the player (mario)
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
    class Player : Characters

    {
        //Attributes

        //Created specific Texture2D holders for specific still images
        private Texture2D jumpFrame;
        private Texture2D skidFrame;
        private Texture2D deathFrame;

        //Created an integer to store the base frame timer for animation
        private int baseFrame = 15;

        //Created bools that store certain buffs or debuffs for the player
        private bool isRunning = false;
        private bool wasRunning = false;
        private bool isSkidding = false;
        private bool isJumping = false;
        private bool isBouncing = false;
        private bool isDying = false;
        private bool isInAir = false;
        private bool jumpButtonPressed;
        private bool jumpButtonHeld;
        private bool initialJumpRight;

        //Created constant ints for states mario will have and set current state to SMALL_MARIO
        public const int SMALL_MARIO = 0;
        public const int BIG_MARIO = 1;
        public const int FIRE_MARIO = 2;
        private int marioState = SMALL_MARIO;

        //Created constant floats that will be used for walk physics related interactions
        private const float MIN_WALK_VELOCITY = 0.13f * SCALE_CONST;
        private const float WALK_ACC = 0.098f * SCALE_CONST;
        private const float RUN_ACC = 0.228f;
        private const float RELEASE_DEC = 0.208f;
        private const float SKID_SPEED = 0.26f;
        private const float MAX_WALK_SPEED = 1.9f * SCALE_CONST;
        private const float MAX_RUN_SPEED = 2.9f * SCALE_CONST;

        //Created constant floats that will be used for air gain physics related interactions
        private const float AIR_GAIN_MOM_LOW = WALK_ACC;
        private const float AIR_GAIN_MOM_HIGH = RUN_ACC;

        //Created constant floats that will be used for air loss physics related interactions
        private const float AIR_LOSS_MOM_HIGH = RUN_ACC;
        private const float AIR_LOSS_MOM_MID = RELEASE_DEC;
        private const float AIR_LOSS_MOM_LOW = WALK_ACC;

        //Created constant floats that will be used for terminal velocity physics
        private const float MAX_FALL_VELOCITY = 4.8f * SCALE_CONST;

        //Created constant floats that will be used for small jump physics related interactions
        private const float INITIAL_JUMP_LOWER = 4f * SCALE_CONST;
        private const float HOLD_JUMP_LOWER = .2f * SCALE_CONST;
        private const float NEW_GRAVITY_LOWER = .7f * SCALE_CONST / 2;

        //Created constant floats that will be used for medium jump physics related interactions
        private const float INITIAL_JUMP_MID = 4f * SCALE_CONST;
        private const float HOLD_JUMP_MID = .3f * SCALE_CONST;
        private const float NEW_GRAVITY_MID = .6f * SCALE_CONST / 2;

        //Created constant floats that will be used for high jump physics related interactions
        private const float INITIAL_JUMP_HIGH = 5f * SCALE_CONST;
        private const float HOLD_JUMP_HIGH = .28f * SCALE_CONST;
        private const float NEW_GRAVITY_HIGH = .9f * SCALE_CONST / 2;

        //Created floats that hold the actual velocity and gravity values to be used on the player
        private float initialXVelocity;
        private float currentGravity;

        //Constructor

        public Player(Rectangle destRec, int marioState)
        {
            //Sets data based on what was provided in the constructro
            this.destRec = destRec;
            this.marioState = marioState;
            isGoingRight = true;

            //Sets specific animation integers for this specific class
            walkFrames = 4;
            numRows = 1;
            numCols = 4;
            currentWalkFrame = 0;
            frameCount = 0;
            walkFrameRepeat = 5;
        }

        //Getters and Setters

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the integer tied to state mario is in right now
        //Desc: Used in MainGame to determine which different interactions are done based
        //      on the state mario is in
        public int GetMarioState()
        {
            //Returns marioState
            return marioState;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Texture2D of the asked for image
        //Desc: Used to provide a jump image for when the player is jumping
        public Texture2D GetMarioJumpImg()
        {
            //Returns jumpFrame
            return jumpFrame;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Texture2D of the asked for image
        //Desc: Used to provide a skid image for when the player is skidding
        public Texture2D GetMarioSkid()
        {
            //Returns skidFrame
            return skidFrame;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Texture2D of the asked for image
        //Desc: Used to provide a death image for when the player is dying
        public Texture2D GetDeathImg()
        {
            //Returns deathFrame
            return deathFrame;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the Texture2D of the asked for image
        //Desc: Used to provide a jump image for when the player is jumping
        public Texture2D GetJumpImg()
        {
            //Returns marioState
            return jumpFrame;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to enable or diable the isSkidding bool
        public void SetMarioSkid(bool inBool)
        {
            //Sets is skidding to the bool provided
            isSkidding = inBool;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool that was asked for
        //Desc: Used to provide a skidding bool for when the player is potentially skidding
        public bool GetSkidBool()
        {
            //Returns isSkidding
            return isSkidding;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to enable or diable the isInAir bool
        public void SetIsInAir(bool airBool)
        {
            //Sets the isInAir bool to the bool provided
            isInAir = airBool;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool that was asked for
        //Desc: Used to provide a inAir bool for when the player is potentially in the air
        public bool GetInAir()
        {
            //Returns isInAir
            return isInAir;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to provide a isBouncing bool for when the player is potentially bouncing
        public void SetIsBouncing(bool bounceBool)
        {
            //Sets isBouncing to the provided bool
            isBouncing = bounceBool;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool that was asked for
        //Desc: Used to provide a isBouncing bool for when the player is potentially bouncing
        public bool GetIsBouncing()
        {
            //Returns marioState
            return isBouncing;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to set a isJumping bool for when the player is potentially jumping
        public void SetIsIsJumping()
        {
            //Sets isJumping to false
            isJumping = false;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool that was asked for
        //Desc: Used to provide a jumpButtonPressed bool for when the player is potentially pressing the jump button
        public bool GetIsJumpBtnPressed()
        {
            //Returns jumpButtonPressed
            return jumpButtonPressed;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to set a jumpButtonPressed bool for when the player is potentially pressing the jump button
        public void SetIsJumpBtnPressed(bool isPressed)
        {
            //Sets jumpButtonPressed to the provided bool and enables isJumping
            jumpButtonPressed = isPressed;
            isJumping = true;

            //Sets the initial xVelocity and direction to their current values
            initialXVelocity = xVelocity;
            initialJumpRight = isGoingRight;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool that was asked for
        //Desc: Used to provide a jumpButtonHeld bool for when the player is potentially pressing the jump button
        public bool GetIsJumpBtnHeld()
        {
            //Returns jumpButtonHeld
            return jumpButtonHeld;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to set a jumpButtonPressed bool for when the player is potentially pressing the jump button
        public void SetIsJumpBtnHeld(bool isHeld)
        {
            //Sets is jumpButtonHeld to the provided bool
            jumpButtonPressed = isHeld;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Sets the int inside this object to its initial form
        //Desc: Used to set currentWalkFrame int to 0 so the images will be on their initial form
        public void ResetFrameCounter()
        {
            //Resets currentWalkFrame to 0
            currentWalkFrame = 0;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to set a isRunning bool to true for when the player is running
        public void SetMarioRun()
        {
            //Sets isRunning to true
            isRunning = true;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to set a isRunning bool to false for when the player is not running
        public void DeSetMarioRun()
        {
            //Sets isRunning to false
            isRunning = false;
        }

        //Pre:  Requires to be called by the MainGame class and be provided a bool as a parameter
        //Post: Sets the bool inside this object to the one provided
        //Desc: Used to set a isDying bool for when the player is potentially dying
        public void SetIsDeathAnimationPlaying(bool newBool)
        {
            //Sets isDying to the provided bool
            isDying = newBool;
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Gives the bool that was asked for
        //Desc: Used to provide a isDying bool for when the player is potentially pressing dying
        public bool GetIsDeathAnimationPlaying()
        {
            //Provides isDying bool
            return isDying;
        }

        //Behaviours

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles
        //Desc: When the player inputs a command asking to move the player object forwards, this behaivour is
        //      used to determine the effects it will have on the rectangles of this object
        public void IncrimentMarioRec()
        {
            //Checks if the player is in the air before applying velocity since air and walking speeds are different
            if (isInAir == false)
            {
                //Sets bools prior to default state before anything is done based off them
                isGoingRight = true;
                isSkidding = false;

                //Check if the currentWalk frame is the standing frame
                if (currentWalkFrame == 0)
                {
                    //Appplies initial walk velocity if the current walk frame is the default one
                    xVelocity = MIN_WALK_VELOCITY;
                }
                else if (xVelocity < 0 && isInAir == false)
                {
                    //Skids velocity and bool if the player is going in the opposite direction of where he was headed
                    xVelocity += SKID_SPEED;
                    isSkidding = true;
                }
                else if (isRunning == false && xVelocity > MAX_WALK_SPEED)
                {
                    //Slows down the player if they stop running but continue walking by applying decelleration
                    xVelocity += -RELEASE_DEC;
                }
                else if (isRunning == false && xVelocity <= MAX_WALK_SPEED)
                {
                    //Adds walking velocity onto the player if they aren't running 
                    xVelocity += WALK_ACC;
                }
                else if (isRunning == true && xVelocity <= MAX_RUN_SPEED)
                {
                    //Adds walking velocity onto the player if they are running 
                    xVelocity += RUN_ACC;
                }
            }
            else
            {
                //Checks which direction the player initiall jumped in
                if (initialJumpRight == true)
                {
                    //Checks if the initial velocity is less thant the max speed
                    if (initialXVelocity <= MAX_WALK_SPEED)
                    {
                        //Applies additional velocity while in the aior
                        xVelocity += AIR_GAIN_MOM_LOW;

                        //Checks if the player was walking initially so it won't exceed the walk speed
                        if (xVelocity > MAX_WALK_SPEED && wasRunning == false)
                        {
                            //Reduces spead to running speed if the player exceeds normal running speed
                            xVelocity -= RELEASE_DEC * 2;
                        }
                    }
                }
                else if (isGoingRight == false)
                {
                    //Checks if the initial velocity they started off in fell in the lower range
                    if ((initialXVelocity < MAX_WALK_SPEED && initialXVelocity < 0.29f * SCALE_CONST) && initialXVelocity < MAX_WALK_SPEED)
                    {
                        //Applies a lower gain in air velocity
                        xVelocity += AIR_LOSS_MOM_LOW;

                        //Checks if the player was walking initially so it won't exceed the walk speed
                        if (xVelocity > MAX_WALK_SPEED)
                        {
                            //Reduces spead to running speed if the player exceeds normal waklking speed
                            xVelocity -= RELEASE_DEC;
                        }
                    }
                    else if ((initialXVelocity < MAX_WALK_SPEED && initialXVelocity >= 0.29f * SCALE_CONST) && initialXVelocity < MAX_WALK_SPEED)
                    {
                        //Applies a medium gain in air velocity
                        xVelocity += AIR_LOSS_MOM_MID;

                        //Checks if the player was walking initially so it won't exceed the walk speed
                        if (xVelocity > MAX_WALK_SPEED)
                        {
                            //Reduces spead to running speed if the player exceeds normal waklking speed
                            xVelocity -= RELEASE_DEC;
                        }
                    }
                    else if (initialXVelocity <= MAX_WALK_SPEED && initialXVelocity <= MAX_WALK_SPEED)
                    {
                        //Applies a higher gain in air velocity
                        xVelocity += AIR_LOSS_MOM_HIGH;

                        //Checks if the player was walking initially so it won't exceed the walk speed
                        if (xVelocity > MAX_WALK_SPEED)
                        {
                            //Reduces spead to running speed if the player exceeds normal waklking speed
                            xVelocity -= RELEASE_DEC;
                        }
                    }
                }
            }

            //Checks if xVelocity  exceeds maximum running speed
            if (xVelocity > MAX_RUN_SPEED)
            {
                //Sets the velocity to the max run speed to avoid high speed situations
                xVelocity = MAX_RUN_SPEED;
            }

            //Applies the xVelocity to the destination rectangle and hitboxes
            destRec.X += (int)xVelocity;
            SetHitXPos();
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles
        //Desc: When the player inputs a command asking to move the player object backwards, this behaivour is
        //      used to determine the effects it will have on the rectangles of this object
        public void DecrimentMarioRec()
        {
            //Checks if the player is in the air before applying velocity since air and walking speeds are different
            if (isInAir == false)
            {
                //Sets bools prior to default state before anything is done based off them
                isGoingRight = false;
                isSkidding = false;

                //Check if the currentWalk frame is the standing frame
                if (currentWalkFrame == 0)
                {
                    //Appplies initial walk velocity if the current walk frame is the default one
                    xVelocity = -MIN_WALK_VELOCITY;
                }
                else if (xVelocity > 0 && isInAir == false)
                {
                    //Skids velocity and bool if the player is going in the opposite direction of where he was headed
                    xVelocity -= SKID_SPEED;
                    isSkidding = true;
                }
                else if (isRunning == false && xVelocity < -MAX_WALK_SPEED)
                {
                    //Slows down the player if they stop running but continue walking by applying decelleration
                    xVelocity += RELEASE_DEC;
                }
                else if (isRunning == false && xVelocity >= -MAX_WALK_SPEED)
                {
                    //Adds walking velocity onto the player if they aren't running 
                    xVelocity -= WALK_ACC;
                }
                else if (isRunning == true && xVelocity >= -MAX_RUN_SPEED)
                {
                    //Adds walking velocity onto the player if they are running 
                    xVelocity -= RUN_ACC;
                }
            }
            else
            {
                //Checks which direction the player initiall jumped in
                if (initialJumpRight == true) 
                {
                    //Checks if the initial velocity is less thant the max speed
                    if ((initialXVelocity > -MAX_WALK_SPEED && initialXVelocity > -0.29f * SCALE_CONST) && initialXVelocity > -MAX_WALK_SPEED)
                    {
                        //Applies additional velocity while in the air
                        xVelocity += -AIR_LOSS_MOM_LOW;

                        //Checks if the player was walking initially so it won't exceed the walk speed
                        if (xVelocity < -MAX_WALK_SPEED)
                        {
                            //Reduces spead to running speed if the player exceeds normal waklking speed
                            xVelocity = -MAX_WALK_SPEED;
                        }
                    }
                    else if ((initialXVelocity > -MAX_WALK_SPEED && initialXVelocity <= -0.29f * SCALE_CONST) && initialXVelocity > -MAX_WALK_SPEED)
                    {
                        //Applies a medium gain in air velocity
                        xVelocity += -AIR_LOSS_MOM_MID;

                        //Checks if xVelocity  exceeds maximum running speed
                        if (xVelocity < -MAX_WALK_SPEED)
                        {
                            //Reduces spead to running speed if the player exceeds normal waklking speed
                            xVelocity = -MAX_WALK_SPEED;
                        }
                    }
                    else if (initialXVelocity >= -MAX_WALK_SPEED && initialXVelocity > -MAX_WALK_SPEED)
                    {
                        //Applies a higher gain in air velocity
                        xVelocity += -AIR_LOSS_MOM_HIGH;

                        //Checks if xVelocity exceeds maximum running speed
                        if (xVelocity < -MAX_WALK_SPEED)
                        {
                            //Reduces spead to running speed if the player exceeds normal waklking speed
                            xVelocity = -MAX_WALK_SPEED;
                        }
                    }
                }
                else if (initialJumpRight == false)
                {
                    //Checks if the player was Running initially so it won't exceed the run speed
                    if (initialXVelocity < MAX_WALK_SPEED)
                    {
                        //Applies lower gain momenteum
                        xVelocity += -AIR_GAIN_MOM_LOW;

                        //Checks if xVelocity exceeds maximum running speed
                        if (xVelocity < -MAX_WALK_SPEED)
                        {
                            //Applies the walk speed to the xvelocity
                            xVelocity = -MAX_WALK_SPEED;
                        }
                    }
                    else if (initialXVelocity < MAX_RUN_SPEED)
                    {
                        //Applies higher gain momenteum
                        xVelocity += -AIR_GAIN_MOM_HIGH;

                        //Checks if xVelocity exceeds maximum running speed
                        if (xVelocity < -MAX_WALK_SPEED)
                        {
                            //Applies the walk speed to the xvelocity
                            xVelocity = -MAX_WALK_SPEED;
                        }
                    }
                }
            }

            //Checks if xVelocity exceeds maximum running speed
            if (xVelocity < -MAX_RUN_SPEED)
            {
                //Sets the velocity to the max run speed to avoid high speed situations
                xVelocity = -MAX_RUN_SPEED;
            }

            //Applies the xVelocity to the destination rectangle and hitboxes
            destRec.X += (int)xVelocity;
            SetHitXPos();
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles
        //Desc: When the player inputs no command to move the player object, this behaivour is applied to 
        //      return the rectangles to its resting state
        public void DecelerateMarioRec()
        {
            //Checks if the player has positive xVeloctiy(forwards)
            if (xVelocity > 0)
            {
                //Reduces the xVelocity until it reaches 0
                xVelocity -= RELEASE_DEC;

                //Applies the xVelocity to the destination rectangle and hitboxes
                destRec.X += (int)xVelocity;
                SetHitXPos();
            }

            //Checks if the player has negative xVeloctiy(backwards)
            if (xVelocity < 0)
            {
                //Reduces the xVelocity until it reaches 0
                xVelocity += RELEASE_DEC;

                //Applies the xVelocity to the destination rectangle and hitboxes
                destRec.X += (int)xVelocity;
                SetHitXPos();
            }
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles upwards
        //Desc: When the player inputs a command to move the player object with a jump, this behaivour is 
        //      applied to return the rectangles to push the rectangles up
        public void InitiateJump()
        {
            //Stores the initial velocity and running states
            initialXVelocity = xVelocity;
            wasRunning = isRunning;

            //Makes sure player is dead to run this code
            if (isDead == true)
            {
                //Makes the player do a high jump
                yVelocity = -INITIAL_JUMP_HIGH * 2;
            }
            else if (initialXVelocity > 2.5f || initialXVelocity < 2.5f)
            {
                //Applies a higher initial jump to the yVelocity upwards
                yVelocity = -INITIAL_JUMP_HIGH;
            }
            else if (initialXVelocity < 2.49f || initialXVelocity > -2.49f)
            {
                //Applies a medium initial jump to the yVelocity upwards
                yVelocity = -INITIAL_JUMP_MID;
            }
            else if (initialXVelocity < 1f || initialXVelocity > -1f)
            {
                //Applies a lower initial jump to the yVelocity upwards
                yVelocity = -INITIAL_JUMP_LOWER;
            }

            //Applies the xVelocity to the destination rectangle and hitboxes
            destRec.Y += (int)yVelocity;
            SetHitYPos();
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles upwards continuousy
        //Desc: When the player inputs a command to move the player object with a held jump, this behaivour is 
        //      applied to return the rectangles to push the rectangles up a little bit more
        public void HoldJump()
        {
            //Checks if the player is Bouncing off of an object
            if (isBouncing == false)
            {
                //Checks if the xVelocity of the player is in its lower velocities initially
                if (initialXVelocity > 2.5f || initialXVelocity < 2.5f)
                {
                    //Applies the higher jump velocties the lower the initial xVelocity is
                    yVelocity += -HOLD_JUMP_HIGH;
                }
                else if (initialXVelocity < 2.49f || initialXVelocity > -2.49f)
                {
                    //Applies the medium jump velocties the more medium the initial xVelocity is
                    yVelocity += -HOLD_JUMP_MID;
                }
                else if (initialXVelocity < 1f || initialXVelocity > -1f)
                {
                    //Applies the lower jump velocties the higher the initial xVelocity is
                    yVelocity += -HOLD_JUMP_LOWER;
                }
            }
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles downwards
        //Desc: When the player is in the air, this function is applied to apply gravity on their destination rectangle
        public void ApplyGravity()
        {
            //Checks if the xVelocity of the player is in its lower velocities initially
            if (initialXVelocity > 2.5f || initialXVelocity < 2.5f)
            {
                //Applies the lower gravity speed for lower initial xVelocities
                yVelocity += NEW_GRAVITY_HIGH;
            }
            else if (initialXVelocity < 2.49f || initialXVelocity > -2.49f)
            {
                //Applies the medium gravity speed for medium initial xVelocities
                yVelocity += NEW_GRAVITY_MID;
            }
            else if (initialXVelocity < 1f || initialXVelocity > -1f)
            {
                //Applies the higher gravity speed for higher initial xVelocities
                yVelocity += NEW_GRAVITY_LOWER;
            }

            //Applies the velocity to the destination rectangle and hitbox positions
            destRec.Y += (int)yVelocity;
            SetHitYPos();
        }

        //Pre:  Requires to be called by the MainGame class and be provide Texture2D s for runningAnimation, jumpFrame,
        //      skidFrame, and deathFrame
        //Post: Sets the images to the same named Texture2D s in this object and does calculations for hitboxes and srcRec
        //      based off the data calculated
        //Desc: Used to set the initial images and hitboxes to be later used with within the physics of this object
        public void SetMarioImages(Texture2D runningAnimation, Texture2D jumpFrame, Texture2D skidFrame, Texture2D deathFrame)
        {
            //Sets the Texture2Ds inside this object to the ones provided by the parameters
            this.runningAnimation = runningAnimation;
            this.jumpFrame = jumpFrame;
            this.skidFrame = skidFrame;
            this.deathFrame = deathFrame;

            //Does dimension calculations based off the image data provided
            int halfMarioX = (runningAnimation.Width / numCols) / 2;
            int halfMarioY = (runningAnimation.Height / numRows) / 2;
            int headSize = 32;
            int halfHead = headSize / 2;

            //Builds hitboxes for rectangles
            hitBoxes[0] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y, halfMarioX + halfHead, halfHead / 2);
            hitBoxes[1] = new Rectangle(GetDest().X + halfHead / 2, GetDest().Y + GetDest().Height - halfHead / 2, halfMarioX + halfHead, halfHead / 2);
            hitBoxes[2] = new Rectangle(GetDest().X, GetDest().Y + halfHead / 2, halfMarioX, GetDest().Height - headSize / 2);
            hitBoxes[3] = new Rectangle(GetDest().X + halfMarioX, GetDest().Y + halfHead / 2, halfMarioX, GetDest().Height - headSize / 2);

            //Calculates dimensions to be used in creation of source rectangle
            walkFrameWidth = runningAnimation.Width / numCols;
            walkFrameHeight = runningAnimation.Height / numRows;
            srcRec = new Rectangle(GetSrcX(currentWalkFrame, walkFrameWidth, numCols), GetSrcY(currentWalkFrame, walkFrameHeight, numRows), walkFrameWidth, walkFrameHeight);
        }

        //Pre:  Requires to be called by the MainGame class
        //Post: Applys a translation onto the destination rectangle and hitbox rectangles downwards
        //Desc: Used to animate mario based on the action performed in game, based off a source rectangles using
        //      a provided sprite sheet
        public void AnimateMario()
        {
            //Checks if mario is walking to the right (positive)
            if (xVelocity > 0)
            {
                //Reduces the walk frame repeat by the velocity to simulate increase in speed
                walkFrameRepeat = baseFrame - (int)xVelocity;
            }
            else
            {
                //Reduces the walk frame repeat by the velocity to simulate increase in speed
                walkFrameRepeat = baseFrame + (int)xVelocity;
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

                //reset the number of frames passed since the alst update back to 0
                frameCount = 0;
            }
        }
    }
}
