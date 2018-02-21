using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Tetris_game
{

    public class Square
    {
        public Point location;
        public Size size;
        public static System.IntPtr hand; 

        public Square(Point location,Size size)
        {
            this.location = location;
            this.size = size;
        }
        public void Draw(System.IntPtr handler)
        {
            Graphics g = Graphics.FromHwnd(handler);
            GraphicsPath gp = new GraphicsPath();
            Rectangle rec = new Rectangle(location, size);
            gp.AddRectangle(rec);
            Color[] surroundColor = new Color[] { Colour.BackCo };
            PathGradientBrush pb = new PathGradientBrush(gp);
            pb.CenterColor = Colour.Square;
            pb.SurroundColors = surroundColor;
            g.FillPath(pb, gp);
        }

        public void Delete(System.IntPtr handler)
        {
            Graphics g = Graphics.FromHwnd(handler);
            Rectangle rec = new Rectangle(location, size);
            g.FillRectangle(new SolidBrush(Colour.BackCo), rec);
        }
    }

    public class BlockTypes
    {
        Random rand = new Random();
        public int RandomType()
        {
            int x;
            x = rand.Next(0,6);
            return x;
        }
    }
    public class Block
    {
        public int type;
        public Point location;
        public State BlockState = new North();
        public const int width = 12;           
        public const int height = 25;
        public const int squareSize = 20;     
        public static System.IntPtr handler; 
        public static bool isChanged = false;
        public static Square[,] SqPanel = new Square[height+1, width+1];
        public static int[,] IntPanel = new int[height+1, width+1];  
        public Square square1;  
        public Square square2;
        public Square square3;
        public Square square4;

        public Block(Point TransLocation)
        {
            this.location = TransLocation;
            Point Squarelocation = new Point();
            Size size = new Size(squareSize, squareSize);
            square1 = new Square(Squarelocation, size);
            square2 = new Square(Squarelocation, size);
            square3 = new Square(Squarelocation, size);
            square4 = new Square(Squarelocation, size);
            for (int i = 0; i < width+1; i++)
            {
                IntPanel[height, i] = 1;
            }
            for (int i = 0; i < height+1; i++)
            {
                IntPanel[i, width] = 1;
            }    
        }

        public void DrawObject(System.IntPtr handler)
        {
            square1.Draw(handler);
            square2.Draw(handler);
            square3.Draw(handler);
            square4.Draw(handler);
        }

        public void DeleteObject(System.IntPtr handler)
        {
            square1.Delete(handler);
            square2.Delete(handler);
            square3.Delete(handler);
            square4.Delete(handler);
        }

        public static bool EmptySquare(int x, int y)
        {
            if ((x < 0) || (x > width)) { return false; }
            else if ((y < 0) || (y > height)) { return false; }
            else if (IntPanel[y, x] != 0)
            {
                return false;
            }
            else return true;
        }
        
        public static int Score()
        {
            int score = 0;
            int ZeroCount = 0;
            int OneCount = 0;
            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (IntPanel[i, j] == 0)
                    {
                        ZeroCount++;
                    }
                    if (IntPanel[i,j] == 1){
                        OneCount++;
                    }
                }
                if (ZeroCount == width)
                {
                    break;
                }
                if (OneCount == width)
                {
                    score++;
                    for (int j = 0; j < width; j++)
                    {
                        SqPanel[i, j] = null;
                        IntPanel[i, j] = 0;
                    }
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (IntPanel[k - 1, j] != 0)
                            {
                                SqPanel[k - 1, j].location = new Point(SqPanel[k - 1, j].location.X, SqPanel[k - 1, j].location.Y + squareSize);
                                SqPanel[k, j] = SqPanel[k - 1, j];
                                SqPanel[k - 1, j] = null;
                                IntPanel[k, j] = IntPanel[k - 1, j];
                                IntPanel[k - 1, j] = 0;
                            }
                        }
                    }
                    i++;
                }
                OneCount = 0;
                ZeroCount = 0;
            }
                return score;
        }

        public static void CurrentPanel(Square s)
        {
            int x = s.location.X / squareSize;
            int y = s.location.Y/ squareSize;
            IntPanel[y, x] = 1;
            SqPanel[y, x] = s;
        }
        public static void DeletePanel(){
            int CountZero = 0;
            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (IntPanel[i, j] == 0)
                    {
                        CountZero++;
                    }
                }
                if (CountZero == width)
                {
                    break;
                }
                for (int j = 0; j < width; j++)
                {
                    if (IntPanel[i, j] != 0)
                    {
                        SqPanel[i, j].Delete(handler);
                    }
                }
                CountZero = 0;
            }
        }
        public static void DrawPanel()
        {
            int CountZero = 0;
            for (int i = height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (IntPanel[i, j] == 0)
                        {
                            CountZero++;
                        }
                    }
                    if (CountZero == width)
                    {
                        break;
                    }
                    for (int j = 0; j < width; j++)
                    {
                        if (IntPanel[i, j] != 0)
                        {
                            SqPanel[i, j].Draw(Block.handler);
                        }
                    }
                    CountZero = 0;
                }
        }

        public bool Down()
        {
            if (EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize + 1) &&
               EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize + 1) &&
               EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize + 1) &&
               EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize + 1))
           {
                DeleteObject(Block.handler);
                square1.location = new Point(square1.location.X, square1.location.Y + squareSize);
                square2.location = new Point(square2.location.X, square2.location.Y + squareSize);
                square3.location = new Point(square3.location.X, square3.location.Y + squareSize);
                square4.location = new Point(square4.location.X, square4.location.Y + squareSize);
                DrawObject(Block.handler);
                return true;
            }
            else
            {
                Form1.shiftenable = false;
                DeleteObject(Block.handler);
                CurrentPanel(square1);
                CurrentPanel(square2);
                CurrentPanel(square3);
                CurrentPanel(square4);
                return false;
           }
           
        }
        public void Left()
        {
            if (!Form1.shiftenable) { return; }
            else
            {
                if (EmptySquare(square1.location.X / squareSize - 1, square1.location.Y / squareSize) &&
                    EmptySquare(square2.location.X / squareSize - 1, square2.location.Y / squareSize) &&
                    EmptySquare(square3.location.X / squareSize - 1, square3.location.Y / squareSize) &&
                    EmptySquare(square4.location.X / squareSize - 1, square4.location.Y / squareSize))
                {
                    DeleteObject(Block.handler);
                    square1.location = new Point(square1.location.X - squareSize, square1.location.Y);
                    square2.location = new Point(square2.location.X - squareSize, square2.location.Y);
                    square3.location = new Point(square3.location.X - squareSize, square3.location.Y);
                    square4.location = new Point(square4.location.X - squareSize, square4.location.Y);
                    DrawObject(Block.handler);

                }
            }
        }
        public void Right()
        {
            if (!Form1.shiftenable) { return; }
            else
            {
                if (EmptySquare(square1.location.X / squareSize + 1, square1.location.Y / squareSize) &&
                    EmptySquare(square2.location.X / squareSize + 1, square2.location.Y / squareSize) &&
                    EmptySquare(square3.location.X / squareSize + 1, square3.location.Y / squareSize) &&
                    EmptySquare(square4.location.X / squareSize + 1, square4.location.Y / squareSize))
                {
                    DeleteObject(Block.handler);
                    square1.location = new Point(square1.location.X + squareSize, square1.location.Y);
                    square2.location = new Point(square2.location.X + squareSize, square2.location.Y);
                    square3.location = new Point(square3.location.X + squareSize, square3.location.Y);
                    square4.location = new Point(square4.location.X + squareSize, square4.location.Y);
                    DrawObject(Block.handler);
                }
            }
        }
        public virtual bool rotate(State state){return true;}
        public void Up()
        {
            BlockState = BlockState.Rotate(this);
        }

        public int Top()
        {
            return Math.Min(square1.location.Y, Math.Min(square2.location.Y, Math.Min(square3.location.Y, square4.location.Y)));
        }
    }

           public class Sq : Block
            {
               public Sq(Point TransLocation)
                   : base(TransLocation)
               {
               type = 0;
                square1.location = new Point(location.X, location.Y);
                square2.location = new Point(location.X + squareSize, location.Y);
                square3.location = new Point(location.X, location.Y + squareSize);
                square4.location = new Point(location.X + squareSize, location.Y + squareSize);
               }
               public override bool rotate(State state)
               {
                   return true;
               }
            }
           public class Line : Block
            {
               public Line(Point TransLocation)
                   : base(TransLocation)
               {
                       type = 1;
                       square1.location = new Point(location.X, location.Y);
                       square2.location = new Point(location.X + squareSize, location.Y);
                       square3.location = new Point(location.X + 2 * squareSize, location.Y);
                       square4.location = new Point(location.X + 3 * squareSize, location.Y);
                   
               }
               public override bool rotate(State state)
               {

                   Point oldPosition1 = square1.location;
                   Point oldPosition2 = square2.location;
                   Point oldPosition3 = square3.location;
                   Point oldPosition4 = square4.location;

                   DeleteObject(Block.handler);
                   if (state.Direction == 0 || state.Direction == 2)
                   {
                       square1.location = new Point(square2.location.X, square2.location.Y - squareSize);
                       square3.location = new Point(square2.location.X, square2.location.Y + squareSize);
                       square4.location = new Point(square2.location.X, square2.location.Y + 2 * squareSize);
                   }
                   else if (state.Direction == 1 || state.Direction == 3)
                   {
                       square1.location = new Point(square2.location.X - squareSize, square2.location.Y);
                       square3.location = new Point(square2.location.X + squareSize, square2.location.Y);
                       square4.location = new Point(square2.location.X + 2 * squareSize, square2.location.Y);
                   }
                   if (!(EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize) &&
                EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize) &&
                EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize) &&
                EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize)))
                   {
                       square1.location = oldPosition1;
                       square2.location = oldPosition2;
                       square3.location = oldPosition3;
                       square4.location = oldPosition4;
                       return false;
                   }
                   else { return true; }
                   DrawObject(Block.handler);
               }

            }
            public class J: Block
            {
                public J(Point TransLocation)
                    : base(TransLocation)
                {
                   type =2;
                square1.location = new Point(location.X + squareSize, location.Y);
                square2.location = new Point(location.X + squareSize, location.Y + squareSize);
                square3.location = new Point(location.X + squareSize, location.Y + 2 * squareSize);
                square4.location = new Point(location.X, location.Y + 2 * squareSize);
                }
                public override bool rotate(State state)
                {

                    Point oldPosition1 = square1.location;
                    Point oldPosition2 = square2.location;
                    Point oldPosition3 = square3.location;
                    Point oldPosition4 = square4.location;

                    DeleteObject(Block.handler);
                    if (state.Direction == 0)
                    {
                        square1.location = new Point(square3.location.X + 2 * squareSize, square3.location.Y);
                        square2.location = new Point(square3.location.X + squareSize, square3.location.Y);
                        square4.location = new Point(square3.location.X, square3.location.Y - squareSize);
                    }
                    else if (state.Direction == 1)
                    {
                        square1.location = new Point(square3.location.X, square3.location.Y + 2 * squareSize);
                        square2.location = new Point(square3.location.X, square3.location.Y + squareSize);
                        square4.location = new Point(square3.location.X + squareSize, square3.location.Y);
                    }
                    else if (state.Direction == 2)
                    {
                        square1.location = new Point(square3.location.X - 2 * squareSize, square3.location.Y);
                        square2.location = new Point(square3.location.X - squareSize, square3.location.Y);
                        square4.location = new Point(square3.location.X, square3.location.Y + squareSize);
                    }
                    else if (state.Direction == 3)
                    {
                        square1.location = new Point(square3.location.X, square3.location.Y - 2 * squareSize);
                        square2.location = new Point(square3.location.X, square3.location.Y - squareSize);
                        square4.location = new Point(square3.location.X - squareSize, square3.location.Y);
                    }

                    if (!(EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize) &&
                 EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize) &&
                 EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize) &&
                 EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize)))
                    {
                        square1.location = oldPosition1;
                        square2.location = oldPosition2;
                        square3.location = oldPosition3;
                        square4.location = oldPosition4;
                        return false;
                    }
                    else { return true; }
                    DrawObject(Block.handler);
                }
            }
            public class L: Block
            {
                public L(Point TransLocation)
                    : base(TransLocation)
                {
                   type =3;
                square1.location = new Point(location.X, location.Y);
                square2.location = new Point(location.X, location.Y + squareSize);
                square3.location = new Point(location.X, location.Y + 2 * squareSize);
                square4.location = new Point(location.X + squareSize, location.Y + 2 * squareSize);
                }

                public override bool rotate(State state)
                {

                    Point oldPosition1 = square1.location;
                    Point oldPosition2 = square2.location;
                    Point oldPosition3 = square3.location;
                    Point oldPosition4 = square4.location;

                    DeleteObject(Block.handler);
                    if (state.Direction == 0)
                    {
                        square1.location = new Point(square3.location.X + 2 * squareSize, square3.location.Y);
                        square2.location = new Point(square3.location.X + squareSize, square3.location.Y);
                        square4.location = new Point(square3.location.X, square3.location.Y + squareSize);
                    }
                    else if (state.Direction == 1)
                    {
                        square1.location = new Point(square3.location.X, square3.location.Y + 2 * squareSize);
                        square2.location = new Point(square3.location.X, square3.location.Y + squareSize);
                        square4.location = new Point(square3.location.X - squareSize, square3.location.Y);
                    }
                    else if (state.Direction == 2)
                    {
                        square1.location = new Point(square3.location.X - 2 * squareSize, square3.location.Y);
                        square2.location = new Point(square3.location.X - squareSize, square3.location.Y);
                        square4.location = new Point(square3.location.X, square3.location.Y - squareSize);
                    }
                    else if (state.Direction == 3)
                    {
                        square1.location = new Point(square3.location.X, square3.location.Y - 2 * squareSize);
                        square2.location = new Point(square3.location.X, square3.location.Y - squareSize);
                        square4.location = new Point(square3.location.X + squareSize, square3.location.Y);
                    }
                    if (!(EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize) &&
                 EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize) &&
                 EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize) &&
                 EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize)))
                    {
                        square1.location = oldPosition1;
                        square2.location = oldPosition2;
                        square3.location = oldPosition3;
                        square4.location = oldPosition4;
                        return false;
                    }
                    else { return true; }
                    DrawObject(Block.handler);
                }
            }
            public class T: Block
            {
                public T(Point TransLocation)
                    : base(TransLocation)
                {
                   type =4;
                square1.location = new Point(location.X, location.Y);
                square2.location = new Point(location.X + squareSize, location.Y);
                square3.location = new Point(location.X + 2 * squareSize, location.Y);
                square4.location = new Point(location.X + squareSize, location.Y + squareSize);
                }
                public override bool rotate(State state)
                {

                    Point oldPosition1 = square1.location;
                    Point oldPosition2 = square2.location;
                    Point oldPosition3 = square3.location;
                    Point oldPosition4 = square4.location;

                    DeleteObject(Block.handler);
                    if (state.Direction == 0)
                    {
                        square1.location = new Point(square2.location.X, square2.location.Y - squareSize);
                        square3.location = new Point(square2.location.X, square2.location.Y + squareSize);
                        square4.location = new Point(square2.location.X - squareSize, square2.location.Y);
                    }
                    else if (state.Direction == 1)
                    {
                        square1.location = new Point(square2.location.X + squareSize, square2.location.Y);
                        square3.location = new Point(square2.location.X - squareSize, square2.location.Y);
                        square4.location = new Point(square2.location.X, square2.location.Y - squareSize);                           
                    }
                    else if (state.Direction == 2)
                    {
                        square1.location = new Point(square2.location.X, square2.location.Y + squareSize);
                        square3.location = new Point(square2.location.X, square2.location.Y - squareSize);
                        square4.location = new Point(square2.location.X + squareSize, square2.location.Y);
                    }
                    else if (state.Direction == 3)
                    {
                        square1.location = new Point(square2.location.X - squareSize, square2.location.Y);
                        square3.location = new Point(square2.location.X + squareSize, square2.location.Y);
                        square4.location = new Point(square2.location.X, square2.location.Y + squareSize);
                    }
                    if (!(EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize) &&
                 EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize) &&
                 EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize) &&
                 EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize)))
                    {
                        square1.location = oldPosition1;
                        square2.location = oldPosition2;
                        square3.location = oldPosition3;
                        square4.location = oldPosition4;
                        return false;
                    }
                    else { return true; }
                    DrawObject(Block.handler);
                }
            }
            public class Z: Block
            {
                public Z(Point TransLocation)
                    : base(TransLocation)
                {
                   type =5;
                square1.location = new Point(location.X, location.Y);
                square2.location = new Point(location.X + squareSize, location.Y);
                square3.location = new Point(location.X + squareSize, location.Y + squareSize);
                square4.location = new Point(location.X + 2 * squareSize, location.Y + squareSize);
                }
                public override bool rotate(State state)
                {

                    Point oldPosition1 = square1.location;
                    Point oldPosition2 = square2.location;
                    Point oldPosition3 = square3.location;
                    Point oldPosition4 = square4.location;

                    DeleteObject(Block.handler);
                    if (state.Direction == 0 || state.Direction == 2)
                    {
                        square1.location = new Point(square2.location.X, square2.location.Y - squareSize);
                        square3.location = new Point(square2.location.X - squareSize, square2.location.Y);
                        square4.location = new Point(square2.location.X - squareSize, square2.location.Y + squareSize);
                    }
                    else if (state.Direction == 1 || state.Direction == 3)
                    {
                        square1.location = new Point(square2.location.X - squareSize, square2.location.Y);
                        square3.location = new Point(square2.location.X, square2.location.Y + squareSize);
                        square4.location = new Point(square2.location.X + squareSize, square2.location.Y + squareSize);
                    }

                    if (!(EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize) &&
                 EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize) &&
                 EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize) &&
                 EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize)))
                    {
                        square1.location = oldPosition1;
                        square2.location = oldPosition2;
                        square3.location = oldPosition3;
                        square4.location = oldPosition4;
                        return false;
                    }
                    else { return true; }
                    DrawObject(Block.handler);
                }
            }
            public class S: Block
            {
                public S(Point TransLocation)
                    : base(TransLocation)
                {
                    type = 6;
                    square1.location = new Point(location.X, location.Y + squareSize);
                    square2.location = new Point(location.X + squareSize, location.Y + squareSize);
                    square3.location = new Point(location.X + squareSize, location.Y);
                    square4.location = new Point(location.X + 2 * squareSize, location.Y);
                }
                public override bool rotate(State state)
                {

                    Point oldPosition1 = square1.location;
                    Point oldPosition2 = square2.location;
                    Point oldPosition3 = square3.location;
                    Point oldPosition4 = square4.location;

                    DeleteObject(Block.handler);
                    if (state.Direction == 0 || state.Direction == 2)
                    {
                        square1.location = new Point(square3.location.X + squareSize, square3.location.Y + squareSize);
                        square2.location = new Point(square3.location.X + squareSize, square3.location.Y);
                        square4.location = new Point(square3.location.X, square3.location.Y - squareSize);
                    }
                    else if (state.Direction == 1 || state.Direction == 3)
                    {
                        square1.location = new Point(square3.location.X - squareSize, square3.location.Y + squareSize);
                        square2.location = new Point(square3.location.X, square3.location.Y + squareSize);
                        square4.location = new Point(square3.location.X + squareSize, square3.location.Y);
                    }

                    if (!(EmptySquare(square1.location.X / squareSize, square1.location.Y / squareSize) &&
                 EmptySquare(square2.location.X / squareSize, square2.location.Y / squareSize) &&
                 EmptySquare(square3.location.X / squareSize, square3.location.Y / squareSize) &&
                 EmptySquare(square4.location.X / squareSize, square4.location.Y / squareSize)))
                    {
                        square1.location = oldPosition1;
                        square2.location = oldPosition2;
                        square3.location = oldPosition3;
                        square4.location = oldPosition4;
                        return false;
                    }
                    else { return true; }
                    DrawObject(Block.handler);
                }
            }    
}
