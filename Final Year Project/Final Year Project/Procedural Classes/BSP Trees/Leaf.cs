using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Multiplayer_Software_Game_Engineering.Procedural_Classes.BSP_Trees
{
    class Leaf
    {
        private const int minLeafSize = 6;
        private const int maxLeafSize = 20;
        private readonly int x, y; 
        public readonly int width, height;
        public Rectangle currentRoom;
        public Leaf leftRoom, rightRoom;
        public List<Rectangle> pathways;
        readonly Random rand = new Random();

        public static int getMaxLeafSize()
        {
            return maxLeafSize;
        }

        public Leaf(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /**
         * Split this current leaf into 2 child leaves if possible
         */
        public Boolean SplitLeaf()
        {
            if (leftRoom != null || rightRoom != null)
                return false;                                                     // This leaf already has a left and right room

            Boolean splitHorizontally;                                            // Should the leaf be split vertically or horizontally

            if (height > width && (float)width / height >= .20)                   // If the leaf height is 20% larger than the leaf width, split horizontally
                splitHorizontally = true;
            else if (width > height && (float)height / width >= .20)              // If the leaf width is 20% larger than the leaf height, split vertically
                splitHorizontally = false;
            else
                splitHorizontally = rand.NextDouble() > .5;                       // Split the leaf in a random direction
 
            var maxSize = (splitHorizontally ? height : width) - minLeafSize;     // The maximum height or width of the new room

            if (maxSize <= minLeafSize)
                return false;                                                     // The room is already too small to split into further rooms

            var randomPoint = rand.Next(minLeafSize, maxSize);                    // A random point on the leaf to start the split                    

            leftRoom  = splitHorizontally ? new Leaf(x, y, width, randomPoint)                        : new Leaf(x, y, randomPoint, height);
            rightRoom = splitHorizontally ? new Leaf(x, y + randomPoint, width, height - randomPoint) : new Leaf(x + randomPoint, y, width - randomPoint, height);

            return true; 
        }

        /**
         * Create the rooms, and pathways between the rooms, for this leaf and all child leaves
         */
        public void GenerateRooms()
        {
            if (leftRoom != null || rightRoom != null)
            {
                if (leftRoom != null)
                    leftRoom.GenerateRooms();
                if (rightRoom != null)
                    rightRoom.GenerateRooms();
                if (leftRoom != null && rightRoom != null)
                    CreatePathway(leftRoom.GetRoom(), rightRoom.GetRoom());
            }
            else
            {
                var roomSize = new Point(rand.Next(3, width - 2),              rand.Next(3, height - 2));                   // Randomly generate the size of the room within the leaf
                var roomPos  = new Point(rand.Next(1, width - roomSize.X - 1), rand.Next(1, height - roomSize.Y - 1));      // Randomly generate the position of the room within the leaf

                currentRoom  = new Rectangle(x + roomPos.Y, y + roomPos.Y, roomSize.X, roomSize.Y);                         // Create a rectangle defining the current room
            }
        }

        /**
         * Return a rectangle defining the room of the leaf
         */
        private Rectangle GetRoom()
        {
            if (!currentRoom.IsEmpty)
                return currentRoom;                                                                                         // Return the current room if the current leaf has a room (It hasn't been split)
            
            var roomLeft  = new Rectangle();
            var roomRight = new Rectangle();

            if (leftRoom != null)
                roomLeft = leftRoom.GetRoom();
            if (rightRoom != null)
                roomRight = rightRoom.GetRoom();
            if (roomLeft.IsEmpty && roomRight.IsEmpty)
                return new Rectangle();
            if (roomRight.IsEmpty)
                return roomLeft;
            if (roomLeft.IsEmpty)
                return roomRight;

            return rand.NextDouble() > .5 ? roomLeft : roomRight;
        }

        private void CreatePathway(Rectangle leftRect, Rectangle rightRect)
        {
            pathways = new List<Rectangle>();
 
            var point1 = new Point(rand.Next(leftRect.Left + 1,  leftRect.Right - 2),  rand.Next(leftRect.Top + 1,  leftRect.Bottom - 2));
            var point2 = new Point(rand.Next(rightRect.Left + 1, rightRect.Right - 2), rand.Next(rightRect.Top + 1, rightRect.Bottom - 2));
 
            var w = point2.X - point1.X;
            var h = point2.Y - point1.Y;
 
            if (w < 0)
            {
                if (h < 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        pathways.Add(new Rectangle(point2.X, point1.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        pathways.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point1.X, point2.Y, 1, Math.Abs(h)));
                    }
                }
                else if (h > 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        pathways.Add(new Rectangle(point2.X, point1.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point2.X, point1.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        pathways.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                    }
                }
                else // if (h == 0)
                {
                    pathways.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                }
            }
            else if (w > 0)
            {
                if (h < 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        pathways.Add(new Rectangle(point1.X, point2.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point1.X, point2.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        pathways.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                    }
                }
                else if (h > 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        pathways.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point2.X, point1.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        pathways.Add(new Rectangle(point1.X, point2.Y, Math.Abs(w), 1));
                        pathways.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                    }
                }
                else // if (h == 0)
                {
                    pathways.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                }
            }
            else // if (w == 0)
            {
                if (h < 0)
                {
                    pathways.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                }
                else if (h > 0)
                {
                    pathways.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                }
            }
        }
    }
}