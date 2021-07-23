using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Top_Down_shooter.Scripts.Controllers
{
    class Controller
    {
        [DllImport("user32.dll")]
        private static extern short GetKeyState(Keys key);
     
        private static readonly Dictionary<Keys, State> bindings = new Dictionary<Keys, State>()
        {
            [Keys.W] = State.Up,
            [Keys.S] = State.Down,
            [Keys.A] = State.Left,
            [Keys.D] = State.Right,
            [Keys.LButton] = State.Fire,
        };

        public List<State> Update()
        {
            return bindings
                .Where(pair => IsKeyPressed(pair.Key))
                .Select(pair => pair.Value)
                .ToList();
        }

        private bool IsKeyPressed(Keys key) => GetKeyState(key) > 1;
    }
}
