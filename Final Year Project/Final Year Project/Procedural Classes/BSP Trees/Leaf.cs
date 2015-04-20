using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.Xna.Framework;

namespace Multiplayer_Software_Game_Engineering.Procedural_Classes.BSP_Trees
{
    internal class Leaf
    {
        private const int minLeafSize = 6;
        public int x, y, width, height; // the position and size of this Leaf

        public Leaf leftChild; // the Leaf's left child Leaf
        public Leaf rightChild; // the Leaf's right child Leaf
        public Rectangle room; // the room that is inside this Leaf
        public List<Rectangle> halls; // hallways to connect this Leaf to other Leafs
        Random rand = new Random();

        public Leaf(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Boolean split()
        {
            // begin splitting the leaf into two children
            if (leftChild != null || rightChild != null)
                return false; 

            // determine direction of split
            // if the width is >25% larger than height, split vertically
            // if the height is >25% larger than the width, split horizontally
            // otherwise split randomly


            Boolean splitH = rand.NextDouble() > 0.5;
            if (width > height && height/width >= 0.05)
                splitH = false;
            else if (height > width && width/height >= 0.05)
                splitH = true;

            int max = (splitH ? height : width) - minLeafSize; // determine the maximum height or width
            if (max <= minLeafSize)
                return false; // the area is too small to split any more...

            int split = rand.Next(minLeafSize, max); // determine where we're going to split

            // create our left and right children based on the direction of the split
            if (splitH)
            {
                leftChild = new Leaf(x, y, width, split);
                rightChild = new Leaf(x, y + split, width, height - split);
            }
            else
            {
                leftChild = new Leaf(x, y, split, height);
                rightChild = new Leaf(x + split, y, width - split, height);
            }
            return true; // split successful!
        }

        public void createRooms()
        {
            // this function generates all the rooms and hallways for this Leaf and all of its children.
            if (leftChild != null || rightChild != null)
            {
                if (leftChild != null)
                {
                    leftChild.createRooms();
                }
                if (rightChild != null)
                {
                    rightChild.createRooms();
                }
                if (leftChild != null && rightChild != null)
                {
                    createHall(leftChild.getRoom(), rightChild.getRoom());
                }
            }
            else
            {
                Point roomSize, roomPos;
                roomSize = new Point(rand.Next(3, width - 2), rand.Next(3, height - 2));
                roomPos = new Point(rand.Next(1, width - roomSize.X - 1),
                    rand.Next(1, height - roomSize.Y - 1));

               


                room = new Rectangle(x + roomPos.Y, y + roomPos.Y, roomSize.X, roomSize.Y);

                

            }
        }

        public Rectangle getRoom()
        {
            if (!room.IsEmpty)
                return room;
            
            Rectangle lRoom = new Rectangle();
            Rectangle rRoom = new Rectangle();
            if (leftChild != null)
            {
                lRoom = leftChild.getRoom();
            }
            if (rightChild != null)
            {
                rRoom = rightChild.getRoom();
            }
            if (lRoom.IsEmpty && rRoom.IsEmpty)
                return new Rectangle();
            else if (rRoom.IsEmpty)
                return lRoom;
            else if (lRoom.IsEmpty)
                return rRoom;
            else if (rand.NextDouble() > .5)
                return lRoom;
            else
                return rRoom;
        }

        public void createHall(Rectangle l, Rectangle r)
        {
 
            halls = new List<Rectangle>();
 
            Point point1 = new Point(rand.Next(l.Left + 1, l.Right - 2), rand.Next(l.Top + 1, l.Bottom - 2));
            Point point2 = new Point(rand.Next(r.Left + 1, r.Right - 2), rand.Next(r.Top + 1, r.Bottom - 2));
 
            int w = point2.X - point1.X;
            int h = point2.Y - point1.Y;
 
            if (w < 0)
            {
                if (h < 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        halls.Add(new Rectangle(point2.X, point1.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.X, point2.Y, 1, Math.Abs(h)));
                    }
                }
                else if (h > 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        halls.Add(new Rectangle(point2.X, point1.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.X, point1.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                    }
                }
                else // if (h == 0)
                {
                    halls.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                }
            }
            else if (w > 0)
            {
                if (h < 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        halls.Add(new Rectangle(point1.X, point2.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.X, point2.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                    }
                }
                else if (h > 0)
                {
                    if (Math.Abs(rand.NextDouble() * 0.5) > 0.2)
                    {
                        halls.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.X, point1.Y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point1.X, point2.Y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                    }
                }
                else // if (h == 0)
                {
                    halls.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                }
            }
            else // if (w == 0)
            {
                if (h < 0)
                {
                    halls.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                }
                else if (h > 0)
                {
                    halls.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                }
            }
        }
    }
}

