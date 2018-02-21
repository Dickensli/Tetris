using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Tetris_game
{
    public partial class Form1 : Form
    {
        private Block currentBlock = new Block(new Point()) { type = -1 };
        private Block nextBlock = new Block(new Point()) { type = -1 };
        private Point startLocation1 = new Point(Block.squareSize * 6, 0);
        private Point startLocation2 = new Point(Block.squareSize, Block.squareSize*2);         
        private bool NewBegin = true;
        private bool NewGame = false;
        bool stop = false;
        public static bool shiftenable = true;
        Point loca = new Point(Block.squareSize * 6, 0);
        Square Panel2s1 = new Square(new Point(0,0),new Size(0,0));
        Square Panel2s2 = new Square(new Point(0,0),new Size(0,0));
        Square Panel2s3 = new Square(new Point(0,0),new Size(0,0));
        Square Panel2s4 = new Square(new Point(0,0), new Size(0,0));
        int FinalScore = 0;
        int BestScore = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            panel1.BackColor = Colour.BackCo;
            panel2.BackColor = Colour.BackCo;
            Block.handler = panel1.Handle;
            Square.hand = panel2.Handle;
            timer1.Interval = 200;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
                button1.Visible = false;
                button1.Text = "";
                button1.BackColor = Colour.BackCo;
                if (NewBegin == true)
                {
                    shiftenable = true;
                    currentBlock = nextBlock;
                    Panel2s1.Delete(Square.hand);
                    Panel2s2.Delete(Square.hand);
                    Panel2s3.Delete(Square.hand);
                    Panel2s4.Delete(Square.hand);

                    BlockTypes type = new BlockTypes();
                    int num = type.RandomType();
                    //int num = 1;
                    if (num == 0)
                    {
                        nextBlock = new Sq(startLocation1);
                    }
                    else if (num == 1)
                    {
                        nextBlock = new Line(startLocation1);
                    }
                    else if (num == 2)
                    {
                        nextBlock = new J(startLocation1);
                    }
                    else if (num == 3)
                    {
                        nextBlock = new L(startLocation1);
                    }
                    else if (num == 4)
                    {
                        nextBlock = new T(startLocation1);
                    }
                    else if (num == 5)
                    {
                        nextBlock = new Z(startLocation1);
                    }
                    else
                    {
                        nextBlock = new S(startLocation1);
                    }

                    Panel2s1 = new Square(new Point(nextBlock.square1.location.X - startLocation1.X + startLocation2.X, nextBlock.square1.location.Y - startLocation1.Y + startLocation2.Y), new Size(Block.squareSize, Block.squareSize));
                    Panel2s2 = new Square(new Point(nextBlock.square2.location.X - startLocation1.X + startLocation2.X, nextBlock.square2.location.Y - startLocation1.Y + startLocation2.Y), new Size(Block.squareSize, Block.squareSize));
                    Panel2s3 = new Square(new Point(nextBlock.square3.location.X - startLocation1.X + startLocation2.X, nextBlock.square3.location.Y - startLocation1.Y + startLocation2.Y), new Size(Block.squareSize, Block.squareSize));
                    Panel2s4 = new Square(new Point(nextBlock.square4.location.X - startLocation1.X + startLocation2.X, nextBlock.square4.location.Y - startLocation1.Y + startLocation2.Y), new Size(Block.squareSize, Block.squareSize));
                    Panel2s1.Draw(Square.hand);
                    Panel2s2.Draw(Square.hand);
                    Panel2s3.Draw(Square.hand);
                    Panel2s4.Draw(Square.hand);

                    NewBegin = false;
                }
                else
                {
                    if (currentBlock.type == -1)
                    {
                        NewBegin = true;
                    }
                    else
                    {
                        if (!currentBlock.Down())
                        {
                            if (currentBlock.Top() == 0)
                            {
                                button1.Text = "Game Over!\n"+Convert.ToString(FinalScore);
                                button1.Visible = true;
                                NewGame = false;
                                timer1.Stop();
                            }
                            Block.DeletePanel();
                            FinalScore += Block.Score();
                            label2.Text = Convert.ToString(FinalScore);
                            Block.DrawPanel();
                            NewBegin = true;
                        }
                        else
                        {
                            loca = new Point(loca.X, loca.Y + 1);
                            currentBlock.location = loca;
                        }
                    }

                }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black);

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && stop == false)
            {
                currentBlock.Left();
            }
            if (e.KeyCode == Keys.Right && stop == false)
            {
                currentBlock.Right();
            }
            if (e.KeyCode == Keys.Up && stop == false)
            {
                currentBlock.Up();
            }
            if (e.KeyCode == Keys.Down && stop == false)
            {
                timer1.Interval = 100;
                currentBlock.Down();
                timer1.Interval = 200;
            }
            if (e.KeyCode == Keys.Space)
            {
                if (NewGame == true)
                {
                    if (stop)
                    {
                        button1.Visible = false;
                        button1.Text = "";
                        timer1.Start();
                        stop = false;
                    }
                    else
                    {
                        button1.Visible = true;
                        button1.Text = "Pause";
                        button1.BackColor = Color.SeaGreen;
                        timer1.Stop();
                        stop = true;
                    }
                }
            }
        }
                

        private void button1_Click(object sender, EventArgs e)
        {
            if (NewGame == false)
            {
                if (FinalScore >= BestScore)
                {
                    BestScore = FinalScore;
                }
                FinalScore = 0;
                timer1.Start();
                button1.Text = "";
                button1.BackColor = Colour.BackCo;
                button1.Visible = false;
                label5.Text = Convert.ToString(BestScore);
                Block.DeletePanel();
                for (int i = 0; i < Block.height; i++)
                {
                    for (int j = 0; j < Block.width; j++)
                    {
                        Block.SqPanel[i, j] = null;
                        Block.IntPanel[i, j] = 0;
                    }
                }
                NewGame = true;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black);
        }


    }
}

