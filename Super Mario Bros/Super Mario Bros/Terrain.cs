// Author: Eduard Varshavsky
// File Name: Terrain.cs
// Project Name: Super Mario Bros
// Creation Date: October 7, 2017
// Modified Date: October 8, 2017
// Description: This class is created to hold data for the static walls and floors in the world
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
    class Terrain : MainGame
    {
        //Attributes

        //Created an array to hold data for the world and integers for the dimensions of the data
        private Rectangle[] world = new Rectangle[38];
        private int backGroundWidth;
        private int blockDimension;
        private int upOffset;

        //Constructor

        //Pre:  backGroundWidth, blockDimension, and upOffset are set with appropriate values for dimensions
        //Post: Gives the integers with the correct values and creates an instance of Terrain
        //Desc: Called MainGame to create an instance of Terrain and to create the dimensions to be used for creation
        public Terrain()
        {
            backGroundWidth = 2048;
            blockDimension = 64;
            upOffset = blockDimension - 8;
        }

        //Getters

        //Pre:  Requires the value of i to determine which data part to get from the world array
        //Post: Returns the specific data for the world array
        //Desc: Used as a getter when determining collision with the world rectangles
        public Rectangle GetWorldWalls(int i)
        {
            //Gives the specific rectangle for the world array
            return world[i];
        }

        //Behaviours

        //Pre:  Requires the dimensions (bacGroundWidth, blockDimension, upOffset) and the array for the world
        //Post: Sets each value of the world array with a specific rectangle
        //Desc: Sets specific value for each world array value and thus acts as a static rectangle wall to be
        //      used in game
        public void buildWalls()
        {
            //Creates a new rectangle to be set for each data value in an array
            world[0] = new Rectangle(0, 832, backGroundWidth * 2 + 320, 128);
            world[1] = new Rectangle(1792, 704, 128, 128);
            world[2] = new Rectangle(384 + backGroundWidth, 640, 128, 192);
            world[3] = new Rectangle(896 + backGroundWidth, 576, 128, 256);
            world[4] = new Rectangle(1600 + backGroundWidth, 576, 128, 256);
            world[5] = new Rectangle(448 + backGroundWidth * 2, 832, 960, 128);
            world[6] = new Rectangle(1600 + backGroundWidth * 2, 832, 448 + backGroundWidth + 1600, 128);

            //Made a for loop to set values of the world array which share similar dimensions
            for (int i = 0; i < 4; i++)
            {
                world[7 + i] = new Rectangle(384 + backGroundWidth * 4 + (blockDimension * i), 768 - (blockDimension * i), blockDimension, blockDimension + (blockDimension * i));
            }

            //Made a for loop to set values of the world array which share similar dimensions
            for (int i = 0; i < 4; i++)
            {
                //Sets the data for the world array based on the value of i, modifying certain aspects of the rectangle
                world[11 + i] = new Rectangle(768 + backGroundWidth * 4 + (blockDimension * i), 576 + (blockDimension * i), blockDimension, 256 - (blockDimension * i));
            }

            //Made a for loop to set values of the world array which share similar dimensions
            for (int i = 0; i < 4; i++)
            {
                //Sets the data for the world array based on the value of i, modifying certain aspects of the rectangle
                world[15 + i] = new Rectangle(1280 + backGroundWidth * 4 + (blockDimension * i), 768 - (blockDimension * i), blockDimension, blockDimension + (blockDimension * i));
            }

            //Creates a new rectangle to be set for each data value in an array
            world[19] = new Rectangle(1536 + backGroundWidth * 4, 576, blockDimension, 256);

            for (int i = 0; i < 4; i++)
            {
                //Sets the data for the world array based on the value of i, modifying certain aspects of the rectangle
                world[20 + i] = new Rectangle(1728 + backGroundWidth * 4 + (blockDimension * i), 576 + (blockDimension * i), blockDimension, 256 - (blockDimension * i));
            }

            //Creates a new rectangle to be set for each data value in an array
            world[24] = new Rectangle(1728 + backGroundWidth * 4, 832, 320, 128);
            world[25] = new Rectangle(192 + backGroundWidth * 5, 704, 128, 128);
            world[26] = new Rectangle(1216 + backGroundWidth * 4 + (backGroundWidth / 2) - blockDimension * 3, 832, backGroundWidth * 2, 128);

            //Made a for loop to set values of the world array which share similar dimensions
            for (int i = 0; i < 8; i++)
            {
                //Sets the data for the world array based on the value of i, modifying certain aspects of the rectangle
                world[27 + i] = new Rectangle(1344 + backGroundWidth * 5 + (blockDimension * i), 768 - (blockDimension * i), blockDimension, blockDimension + (blockDimension * i));
            }

            //Creates a new rectangle to be set for each data value in an array
            world[35] = new Rectangle(1856 + backGroundWidth * 5, 320, blockDimension, blockDimension * 8);
            world[36] = new Rectangle(384 + backGroundWidth * 6, 768, blockDimension, blockDimension);
            world[37] = new Rectangle(1216 + backGroundWidth * 5, 704, 128, 128);
        }
    }
}
