using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_game
{
    public class State
    {
        public int Direction;
        public virtual State Rotate(Block b){
            State state = new State(); 
           return state;
        }
    }

    public class North : State
    {
        public North() : base() { Direction = 0; }
        public override State Rotate(Block b)
        {
            State state = new North();
            if (b.rotate(this))
            {
                state = new East();
            }
            return state;
        }
    }

    public class East : State
    {
        public East() : base() { Direction = 1; }
        public override State Rotate(Block b)
        {
            State state = new East();
            if (b.rotate(this))
            {
                state = new South();
            }
            return state;
        }
    }

    public class South : State
    {
        public South() : base() { Direction = 2; }
        public override State Rotate(Block b)
        {
            State state = new South();
            if (b.rotate(this))
            {
                state = new West();
            }
            return state;
        }
    }

    public class West : State
    {
        public West() : base() { Direction = 3; }
        public override State Rotate(Block b)
        {
            State state = new West();
            if (b.rotate(this))
            {
                state = new North();
            }
            return state;
        }
    }

}

