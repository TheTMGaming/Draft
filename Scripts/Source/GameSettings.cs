
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

        public const int DelaySpawnNewMonster = 1850;
        public const int StartEnemiesCount = 5;
        public const int DistanceBossToSpawnPowerup = 80;

        public static TimeSpan TimeToEnd = new TimeSpan(0, 5, 0);

        public const int SmallLootBoost = 15;
        public const int BigLootBoost = 30;
        public const float ProbabilitySpawnBigLoot = 0.2f;
        public const int SmallLootsCount = 10;

        public const int HPUp = 150;
        public const int CountHPPowerups = 10;

        public const int BoxHealth = 40;

        // Player
        public const int StartCountBullets = 30;
        public const int PlayerSpeed = 10;
        public const int PlayerHealth = 500;
        public const int PlayerDamage = 10;
        public const int PlayerCooldown = 30;
        public const int PlayerBulletSpeed = 20;

        // Boss
        public const int BossHealth = 1400;
        public const int BossCooldown = 16000;

        // Boss.Fire
        public const int FireDamage = 130;
        public const int FireCooldown = 1500;
        public const int FireMinSpeed = 20;
        public const int FireMaxSpeed = 30;
        public const int FireSpawnEnemy = 12000;

        // Enemy.Tank
        public const int TankHealthMax = 70;
        public const int TankHealthMin = 40;

        public const int TankSpeedMin = 7;
        public const int TankSpeedMax = 13;
        public const float ProbabilitiSpeedMax = 0.5f;

        public const int TankResetPathMin = 5;
        public const int TankResetPathMax = 10;

        public const int TankDamage = 15;
        public const int TankCooldown = 300;
        public const int TankSizeCollider = 60;

        // Enemy.Fireman
        public const int FiremanHealth = 30;
        public const int FiremanSpeed = 9;
        public const int FiremanDamage = 90;

        public const int FiremanCooldown = 5000;

        public const int FiremanSizeCollider = 60;

        public const int FiremanDistanceFire = 800;
        public const int FiremanDistanceRotation = 300;
        public const int FiremanSpeedBullet = 17;

        //// Enemy.Waterman
        //public const int WatermanHealth = 80;
        //public const int WatermanDamage = 10;
        //public const float WatermanCooldown = 200;
        //public const int WatermanSizeCollider = 60;
    }
}
