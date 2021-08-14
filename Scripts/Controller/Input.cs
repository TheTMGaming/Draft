using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Top_Down_shooter.Scripts.Components
{
    class Input
    {
        public List<PlayerState> States => _states.ToList();

        private List<PlayerState> _states;

        private static readonly Dictionary<Keys, PlayerState> _bindings = new Dictionary<Keys, PlayerState>()
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
            _states = _bindings
                .Where(pair => IsKeyPressed(pair.Key))
                .Select(pair => pair.Value)
                .ToList();
        }

        private bool IsKeyPressed(Keys key) => GetKeyState(key) > 1;
    }
}
