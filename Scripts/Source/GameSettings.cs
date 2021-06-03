
using System;

namespace Top_Down_shooter.Scripts.Source
{
    static class GameSettings
    {
        public const int MapWidth = 1920;
        public const int MapHeight = 1920;
        public const int ScreenWidth = 1100;
        public const int ScreenHeight = 768;
        public const int TileSize = 64;

        public const int DelaySpawnNewMonster = 50000;
        public const int StartEnemiesCount = 3;
        public const int DistanceBossToSpawnerPowerup = 200;
        public const int DistancePlayerToSpawnerMonster = 700;

        public static TimeSpan TimeToEnd = new TimeSpan(0, 5, 0);

        public const int SmallLoot = 5;
        public const int BigLoot = 10;
        public const float ProbabilityBigLoot = 0.4f;
        public const int CountSmallLoots = 10;

        public const int HPUp = 150;
        public const int CountHPPowerups = 5;

        public const int BoxHealth = 40;

        // Player
        public const int StartCountBullets = 15;
        public const int PlayerSpeed = 10;
        public const int PlayerHealth = 500;
        public const int PlayerDamage = 10;
        public const int PlayerCooldown = 30;
        public const int PlayerBulletSpeed = 20;

        // Boss
        public const int BossHealth = 1300;
        public const int BosCooldown = 8000;

        // Boss.Fire
        public const int FireDamage = 50;
        public const int FireCooldown = 1000;
        public const int FireMinSpeed = 20;
        public const int FireMaxSpeed = 30;
        public const int FireSpawnEnemy = 5000;

        // Enemy.Tank
        public const int TankHealthMax = 80;
        public const int TankHealthMin = 30;

        public const int TankSpeedMin = 5;
        public const int TankSpeedMax = 12;
        public const float ProbabilitiSpeedMax = 0.4f;

        public const int TankResetPathMin = 5;
        public const int TankResetPathMax = 10;

        public const int TankDamage = 5;
        public const int TankCooldown = 300;
        public const int TankSizeCollider = 60;

        // Enemy.Fireman
        public const int FiremanHealth = 60;
        public const int FiremanSpeed = 10;
        public const int FiremanDamage = 30;

        public const int FiremanCooldown = 500;

        public const int FiremanSizeCollider = 60;

        public const int FiremanDistanceFire = 100;
        public const int FiremanSpeedBullet = 50;

        // Enemy.Waterman
        public const int WatermanHealth = 80;
        public const int WatermanDamage = 10;
        public const float WatermanCooldown = 200;
        public const int WatermanSizeCollider = 60;
    }
}
