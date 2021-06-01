
using System;

namespace Top_Down_shooter.Scripts.Source
{
    static class GameSettings
    {
        public const int MapWidth = 1920;
        public const int MapHeight = 1920;
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 768;
        public const int TileSize = 64;

        public const int DelaySpawnNewMonster = 50000;
        public const int StartEnemiesCount = 3;
        public const int DistancePlayerToSpawner = 700;

        public static TimeSpan TimeToEnd = new TimeSpan(0, 5, 0);

        public const int SmallLoot = 5;
        public const int BigLoot = 10;
        public const float ProbabilityBigLoot = 0.4f;
        public const int CountSmallLoots = 10;

        public const int HPUp = 5;
        public const int CountHPPowerups = 5;

        // Player
        public const int StartCountBullets = 15;
        public const int PlayerSpeed = 10;
        public const int PlayerHealth = 100;
        public const int PlayerDamage = 10;
        public const int PlayerCooldown = 30;

        // Boss
        public const int BossHealth = 300;

        // Enemy.Tank
        public const int TankHealthMax = 7;
        public const int TankHealthMin = 3;

        public const int TankSpeedMin = 5;
        public const int TankSpeedMax = 12;
        public const float ProbabilitiSpeedMax = 0.4f;

        public const int TankResetPathMin = 5;
        public const int TankResetPathMax = 10;

        public const int TankDamage = 1;
        public const int TankCooldown = 300;
        public const int TankSizeCollider = 60;

        // Enemy.Fireman
        public const int FiremanHealth = 60;
        public const int FiremanDamage = 30;
        public const int FiremanCooldown = 500;
        public const int FiremanSizeCollider = 60;

        // Enemy.Waterman
        public const int WatermanHealth = 80;
        public const int WatermanDamage = 10;
        public const float WatermanCooldown = 200;
        public const int WatermanSizeCollider = 60;
    }
}
