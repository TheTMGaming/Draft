﻿using System;
using System.Drawing;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Bullet : GameObject
    {
        public GameObject Parent { get; }
        public int Damage{ get; set; }
        public int Speed { get; set; }
        public float Angle { get; set; }

        public Bullet(GameObject parent, int x, int y, int speed, float angle, int damage)
        {
            Parent = parent;
            Collider = new Collider(this, 0, 0, 10, 10, isIgnoreNavMesh: true);
            Damage = damage;
            Speed = speed;
            X = x;
            Y = y;
            Angle = angle;
        }

        public void Move()
        {
            X += (int)(Speed * Math.Cos(Angle));
            Y += (int)(Speed * Math.Sin(Angle));
        }
    }
}
