﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Serpent
{
    class SpinningEnemy : SunflowersModel
    {
        Matrix rotation = Matrix.Identity;

        public SpinningEnemy(Model m)
            : base(m)
        {
        }

        public override void Update()
        {
            rotation *= Matrix.CreateRotationY(MathHelper.Pi / 180);
        }

        public override Matrix GetWorld()
        {
            return world * rotation;
        }
    }
}