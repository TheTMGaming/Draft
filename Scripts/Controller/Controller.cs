using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Top_Down_shooter.Scripts.Controller
{
    class Controller
    {
        public List<PlayerState> States => states.ToList();

        private List<PlayerState> states;

        private static readonly Dictionary<Keys, PlayerState> bindings = new Dictionary<Keys, PlayerState>()
        {
            [Keys.W] = PlayerState.Up,
            [Keys.S] = PlayerState.Down,
            [Keys.A] = PlayerState.Left,
            [Keys.D] = PlayerState.Right,
            [Keys.LButton] = PlayerState.Fire,
        };

        [DllImport("user32.dll")]
        private static extern short GetKeyState(Keys key);

        public void Update()
        {
            states = bindings
                .Where(pair => IsKeyPressed(pair.Key))
                .Select(pair => pair.Value)
                .ToList();
        }

        private bool IsKeyPressed(Keys key) => GetKeyState(key) > 1;
    }
}
