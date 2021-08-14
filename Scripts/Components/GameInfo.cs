using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter.Scripts.Components
{
    class GameInfo
    {
        public float TimeDelta { get; private set; }

        public readonly Input Controller;


        public void UpdateTimeDelta(int dtInMilleseconds)
        {
            TimeDelta = dtInMilleseconds / 1000;
        }
    }
}
