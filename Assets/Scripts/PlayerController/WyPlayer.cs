using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mg.Wy
{
    public abstract class WyPlayer
    {
        private int currHp;
        private float speed;
        private float jumpHeight = 5;
        private float dodgeSpeed;
        private bool direction = true;
        private int currTrack;

        public int CurrHp { get => currHp; set => currHp = value; }
        public float Speed { get => speed; set => speed = value; }
        public float JumpHeight { get => jumpHeight; }
        public float DodgeSpeed { get => dodgeSpeed; set => dodgeSpeed = value; }
        public bool Direction { get => direction; set => direction = value; }
        public int CurrTrack { get => currTrack; set => currTrack = value; }

        public abstract void Skill(params WyPlayer[] players);
    }
}