using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mg.Wy
{
    public class FaHai : WyPlayer
    {
        public FaHai()
        {
            CurrHp = int.MaxValue;
            Speed = WyConstants.FaHaiBenginSpeed;
            DodgeSpeed = WyConstants.FaHaiBenginDogeSpeed;
            CurrTrack = 2;
        }

        public override void Skill(params WyPlayer[] players)
        {
            
        }
    }

    public class BaiShe : WyPlayer
    {
        public BaiShe()
        {
            CurrHp = int.MaxValue;
            Speed = WyConstants.PlayerBenginSpeed;
            DodgeSpeed = WyConstants.PlayerDogeSpeed;
            CurrTrack = 1;
        }

        public override void Skill(params WyPlayer[] players)
        {

        }
    }

    public class XvXian : WyPlayer
    {
        public XvXian()
        {
            CurrHp = int.MaxValue;
            Speed = WyConstants.PlayerBenginSpeed;
            DodgeSpeed = WyConstants.PlayerDogeSpeed;
            CurrTrack = 2;
        }

        public override void Skill(params WyPlayer[] players)
        {

        }
    }

    public class XiaoQin : WyPlayer
    {
        public XiaoQin()
        {
            CurrHp = int.MaxValue;
            Speed = WyConstants.PlayerBenginSpeed;
            DodgeSpeed = WyConstants.PlayerDogeSpeed;
            CurrTrack = 3;
        }

        public override void Skill(params WyPlayer[] players)
        {

        }
    }
}
