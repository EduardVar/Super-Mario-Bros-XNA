// Author: Eduard Varshavsky
// File Name: Level.cs
// Project Name: Super Mario Bros
// Creation Date: October 9, 2017
// Modified Date: October 15, 2017
// Description: This class is created to hold data for non-static level objects
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
    class Level : Terrain
    {
        //Attributes

        //Create int to hold dimension of a square block
        private int blockDim = 64;

        //Constructor

        //Pre:  Doesn't require anything to construct
        //Post: Creates an instance of Level
        //Desc: Called MainGame to create an instance of Level and to act as a base to set the positions
        //      of all non-static objects
        public Level()
        {
        }

        //Setters

        //Pre:  Requires the user to call this Setter
        //Post: Returns a list of all the Goomba objects in the level
        //Desc: Used as a getter that sets a list of goombas in MainGame to be used for update purposes 
        //      and gameplay
        public List<Goomba> PopulateGoombas()
        {
            //Created a list to hold new Goomba objects
            List<Goomba> goombas = new List<Goomba>();

            //Created new Goomba objects while inputing their positions to set their destRec in the constructor
            goombas.Add(new Goomba(new Rectangle(1408, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(2560, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(3264, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(3360, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(5120, 260, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(5248, 260, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(6208, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(6304, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(7296, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(7392, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(7936, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(8032, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(8192, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(8288, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(11136, 772, blockDim, blockDim)));
            goombas.Add(new Goomba(new Rectangle(11232, 772, blockDim, blockDim)));

            //Returns the list of goomba objects
            return goombas;
        }

        //Pre:  Requires the user to call this Setter
        //Post: Returns a list of all the Koopa objects in the level
        //Desc: Used as a getter that sets a list of koombas in MainGame to be used for update purposes 
        //      and gameplay
        public List<Koopa> PopulateKoopas()
        {
            //Created a list to hold new Koomba objects
            List<Koopa> koopas = new List<Koopa>();

            //Created new Koomba object while inputing its positions to set its destRec in the constructor
            koopas.Add(new Koopa(new Rectangle (6848, 744, blockDim, 92)));

            //Returns the list of Koopa objects
            return koopas;
        }

        //Pre:  Requires the user to call this Setter
        //Post: Returns a list of all the Brick objects in the level
        //Desc: Used as a getter that sets a list of bricks in MainGame to be used for update purposes 
        //      and gameplay
        public List<Brick> PopulateBricks()
        {
            //Created a list to hold new Brick objects
            List<Brick> bricks = new List<Brick>();

            //Created new Brick object while inputing its positions to set its destRec in the constructor
            bricks.Add(new Brick(new Rectangle(1280, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(1408, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(1536, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(4928, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(5056, 576, blockDim, blockDim)));

            //Created for loop that creates Brick Objects that share similar positions
            for (int i = 0; i < 8; i++)
            {
                //Adds a new Brick object based on which value i is in the for loop
                bricks.Add(new Brick(new Rectangle(5120 + (i * blockDim), 320, blockDim, blockDim)));
            }

            //Created for loop that creates Brick Objects that share similar positions
            for (int i = 0; i < 3; i++)
            {
                //Adds a new Brick object based on which value i is in the for loop
                bricks.Add(new Brick(new Rectangle(5824 + (i * blockDim), 320, blockDim, blockDim)));
            }

            //Created new Brick object while inputing its positions to set its destRec in the constructor
            bricks.Add(new Brick(new Rectangle(6400, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(7552, 576, blockDim, blockDim)));

            //Created for loop that creates Brick Objects that share similar positions
            for (int i = 0; i < 3; i++)
            {
                //Adds a new Brick object based on which value i is in the for loop
                bricks.Add(new Brick(new Rectangle(7744 + (i * blockDim), 320, blockDim, blockDim)));
            }

            //Created new Brick object while inputing its positions to set its destRec in the constructor
            bricks.Add(new Brick(new Rectangle(8192, 320, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(8384, 320, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(8256, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(8320, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(10752, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(10816, 576, blockDim, blockDim)));
            bricks.Add(new Brick(new Rectangle(10944, 576, blockDim, blockDim)));

            //Returns the list of Brick objects
            return bricks;
        }

        //Pre:  Requires the user to call this Setter
        //Post: Returns a list of all the CoinBlock objects in the level
        //Desc: Used as a getter that sets a list of coin blocks in MainGame to be used for update purposes 
        //      and gameplay
        public List<CoinBlock> PopulateCoinBlocks()
        {
            //Created a list to hold new CoinBlock objects
            List<CoinBlock> coinBlocks = new List<CoinBlock>();

            //Created new CoinBlock objects while inputing their positions to set their destRec in the constructor
            coinBlocks.Add(new CoinBlock(new Rectangle(1024, 576, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(1408, 320, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(1472, 576, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(6016, 320, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(6784, 576, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(6976, 576, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(7168, 576, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(8256, 320, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(8320, 320, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(10880, 576, blockDim, blockDim)));

            coinBlocks.Add(new CoinBlock(new Rectangle(1344, 576, blockDim, blockDim)));
            //InvisibleBlock (12th)
            coinBlocks.Add(new CoinBlock(new Rectangle(4096, 512, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(4992, 576, blockDim, blockDim)));
            //Starblock (14th)
            coinBlocks.Add(new CoinBlock(new Rectangle(6464, 576, blockDim, blockDim)));
            coinBlocks.Add(new CoinBlock(new Rectangle(6976, 320, blockDim, blockDim)));

            //Returns the list of CoinBlock objects
            return coinBlocks;
        }

        //Pre:  Requires the user to call this Setter
        //Post: Returns a list of all the CoinBank objects in the level
        //Desc: Used as a getter that sets a list of coin banks in MainGame to be used for update purposes 
        //      and gameplay
        public List<CoinBank> PopulateCoinBanks()
        {
            //Created a list to hold new CoinBan objects
            List<CoinBank> coinBanks = new List<CoinBank>();

            //Created new CoinBank objects while inputing their positions to set their destRec in the constructor
            coinBanks.Add(new CoinBank(new Rectangle(6016, 576, blockDim, blockDim)));

            //Returns the list of CoinBank objects
            return coinBanks;
        }
    }
}
