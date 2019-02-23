// Author: Eduard Varshavsky
// File Name: MainGame.cs
// Project Name: Super Mario Bros
// Creation Date: October 3, 2017
// Modified Date: October 16, 2017
// Description: This program is built to play a replica of Super Mario Bros and contains all the code for real time updates
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

//Added the reference to the Camera2D_XNA4 library to follow mario
using Camera2D_XNA4;

namespace Super_Mario_Bros
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        //Created variables for graphics and sprites on screen
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Stored the sprite font
        SpriteFont uiFont;

        //Stored the location and content for the time UI string
        int timeFrameCounter;
        int timeFrameCountMax = 24;

        //Stored the location and content for the name UI string
        Vector2 nameLoc;
        string nameContent = "MARIO";

        //Stored the location and content for the score UI string
        Vector2 scoreLoc;
        string actualScore = "000000";

        //Stored the location and content for the coins UI string
        Vector2 coinsCountLoc;
        int coinsCounted;

        //Stored the location and content for the world info UI string
        Vector2 worldTitleLoc;
        string worldTitleContent = "WORLD";

        //Stored the location and content for the world number UI string
        Vector2 worldNumberLoc;
        string worldNumberContent = "1-1";

        //Stored the location and content for the title of time UI string
        Vector2 timeTitleLoc;
        string timeTitleContent = "TIME";

        //Stored the location and content for the time data UI string
        Vector2 timeNumberLoc;
        int timeNumberContent = 400;

        //Created integers to store resolution dimensions
        int screenWidth;
        int screenHeight;

        //Stored sound effects based on Mario's actions
        SoundEffect marioDeathSound;
        SoundEffect marioJumpSound;

        //Stored sound effects based on Enemy actions
        SoundEffect squishSound;
        SoundEffect kickShellSound;
        SoundEffect bumpSound;

        //Stored sound effects based on item and game actions
        SoundEffect coinSound;
        SoundEffect timeRunningOut;
        SoundEffect victoryMusic;

        //Stored music content for the background songs
        Song Music1x1;
        Song Music1x1Fast;

        //Stores the textures and locations for the background, as well as the bounds for the camera
        Texture2D[] backGrounds = new Texture2D[7];
        Rectangle backgroundRec;
        Rectangle cameraBounds;
        Rectangle cameraScreen;

        //Stored the image and location data for the castle
        Texture2D castleImg;
        Rectangle castleRec;

        //Stored the textures for mario based sprites
        Texture2D smallMarioJumpImg;
        Texture2D smallMarioWalkImg;
        Texture2D smallMarioSkidImg;
        Texture2D MarioDieImg;

        //Stored the textures for goomba based sprites
        Texture2D goombaWalkImg;
        Texture2D goombaDieImg;

        //Stored the textures for koopa based sprites
        Texture2D koopaWalkImg;
        Texture2D koopaShellImg;

        //Stored the textures for block based sprites
        Texture2D brickImg;
        Texture2D itemBlockImg;
        Texture2D hitBlockImg;
        Texture2D skyImg;
        Texture2D coinAni;

        //Stored images and Rectangle for the coin picture in the UI
        Texture2D coinImageSmall;
        Rectangle coinUIImage;

        //Stored the dimensions and location of the end zone
        Rectangle endArea;

        //Stored the dimensions of Mario when he is normal small Mario
        Rectangle smallMarioRec;
        const int HALF_MARIO = 34;
        Vector2 middleOfMario = new Vector2(0, 0);

        //Created list to store enemies
        List<Goomba> goombas = new List<Goomba>();
        List<Koopa> koopas = new List<Koopa>();

        //Created list to store blocks
        List<Brick> bricks = new List<Brick>();
        List<CoinBlock> coinBlocks = new List<CoinBlock>();
        List<CoinBank> coinBanks = new List<CoinBank>();

        //Created new instance of player and walls in this level
        Player mario;
        Terrain walls;

        //Stores bools that store true/false statements for sounds
        bool musicStarted;
        bool musicStarted2;
        bool hurrySoundPlayed;
        bool victoryMusicPlayed;

        //Created the Camera object
        Cam2D cam;

        //Stores bools that store true/false statements for movement
        bool isMarioMoving;
        bool aButtonPressed;

        //Stored the reset position for hte wall objects and the amount of walls in game
        int resetPosition;
        int wallsInGame = 38;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Sets width resolution (ORIGINAL 256 pixels)
            screenWidth = this.graphics.PreferredBackBufferWidth = 1024;

            //Sets height resolution (ORIGINAL 224 pixels)
            screenHeight = this.graphics.PreferredBackBufferHeight = 960;

            //Set up multisampling to help with the camera efficiency
            graphics.PreferMultiSampling = true;

            //Turned on vsync
            graphics.SynchronizeWithVerticalRetrace = true;

            //Applies all graphic changes
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            uiFont = Content.Load<SpriteFont>("Fonts/marioFont");

            //Loaded music data for music
            Music1x1 = Content.Load<Song>("Sounds/Music/1-1music");
            Music1x1Fast = Content.Load<Song>("Sounds/Music/1-1musicfast");

            //Loaded sound data for mario sounds
            marioDeathSound = Content.Load<SoundEffect>("Sounds/Mario/marioDeathSound");
            marioJumpSound = Content.Load<SoundEffect>("Sounds/Mario/marioJumpSound");

            //Loaded sound data for enemy sounds
            squishSound = Content.Load<SoundEffect>("Sounds/Enemies/squishSound");
            kickShellSound = Content.Load<SoundEffect>("Sounds/Enemies/koopaKickShell");
            bumpSound = Content.Load<SoundEffect>("Sounds/Enemies/bumpSound");

            //Loaded sound data for game sounds
            coinSound = Content.Load<SoundEffect>("Sounds/Items/coinSound");
            timeRunningOut = Content.Load<SoundEffect>("Sounds/Items/timeRunningOut");
            victoryMusic = Content.Load<SoundEffect>("Sounds/Mario/VictoryMusic");

            //Loaded Texture2D data for background images
            backGrounds[0] = Content.Load<Texture2D>("Images/Backgrounds/1-1a");
            backGrounds[1] = Content.Load<Texture2D>("Images/Backgrounds/1-1b");
            backGrounds[2] = Content.Load<Texture2D>("Images/Backgrounds/1-1c");
            backGrounds[3] = Content.Load<Texture2D>("Images/Backgrounds/1-1d");
            backGrounds[4] = Content.Load<Texture2D>("Images/Backgrounds/1-1e");
            backGrounds[5] = Content.Load<Texture2D>("Images/Backgrounds/1-1f");
            backGrounds[6] = Content.Load<Texture2D>("Images/Backgrounds/1-1g");

            //Loaded Image data for the castle
            castleImg = Content.Load<Texture2D>("Images/Backgrounds/castle");

            //Stored dimensions of background and camera locations
            backgroundRec = new Rectangle(0, 0, screenWidth * 2, screenHeight);
            cameraBounds = new Rectangle(0, 0, backGrounds[0].Width * 7 - screenWidth / 2 - screenWidth / 4, screenHeight);
            cameraScreen = new Rectangle(0, 0, screenWidth, screenHeight);

            //Stored dimensions and origins for the end area and castle location
            castleRec = new Rectangle(13088, 513, 160, 323);
            endArea = new Rectangle(13184, 512, 119, 320);

            //Created new rectangle to store small mario's dimensions and origin
            smallMarioRec = new Rectangle(152, 768, 68, 68);

            //Loaded image data for mario related actions
            smallMarioJumpImg = Content.Load<Texture2D>("Images/Mario/small-mario-jump");
            smallMarioWalkImg = Content.Load<Texture2D>("Images/Mario/small-mario-walk-new");
            smallMarioSkidImg = Content.Load<Texture2D>("Images/Mario/small-mario-skrt");
            MarioDieImg = Content.Load<Texture2D>("Images/Mario/mario-die");

            //Loaded image data for enemy related actions
            goombaWalkImg = Content.Load<Texture2D>("Images/Enemies/goomba-walk");
            goombaDieImg = Content.Load<Texture2D>("Images/Enemies/goomba-die");
            koopaWalkImg = Content.Load<Texture2D>("Images/Enemies/koopa-walk");
            koopaShellImg = Content.Load<Texture2D>("Images/Enemies/koopa-shell");

            //Loaded image data for block related actions
            brickImg = Content.Load<Texture2D>("Images/Blocks/brick");
            itemBlockImg = Content.Load<Texture2D>("Images/Blocks/itemBlock");
            hitBlockImg = Content.Load<Texture2D>("Images/Blocks/hitBlock");
            skyImg = Content.Load<Texture2D>("Images/Blocks/sky");
            coinAni = Content.Load<Texture2D>("Images/Blocks/coinAni");
            coinImageSmall = Content.Load<Texture2D>("Images/Interactive/smallCoin"); 

            //Set points for UI text on screen
            nameLoc = new Vector2(96f, 32f);
            scoreLoc = new Vector2(96f, 64f);
            coinsCountLoc = new Vector2(388f, 64f);
            worldTitleLoc = new Vector2(576f, 32f);
            worldNumberLoc = new Vector2(604f, 64f);
            timeTitleLoc = new Vector2(804f, 32f);
            timeNumberLoc = new Vector2(836, 64f);
            coinUIImage = new Rectangle(356, 64, 20, 32);

            //Created new instance of mario and set the images concerning him
            mario = new Player(smallMarioRec, 0);
            mario.SetMarioImages(smallMarioWalkImg, smallMarioJumpImg, smallMarioSkidImg, MarioDieImg);

            //Created new instances for goomba and koopa lists
            goombas = new Level().PopulateGoombas();
            koopas = new Level().PopulateKoopas();

            //Created new instances for brick, coin block, and coin bank lists
            bricks = new Level().PopulateBricks();
            coinBlocks = new Level().PopulateCoinBlocks();
            coinBanks = new Level().PopulateCoinBanks();

            //Repeated setting of images for the amount of goombas created
            for (int i = 0; i < goombas.Count; i++)
            {
                goombas[i].SetGoombaImages(goombaWalkImg, goombaDieImg);
            }

            //Repeated setting of images for the amount of koopas created
            for (int i = 0; i < koopas.Count; i++)
            {
                koopas[i].SetKoopaImages(koopaWalkImg, koopaShellImg);
            }

            //Repeated setting of images for the amount of bricks created
            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].SetInitialImage(brickImg);
            }

            //Repeated setting of images for the amount of coin blocks created
            for (int i = 0; i < coinBlocks.Count; i++)
            {
                if (i == 11)
                {
                    coinBlocks[i].SetInitialImage(skyImg);
                }
                else if (i == 13)
                {
                    coinBlocks[i].SetInitialImage(brickImg);
                }
                else
                {
                    coinBlocks[i].SetInitialImage(itemBlockImg);
                }

                coinBlocks[i].SetCoinImage(coinAni);
            }

            //Repeated setting of images for the amount of coin banks created
            for (int i = 0; i < coinBanks.Count; i++)
            {
                coinBanks[i].SetInitialBankImage(brickImg);
                coinBanks[i].SetCoinImage(coinAni);
            }

            //Made a new instance of walls and built the walls for the level
            walls = new Terrain();
            walls.buildWalls();

            //Set camera locations based on mario's location and began repeating the mediaplayer for looped songs
            cam = new Cam2D(GraphicsDevice.Viewport, cameraBounds, 1.0f, 4.0f, 0f, mario.GetDest());
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit(); 

            // TODO: Add your update logic here
            //Checks if the first song has started playing
            if (musicStarted == false)
            {
                //Plays the song and disables it from beign played over and over agin
                MediaPlayer.Play(Music1x1);
                musicStarted = true;
            }

            //Checks if the secodn song has started playing and if the game timer reached a certain time
            if (musicStarted2 == false && timeNumberContent == 93)
            {
                //Plays the song and disables it from beign played over and over agin
                MediaPlayer.Play(Music1x1Fast);
                musicStarted2 = true;
            }

            //Checks if the game timer has reached 100
            if (timeNumberContent == 100)
            {
                //Stops the music player
                MediaPlayer.Stop();

                //Checks if the hurry sound effect played
                if (hurrySoundPlayed == false)
                {
                    //Playes the hurry sound effect and disables it from being played again
                    timeRunningOut.Play();
                    hurrySoundPlayed = true;
                }
            }

            //Checks if the player has reached the end area
            if (endArea.Intersects(mario.GetDest()) == true)
            {
                //Checks if victory music has been played
                if (victoryMusicPlayed == false)
                {
                    //Playes the victory music while disabling other songs, disable victory music form being played again
                    victoryMusic.Play();
                    MediaPlayer.Stop();
                    victoryMusicPlayed = true;
                }
            }

            //Checks if victory music has been played or if the player got killed
            if (victoryMusicPlayed == true || mario.GetIsDead() == true)
            {
                //Stops mario in his tracks by reseting xVelocity
                mario.ResetXVelocity();
            }
            else if (victoryMusicPlayed == false && mario.GetIsDead() == false)
            {
                //Incriments the game frame timer
                timeFrameCounter++;
            }

            //Checks if the game frame timer reached the max frame limit
            if (timeFrameCounter >= timeFrameCountMax)
            {
                //Reduces the game time by 1 and resets the frame counter
                timeNumberContent--;
                timeFrameCounter = 0;
            }

            //Resets movement and runnign while updating the cameraScreen
            isMarioMoving = false;
            mario.DeSetMarioRun();
            cameraScreen.X = cameraBounds.X;
            cameraScreen.Y = cameraBounds.Y;

            //Checks if mario is not dead
            if (mario.GetIsDead() == false)
            {
                //Animated all blocks and coins
                AnimateCoinBlocks();
                DoBounceCoinBlock();
                DoBounceCoinBank();
                DoBounceBrick();

                //Goes through every single goomba in the level
                for (int i = 0; i < goombas.Count; i++)
                {
                    //Enables the goomba for interaction
                    ActiveGoomba(i);
                }

                //Goes through every single koopa in the level
                for (int i = 0; i < koopas.Count; i++)
                {
                    //Enables the koopa for interaction
                    ActiveKoopa(i);
                }

                //Cecks if mario is in contact with the end area
                if (endArea.Intersects(mario.GetDest()) == false)
                {
                    //Checks if the player pressed X on the controller
                    if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                    {
                        //Forces mario to start running
                        mario.SetMarioRun();
                    }

                    //Checks if the player pressed A on the controller
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                    {
                        //Checks if A was already pressed while on the ground without having bounced form before
                        if (mario.GetIsJumpBtnPressed() == false && mario.GetInAir() == false && aButtonPressed == false && mario.GetIsBouncing() == false)
                        {
                            //Initiates jump and enables bools for the jump button being pressed, while playing a sound to confirm jump
                            mario.SetIsJumpBtnPressed(true);
                            mario.InitiateJump();
                            marioJumpSound.Play(0.5f, 0.0f, 0f);
                            aButtonPressed = true;
                        }

                        //Will hold jump if A is continuously pressed
                        mario.HoldJump();
                        mario.SetIsJumpBtnHeld(true);
                    }

                    //Checks if the player let go of A on the controller
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released)
                    {
                        //Disables all bools set to jump
                        mario.SetIsJumpBtnPressed(false);
                        mario.SetIsJumpBtnHeld(false);
                        aButtonPressed = false;
                    }

                    //Checks if the player pressed right on the D-Pad
                    if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                    {
                        //Incriments mario positively (forward) while enabling the isMarioMoving variable
                        mario.IncrimentMarioRec();
                        isMarioMoving = true;
                    }

                    //Checks if the player pressed left on the D-Pad
                    if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed && mario.GetDest().X > cameraBounds.X)
                    {
                        //Incriments mario negativley (backward) while enabling the isMarioMoving variable
                        mario.DecrimentMarioRec();
                        isMarioMoving = true;
                    }
                }

                //Checks collision for mario 
                CheckMarioCollisions();
            }

            //Checks if Mario goes too far left past camera bounds
            if (mario.GetDest().X < cameraBounds.X)
            {
                //Resets mario position to remain inside the camera and reset his xVelocity
                mario.SetCharacterX(cameraBounds.X);
                mario.ResetXVelocity();
            }

            //Checks for mario location if he reaches the middle of the screen
            if (mario.GetDest().X - cameraBounds.X + HALF_MARIO >= screenWidth / 2)
            {
                //Moves the camerabounds to accomadate mario proceeding
                cameraBounds.X = mario.GetDest().X - 478;

                //Focuses cmaera on mario
                cam.LookAt(mario.GetDest());

                //Reassign location of UI to not fall behind with screen moving
                nameLoc.X = cam.GetPosition().X - screenWidth / 2 + 96;
                scoreLoc.X = cam.GetPosition().X - screenWidth / 2 + 96;
                coinUIImage.X = (int)cam.GetPosition().X - screenWidth / 2 + 356;
                coinsCountLoc.X = cam.GetPosition().X - screenWidth / 2 + 388;
                worldTitleLoc.X = cam.GetPosition().X - screenWidth / 2 + 576;
                worldNumberLoc.X = cam.GetPosition().X - screenWidth / 2 + 604;
                timeTitleLoc.X = cam.GetPosition().X - screenWidth / 2 + 804;
                timeNumberLoc.X = cam.GetPosition().X - screenWidth / 2 + 836;
            }

            //Checks if mario's velocity is 0
            if (Math.Round(mario.GetXVelocity()) != 0)
            {
                //Checks if mario is not skidding
                if (mario.GetSkidBool() == false)
                {
                    //Draws mario normally
                    mario.AnimateMario();
                }

                //Checks if mario has been prompted to move
                if (isMarioMoving == false)
                {
                    //Decelerates mario to reduce his speed to 0
                    mario.DecelerateMarioRec();
                }
            }

            //Checks if mario was killed but hasn't started playing the death animation yet
            if (mario.GetIsDead() == true && mario.GetIsDeathAnimationPlaying() == false)
            {
                //Increases death timer
                mario.IncrimentDeathTimer();

                //Checks if the death timer is finished
                if (mario.IsDeathTimerUp() == true)
                {
                    //Cause mario to jump up and play the death animation of him falling
                    mario.InitiateJump();
                    mario.ResetDeathTimer();
                    mario.SetIsDeathAnimationPlaying(true);
                }
            }

            //Checks if the death animation is playing on the player
            if (mario.GetIsDead() == false || mario.GetIsDeathAnimationPlaying() == true)
            {
                //Applies gravity to the dying figure to simulate it falling into the abyss
                mario.ApplyGravity();
            }
            
            //Checks if mario has rached an xVelocity of 0
            if (Math.Round(mario.GetXVelocity()) == 0)
            {
                //Disables skidding and resets frame counter to animate mario standing
                mario.SetMarioSkid(false);
                mario.ResetFrameCounter();
                mario.AnimateMario();
            }

            //Checks if mario falls below the world the world too far.
            if (mario.GetDest().Y >= screenWidth * 2)
            {
                //Checks if mario hasn't been killed yet
                if (mario.GetIsDead() == false)
                {
                    //Begins mario death sequence
                    InitialMarioDeath();
                }
            }

            //For looped based on the amount of coinBanks in the level
            for (int i = 0; i < coinBanks.Count; i++)
            {
                //If the coinbanks have an active timer run this code
                if (coinBanks[i].GetTimerRunning() == true)
                {
                    //Incriments the timer for the coin bnak running out of time
                    coinBanks[i].IncrimentTimer();
                }
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cam.GetTransformation());

            //Does this action for every background in the game
            for (int i = 0; i < backGrounds.Length; i++)
            {
                //Offsets the location of the background based on its number and draws it on screen
                backgroundRec.X = 0 + ((screenWidth * 2) * i);
                spriteBatch.Draw(backGrounds[i], backgroundRec, Color.White);
            }

            //Does this action for every brick in the game
            for (int i = 0; i < bricks.Count; i++)
            {
                //Draws each brick in the game
                spriteBatch.Draw(bricks[i].GetInitialImage(), bricks[i].GetDest(), Color.White);
            }

            //Does this action for every coin block in the game
            for (int i = 0; i < coinBlocks.Count; i++)
            {
                //Checks if the coinblock has been hit
                if (coinBlocks[i].GetIsHit() == false)
                {
                    //Make sure the block drawn does not have a brick animation (13)
                    if (i != 13)
                    {
                        //Draws the coin block with a brick animation
                        spriteBatch.Draw(coinBlocks[i].GetInitialImage(), coinBlocks[i].GetDest(), coinBlocks[i].GetSrc(), Color.White);
                    }
                    else
                    {
                        //Draws the coin block with a questio mark animation
                        spriteBatch.Draw(coinBlocks[i].GetInitialImage(), coinBlocks[i].GetDest(), Color.White);
                    }

                }
                else
                {
                    //Draws the coin block as it is hit and changes images to a hit block
                    spriteBatch.Draw(hitBlockImg, coinBlocks[i].GetDest(), Color.White);

                    //Checks if the block's coin is currently visibly seen
                    if (coinBlocks[i].GetIsCoinVisible() == true)
                    {
                        //Draws the animation of the coin being hit out of the block
                        spriteBatch.Draw(coinBlocks[i].GetCoinImg(), coinBlocks[i].getCoinDest(), coinBlocks[i].getCoinSrc(), Color.White);
                    }
                }
            }

            //Does this action for every coin bank in the game
            for (int i = 0; i < coinBanks.Count; i++)
            {
                //Checks if the coin bank is not disabled
                if (coinBanks[i].GetIsDisabled() == false)
                {
                    //Draws the coin animation inside the block when its hit out
                    spriteBatch.Draw(coinBanks[i].GetCoinImg(), coinBanks[i].getCoinDest(), coinBanks[i].getCoinSrc(), Color.White);
                }

                //Checks if the coin bank is not disabled
                if (coinBanks[i].GetIsDisabled() == false)
                {
                    //Draws the coin bank with its normal image (meaning it can be hit again)
                    spriteBatch.Draw(coinBanks[i].GetInitialImage(), coinBanks[i].GetDest(), Color.White);
                }
                else
                {
                    //Draws the coin block as it is hit and changes images to a hit block
                    spriteBatch.Draw(hitBlockImg, coinBanks[i].GetDest(), Color.White);
                }
            }

            //Does this action for every goomba in the game
            for (int i = 0; i < goombas.Count; i++)
            {
                //Checks if the goomba is dead before exexcuting
                if (goombas[i].GetIsDead() == false && goombas[i].GetIsAlternateDead() == false)
                {
                    //Draws the goomba in its normal walking animation
                    spriteBatch.Draw(goombas[i].GetWalkImg(), goombas[i].GetDest(), goombas[i].GetSrc(), Color.White);
                }
                else if (goombas[i].GetIsDead() == true)
                {
                    //Draws the goomba in its normal death image
                    spriteBatch.Draw(goombas[i].GetDeathImg(), goombas[i].GetDest(), Color.White);
                }
                else if (goombas[i].GetIsAlternateDead() == true)
                {
                    //Draws the goomba in its alternative death image
                    spriteBatch.Draw(goombas[i].GetWalkImg(), goombas[i].GetDest(), new Rectangle(0, 0, goombas[i].GetDest().Width, goombas[i].GetDest().Height), Color.White, 0f, middleOfMario, SpriteEffects.FlipVertically, 0f);
                }
            }

            //Does this action for every koopa in the game
            for (int i = 0; i < koopas.Count; i++)
            {
                //Checks if the koopa is alternatively dead before exexcuting
                if (koopas[i].GetIsAlternateDead() == false)
                {
                    //Checks if koopa is not in the shell
                    if (koopas[i].GetIsInShell() == false)
                    {
                        //Checks if the koopa is not currently going right
                        if (koopas[i].IsGoingRight() == false)
                        {
                            //Draws the koopa walking left in its out of shell animation
                            spriteBatch.Draw(koopas[i].GetWalkImg(), koopas[i].GetDest(), koopas[i].GetSrc(), Color.White);
                        }
                        else
                        {
                            //Draws the koopa walking right in its out of shell animation
                            spriteBatch.Draw(koopas[i].GetWalkImg(), koopas[i].GetDest(), koopas[i].GetSrc(), Color.White, 0f, middleOfMario, SpriteEffects.FlipHorizontally, 0f);
                        }
                    }
                    else
                    {
                        //Draws the koopa in its shell with the animation of it ready to climb out ready to activate
                        spriteBatch.Draw(koopas[i].GetInShellImg(), koopas[i].GetDest(), koopas[i].GetSrc(), Color.White);
                    }
                }
                else
                {
                    //Draws the koopa upside down in it alternative death animation
                    spriteBatch.Draw(koopas[i].GetInShellImg(), koopas[i].GetDest(), koopas[i].GetSrc(), Color.White, 0f, middleOfMario, SpriteEffects.FlipVertically, 0f);
                }
            }

            //Checks if mario has been killed
            if (mario.GetIsDead() == true)
            {
                //Draws mario in his death image
                spriteBatch.Draw(mario.GetDeathImg(), mario.GetDest(), Color.White);
            }
            else
            {
                //Checks if mario is going right
                if (mario.IsGoingRight() == true)
                {
                    //Checks if mario is in the air
                    if (mario.GetInAir() == true)
                    {
                        //Draws mario jumping if he is indeed in the air
                        spriteBatch.Draw(mario.GetJumpImg(), mario.GetDest(), Color.White);
                    }
                    else
                    {
                        //Checks if mario is currently not skidding
                        if (mario.GetSkidBool() == false)
                        {
                            //Draws mario normally with his animation
                            spriteBatch.Draw(mario.GetWalkImg(), mario.GetDest(), mario.GetSrc(), Color.White);
                        }
                        else
                        {
                            //Draws mario skidding
                            spriteBatch.Draw(mario.GetMarioSkid(), mario.GetDest(), Color.White);
                        }
                    }
                }
                else
                {
                    //Checks if mario is in the air
                    if (mario.GetInAir() == true)
                    {
                        //Draws mario jumping if he is indeed in the air
                        spriteBatch.Draw(mario.GetJumpImg(), mario.GetDest(), new Rectangle(0, 0, mario.GetDest().Width, mario.GetDest().Height), Color.White, 0f, middleOfMario, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        //Checks if mario is currently not skidding
                        if (mario.GetSkidBool() == false)
                        {
                            //Draws mario normally with his animation
                            spriteBatch.Draw(mario.GetWalkImg(), mario.GetDest(), mario.GetSrc(), Color.White, 0f, middleOfMario, SpriteEffects.FlipHorizontally, 0);
                        }
                        else
                        {
                            //Draws mario skidding
                            spriteBatch.Draw(mario.GetMarioSkid(), mario.GetDest(), new Rectangle(0, 0, mario.GetDest().Width, mario.GetDest().Height), Color.White, 0f, middleOfMario, SpriteEffects.FlipHorizontally, 0);
                        }
                    }
                }
            }

            //Draws the image of the castle in front of the player to cover him
            spriteBatch.Draw(castleImg, castleRec, Color.White);

            //Draws the UI on screen
            spriteBatch.DrawString(uiFont, nameContent, nameLoc, Color.White);
            spriteBatch.DrawString(uiFont, actualScore, scoreLoc, Color.White);
            spriteBatch.Draw(coinImageSmall, coinUIImage, Color.White);
            spriteBatch.DrawString(uiFont, "x "+ coinsCounted, coinsCountLoc, Color.White);
            spriteBatch.DrawString(uiFont, worldTitleContent, worldTitleLoc, Color.White);
            spriteBatch.DrawString(uiFont, worldNumberContent, worldNumberLoc, Color.White);
            spriteBatch.DrawString(uiFont, timeTitleContent, timeTitleLoc, Color.White);
            spriteBatch.DrawString(uiFont, "" + timeNumberContent, timeNumberLoc, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void CheckMarioCollisions()
        {
            mario.SetIsInAir(true);

            for (int i = 0; i < wallsInGame; i++)
            {
                if (cameraScreen.Intersects(walls.GetWorldWalls(i)))
                {
                    int boxHit = mario.GetInterception(walls.GetWorldWalls(i));

                    if (boxHit >= 0)
                    {
                        if (boxHit == 1)
                        {
                            if (mario.GetInAir() == true)
                            {
                                mario.SetIsJumpBtnPressed(false);
                            }

                            mario.ResetYVelocity();
                            mario.SetIsInAir(false);
                            mario.SetIsBouncing(false);

                            resetPosition = walls.GetWorldWalls(i).Y - mario.GetDest().Height + 4;

                            mario.SetCharacterY(resetPosition);
                        }
                        else if (((mario.IsGoingRight() == true || mario.GetSkidBool() == true && mario.IsGoingRight() == false) && boxHit == 3))
                        {
                            mario.ResetXVelocity();

                            resetPosition = walls.GetWorldWalls(i).X - mario.GetDest().Width + 6;

                            mario.SetCharacterX(resetPosition);
                        }
                        else if (((mario.IsGoingRight() == false || mario.GetSkidBool() == true && mario.IsGoingRight() == true) && boxHit == 2))
                        {
                            mario.ResetXVelocity();

                            resetPosition = walls.GetWorldWalls(i).X + walls.GetWorldWalls(i).Width - 6;

                            mario.SetCharacterX(resetPosition);
                        }
                    }
                }
            }

            for (int i = 0; i < coinBlocks.Count; i++)
            {
                if (cameraScreen.Intersects(coinBlocks[i].GetDest()))
                {
                    int boxHit = mario.GetInterception(coinBlocks[i].GetDest());

                    if (boxHit >= 0)
                    {
                        if (boxHit == 1)
                        {
                            if (i != 11 || i == 11 && coinBlocks[11].GetIsHit())
                            {
                                if (mario.GetInAir() == true)
                                {
                                    mario.SetIsJumpBtnPressed(false);
                                }

                                mario.ResetYVelocity();
                                mario.SetIsInAir(false);
                                mario.SetIsBouncing(false);

                                resetPosition = coinBlocks[i].GetDest().Y - mario.GetDest().Height + 4;

                                mario.SetCharacterY(resetPosition);
                            }
                        }
                        if (boxHit == 0 && mario.GetYVelocity() < 0)
                        {
                            mario.ResetYVelocity();
                            resetPosition = coinBlocks[i].GetDest().Y + mario.GetDest().Height;
                            mario.SetCharacterY(resetPosition);
                            bumpSound.Play();

                            if (coinBlocks[i].GetIsHit() == false)
                            {
                                coinBlocks[i].BeginBounce();
                                coinsCounted++;
                                coinSound.Play();
                            }
                        }
                        else if ((mario.IsGoingRight() == true || mario.GetSkidBool() == true && mario.IsGoingRight() == false) && boxHit == 3)
                        {
                            if (i != 11 || i == 11 && coinBlocks[11].GetIsHit())
                            {
                                mario.ResetXVelocity();

                                resetPosition = coinBlocks[i].GetDest().X - mario.GetDest().Width - 3;

                                mario.SetCharacterX(resetPosition);
                            }
                        }
                        else if ((mario.IsGoingRight() == false || mario.GetSkidBool() == true && mario.IsGoingRight() == true) && boxHit == 2)
                        {
                            if (i != 11 || i == 11 && coinBlocks[11].GetIsHit())
                            {
                                mario.ResetXVelocity();

                                resetPosition = coinBlocks[i].GetDest().X + coinBlocks[i].GetDest().Width + 3;

                                mario.SetCharacterX(resetPosition);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < coinBanks.Count; i++)
            {
                if (cameraScreen.Intersects(coinBanks[i].GetDest()))
                {
                    int boxHit = mario.GetInterception(coinBanks[i].GetDest());

                    if (boxHit >= 0)
                    {
                        if (boxHit == 1)
                        {
                            if (mario.GetInAir() == true)
                            {
                                mario.SetIsJumpBtnPressed(false);
                            }

                            mario.ResetYVelocity();
                            mario.SetIsInAir(false);
                            mario.SetIsBouncing(false);

                            resetPosition = coinBanks[i].GetDest().Y - mario.GetDest().Height + 4;

                            mario.SetCharacterY(resetPosition);
                        }
                        else if (boxHit == 0)
                        {
                            mario.ResetYVelocity();
                            resetPosition = coinBanks[i].GetDest().Y + mario.GetDest().Height;
                            mario.SetCharacterY(resetPosition);
                            bumpSound.Play();

                            if (coinBanks[i].GetIsDisabled() == false)
                            {
                                coinBanks[i].ResetEverything();
                                coinBanks[i].BeginBounce();
                                coinsCounted++;
                                coinSound.Play();
                            }
                        }
                        else if ((mario.IsGoingRight() == true || mario.GetSkidBool() == true && mario.IsGoingRight() == false) && boxHit == 3)
                        {
                            mario.ResetXVelocity();

                            resetPosition = coinBanks[i].GetDest().X - mario.GetDest().Width - 3;

                            mario.SetCharacterX(resetPosition);
                        }
                        else if ((mario.IsGoingRight() == false || mario.GetSkidBool() == true && mario.IsGoingRight() == true) && boxHit == 2)
                        {
                            mario.ResetXVelocity();

                            resetPosition = coinBanks[i].GetDest().X + coinBanks[i].GetDest().Width + 3;

                            mario.SetCharacterX(resetPosition);
                        }
                    }
                }
            }

            for (int i = 0; i < bricks.Count; i++)
            {
                if (cameraScreen.Intersects(bricks[i].GetDest()))
                {
                    int boxHit = mario.GetInterception(bricks[i].GetDest());

                    if (boxHit >= 0)
                    {
                        if (boxHit == 1)
                        {
                            if (mario.GetInAir() == true)
                            {
                                mario.SetIsJumpBtnPressed(false);
                            }

                            mario.ResetYVelocity();
                            mario.SetIsInAir(false);
                            mario.SetIsBouncing(false);

                            resetPosition = bricks[i].GetDest().Y - mario.GetDest().Height + 4;

                            mario.SetCharacterY(resetPosition);
                        }
                        else if (boxHit == 0)
                        {
                            mario.ResetYVelocity();
                            resetPosition = bricks[i].GetDest().Y + mario.GetDest().Height;
                            mario.SetCharacterY(resetPosition);

                            if (mario.GetMarioState() == 0)
                            {
                                bricks[i].BeginBounce();
                                bumpSound.Play();
                            }
                        }
                        else if ((mario.IsGoingRight() == true || mario.GetSkidBool() == true && mario.IsGoingRight() == false) && boxHit == 3)
                        {
                            mario.ResetXVelocity();

                            resetPosition = bricks[i].GetDest().X - mario.GetDest().Width - 3;

                            mario.SetCharacterX(resetPosition);
                        }
                        else if ((mario.IsGoingRight() == false || mario.GetSkidBool() == true && mario.IsGoingRight() == true) && boxHit == 2)
                        {
                            mario.ResetXVelocity();

                            resetPosition = bricks[i].GetDest().X + bricks[i].GetDest().Width + 3;

                            mario.SetCharacterX(resetPosition);
                        }
                    }
                }
            }
        }

        public void ActiveGoomba(int i)
        {
            Rectangle newCameraScreen = cameraScreen;
            newCameraScreen.Width += screenWidth;

            if (newCameraScreen.Intersects(goombas[i].GetDest()))
            {
                if (goombas[i].GetIsDead() == false && goombas[i].GetIsAlternateDead() == false)
                {
                    CheckGoombaWallCollisions(i);
                    CheckGoombaEnemyCollisions(i);
                }
            }

            if (cameraScreen.Intersects(goombas[i].GetDest()))
            {
                goombas[i].AnimateGoomba();

                if (goombas[i].GetIsDead() == false && goombas[i].GetIsAlternateDead() == false)
                {
                    goombas[i].MoveXConstantly();
                    goombas[i].MoveYGravity();
                    CheckGoombaStomped(i);
                }
                else if (goombas[i].GetIsDead() == true)
                {
                    goombas[i].IncrimentDeathTimer();

                    if (goombas[i].IsDeathTimerUp() == true)
                    {
                        goombas.RemoveAt(i);
                    }
                }
                else if (goombas[i].GetIsAlternateDead() == true)
                {
                    goombas[i].MoveXConstantly();
                    goombas[i].MoveYGravity();
                }
            }
        }

        public void CheckGoombaWallCollisions(int j)
        {
            for (int i = 0; i < wallsInGame; i++)
            {
                int boxHit = goombas[j].GetInterception(walls.GetWorldWalls(i));

                if (boxHit >= 0)
                {
                    if (boxHit == 3)
                    {
                        goombas[j].ChangeDirection();
                    }
                    else if (boxHit == 2)
                    {
                        goombas[j].ChangeDirection();
                    }
                    else if (boxHit == 1)
                    {
                        int newYPoint = walls.GetWorldWalls(i).Y - goombas[j].GetDest().Height + 4;

                        goombas[j].SetCharacterY(newYPoint);
                        goombas[j].ResetYVelocity();
                    }
                }
            }

            for (int i = 0; i < bricks.Count; i++)
            {
                int boxHit = goombas[j].GetInterception(bricks[i].GetDest());

                if (boxHit == 1)
                {
                    if (bricks[i].GetIsBouncing() == false && goombas[j].GetIsAlternateDead() == false)
                    {
                        int newYPoint = bricks[i].GetDest().Y - goombas[j].GetDest().Height + 4;

                        goombas[j].SetCharacterY(newYPoint);
                        goombas[j].ResetYVelocity();
                    }
                    else
                    {
                        goombas[j].SetIsAlternateDead();
                        goombas[j].DeathJump();
                        kickShellSound.Play();
                    }
                }
            }

            for (int i = 0; i < coinBlocks.Count; i++)
            {
                int boxHit = goombas[j].GetInterception(coinBlocks[i].GetDest());

                if (boxHit == 1)
                {
                    if (coinBlocks[i].GetIsBouncing() == false && goombas[j].GetIsAlternateDead() == false)
                    {
                        int newYPoint = coinBlocks[i].GetDest().Y - goombas[j].GetDest().Height + 4;

                        goombas[j].SetCharacterY(newYPoint);
                        goombas[j].ResetYVelocity();
                    }
                    else
                    {
                        goombas[j].SetIsAlternateDead();
                        goombas[j].DeathJump();
                        kickShellSound.Play();
                    }
                }
            }
        }

        public void CheckGoombaEnemyCollisions(int j)
        {
            for (int i = 0; i < goombas.Count; i++)
            {
                int boxHit = goombas[j].GetInterception(goombas[i].GetDest());

                if (boxHit >= 2)
                {
                    goombas[j].ResetXVelocity();

                    if (goombas[j].IsGoingRight() == true)
                    {
                        goombas[j].SetCharacterX(goombas[j].GetDest().X - 1);
                    }
                    else
                    {
                        goombas[j].SetCharacterX(goombas[j].GetDest().X + 1);
                    }

                    goombas[j].ChangeDirection();

                    if (goombas[j].IsGoingRight() == true)
                    {
                        goombas[i].SetCharacterX(goombas[i].GetDest().X - 1);
                    }
                    else
                    {
                        goombas[i].SetCharacterX(goombas[i].GetDest().X + 1);
                    }

                    goombas[i].ChangeDirection();
                }
            }

            for (int i = 0; i < koopas.Count; i++)
            {
                int boxHit = goombas[j].GetInterception(koopas[i].GetDest());

                if (koopas[i].GetIsSliding() == true && boxHit >= 0)
                {
                    goombas[j].SetIsAlternateDead();
                    goombas[j].DeathJump();
                    kickShellSound.Play();
                }

                if (boxHit >= 2 && goombas[j].GetIsAlternateDead() == false)
                {
                    goombas[j].ResetXVelocity();

                    if (goombas[j].IsGoingRight() == true)
                    {
                        goombas[j].SetCharacterX(goombas[j].GetDest().X - 1);
                    }
                    else
                    {
                        goombas[j].SetCharacterX(goombas[j].GetDest().X + 1);
                    }

                    goombas[j].ChangeDirection();

                    if (goombas[j].IsGoingRight() == true)
                    {
                        koopas[i].SetCharacterX(koopas[i].GetDest().X - 1);
                    }
                    else
                    {
                        koopas[i].SetCharacterX(koopas[i].GetDest().X + 1);
                    }

                    if (koopas[i].GetIsInShell() == false)
                    {
                        koopas[i].ChangeDirection();
                    }
                    
                }    
            }
        }

        public void CheckGoombaStomped(int j)
        {
            {
                if (mario.GetIsDead() == false)
                {
                    if (mario.GetHitboxRec(1).Intersects((goombas[j].GetHitboxRec(0))) && mario.GetYVelocity() > 0)
                    {
                        mario.SetIsBouncing(true);
                        goombas[j].SetIsDead();
                        mario.SetIsInAir(false);
                        squishSound.Play();
                        mario.InitiateJump();
                    }
                    else if (mario.GetHitboxRec(2).Intersects((goombas[j].GetHitboxRec(3))) || mario.GetHitboxRec(3).Intersects((goombas[j].GetHitboxRec(2))))
                    {
                        InitialMarioDeath();
                    }
                }
            }
        }

        public void ActiveKoopa(int i)
        {
            Rectangle newCameraScreen = cameraScreen;
            newCameraScreen.Width += screenWidth;

            if ((cameraScreen.Intersects(koopas[i].GetDest()) && koopas[i].GetIsSliding() == false) || (newCameraScreen.Intersects(koopas[i].GetDest()) && koopas[i].GetIsSliding() == true))
            {
                koopas[i].AnimateKoopa();

                if (koopas[i].GetIsAlternateDead() == false )
                {
                    CheckKoopaWallCollisions(i);
                    koopas[i].MoveXConstantly();
                    koopas[i].MoveYGravity();
                    CheckKoopaStomped(i);
                }
            }
        }

        public void CheckKoopaWallCollisions(int j)
        {
            for (int i = 0; i < wallsInGame; i++)
            {
                int boxHit = koopas[j].GetInterception(walls.GetWorldWalls(i));

                if (boxHit >= 0)
                {
                    if (boxHit == 3)
                    {
                        if (koopas[j].GetIsSliding() == true)
                        {
                            int xPoint = walls.GetWorldWalls(i).X - koopas[j].GetDest().Width - 12;
                            koopas[j].SetCharacterX(xPoint);
                            bumpSound.Play();
                            koopas[j].KickShellRight();

                        }
                        else
                        {
                            koopas[j].ChangeDirection();
                        }
                    }
                    else if (boxHit == 2)
                    {
                        if (koopas[j].GetIsSliding() == true)
                        {
                            int xPoint = walls.GetWorldWalls(i).X + 12;
                            koopas[j].SetCharacterX(xPoint);
                            bumpSound.Play();
                            koopas[j].KickShellLeft();
                        }
                        else
                        {
                            koopas[j].ChangeDirection();
                        }
                    }
                    else if (boxHit == 1)
                    {
                        int newYPoint = walls.GetWorldWalls(i).Y - koopas[j].GetDest().Height + 4;

                        koopas[j].SetCharacterY(newYPoint);
                        koopas[j].ResetYVelocity();
                    }

                }
            }
        }

        public void CheckKoopaStomped(int j)
        {
            {
                if (mario.GetIsDead() == false)
                {
                    if (mario.GetHitboxRec(1).Intersects((koopas[j].GetHitboxRec(0))) && mario.GetYVelocity() > 0)
                    {
                        if (koopas[j].GetIsInShell() == false)
                        {
                            mario.SetIsBouncing(true);
                            koopas[j].GoIntoShell();
                            koopas[j].SetCharacterY(koopas[j].GetDest().Y);
                            koopas[j].SetInShellHeight();
                            mario.SetIsInAir(false);
                            squishSound.Play();
                            mario.InitiateJump();
                        }
                        if (koopas[j].GetIsSliding() == true)
                        {
                            mario.SetIsBouncing(true);
                            koopas[j].SetIsSliding(false);
                            koopas[j].ResetXVelocity();
                            mario.SetIsInAir(false);
                            squishSound.Play();
                            mario.InitiateJump();
                        }
                    }
                    else if (mario.GetHitboxRec(2).Intersects((koopas[j].GetHitboxRec(3))) || mario.GetHitboxRec(3).Intersects((koopas[j].GetHitboxRec(2))))
                    {
                        if (koopas[j].GetIsInShell() == false)
                        {
                            InitialMarioDeath();
                        }
                        else
                        {
                            if (mario.GetHitboxRec(2).Intersects((koopas[j].GetHitboxRec(3))) && koopas[j].GetIsSliding() == false)
                            {
                                koopas[j].KickShellRight();
                                kickShellSound.Play();
                                koopas[j].SetIsSliding(true);
                            }
                            else if (mario.GetHitboxRec(3).Intersects((koopas[j].GetHitboxRec(2))) && koopas[j].GetIsSliding() == false)
                            {
                                koopas[j].KickShellLeft();
                                kickShellSound.Play();
                                koopas[j].SetIsSliding(true);
                            }

                            if ((mario.GetHitboxRec(2).Intersects((koopas[j].GetHitboxRec(3))) && koopas[j].GetIsSliding() == true && koopas[j].GetXVelocity() == 10f))
                            {
                                InitialMarioDeath();
                            }
                            else if ((mario.GetHitboxRec(3).Intersects((koopas[j].GetHitboxRec(2))) && koopas[j].GetIsSliding() == true && koopas[j].GetXVelocity() == -10f))
                            {
                                InitialMarioDeath();
                            }
                        }
                    }
                }
            }
        }

        public void InitialMarioDeath()
        {
            MediaPlayer.Stop();
            marioDeathSound.Play();
            mario.SetIsDead();
            mario.ResetXVelocity();
            mario.ResetYVelocity();
        }

        public void DoBounceBrick()
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i].GetIsBouncing() == true)
                {
                    bricks[i].ApplyGravity();

                    if (bricks[i].GetDest().Y >= bricks[i].GetOriginalDest().Y)
                    {
                        bricks[i].SetDestRec(bricks[i].GetOriginalDest());
                        bricks[i].ResetYVelocity();
                    }
                }
            }
        }

        public void DoBounceCoinBlock()
        {
            for (int i = 0; i < coinBlocks.Count; i++)
            {
                if (coinBlocks[i].GetIsBouncing() == true)
                {
                    coinBlocks[i].ApplyGravity();

                    if (coinBlocks[i].GetDest().Y >= coinBlocks[i].GetOriginalDest().Y)
                    {
                        coinBlocks[i].SetDestRec(coinBlocks[i].GetOriginalDest());
                        coinBlocks[i].ResetYVelocity();
                    }
                }

                if (coinBlocks[i].GetIsHit() == true && coinBlocks[i].GetIsCoinVisible() == true)
                {
                    coinBlocks[i].ApplyCoinGravity();
                }
            }
        }

        public void AnimateCoinBlocks()
        {
            for (int i = 0; i < coinBlocks.Count; i++)
            {
                if (i != 13)
                {
                    coinBlocks[i].AnimateBlock();
                }
                coinBlocks[i].AnimateCoin();
            }

            for (int i = 0; i < coinBanks.Count; i++)
            {
                coinBanks[i].AnimateBlock();
                coinBanks[i].AnimateCoin();
            }
        }

        public void DoBounceCoinBank()
        {
            for (int i = 0; i < coinBanks.Count; i++)
            {
                if (coinBanks[i].GetIsBouncing() == true)
                {
                    coinBanks[i].ApplyGravity();

                    if (coinBanks[i].GetDest().Y >= coinBanks[i].GetOriginalDest().Y)
                    {
                        coinBanks[i].SetDestRec(coinBanks[i].GetOriginalDest());
                        coinBanks[i].ResetYVelocity();
                    }
                }

                if (coinBanks[i].GetIsDisabled() == false && coinBanks[i].GetIsCoinVisible() == true)
                {
                    coinBanks[i].ApplyCoinBankGravity();
                }
            }
        }
    }
}
