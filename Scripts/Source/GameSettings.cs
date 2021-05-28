
namespace Top_Down_shooter.Scripts.Source
{
    static class GameSettings
    {
        public const int MapWidth = 1920;
        public const int MapHeight = 1920;

        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 768;

        public const int TileSize = 64;

        //Player
        public const int PlayerHealth = 100;
        public const int PlayerDamage = 10;
        public const int PlayerCooldown = 30;

        //Enemy.Tank
        public const int TankHealth = 7;
        public const int TankSpeed = 6;
        public const int TankDamage = 15;
        public const int TankCooldown = 300;
        public const int TankSizeCollider = 60;

        //Enemy.Fireman
        public const int FiremanHealth = 60;
        public const int FiremanDamage = 30;
        public const int FiremanCooldown = 500;
        public const int FiremanSizeCollider = 60;

        //Enemy.Waterman
        public const int WatermanHealth = 80;
        public const int WatermanDamage = 10;
        public const float WatermanCooldown = 200;
        public const int WatermanSizeCollider = 60;
    }
}
