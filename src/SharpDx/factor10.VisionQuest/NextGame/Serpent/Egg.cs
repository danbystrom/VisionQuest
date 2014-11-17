using factor10.VisionThing;
using factor10.VisionThing.Effects;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;

namespace Serpent
{
    public class Egg
    {
        private readonly IVEffect _effect;
        private readonly IVDrawable _sphere;
        private readonly Texture2D _eggSkin;
        private readonly Matrix _world;

        public readonly Whereabouts Whereabouts;

        private float _timeToHatch;

        public Egg(
            IVEffect effect,
            IVDrawable sphere,
            Texture2D eggSkin,
            Matrix world,
            Whereabouts whereabouts,
            float timeToHatch)
        {
            _effect = effect;
            _sphere = sphere;
            _eggSkin = eggSkin;
            _world = world;
            Whereabouts = whereabouts;
            _timeToHatch = timeToHatch;
        }

        public Vector3 Position
        {
            get { return _world.TranslationVector; }    
        }

        public virtual void Update(GameTime gameTime)
        {
            _timeToHatch -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void Draw(
            IVEffect effect,
            Texture2D skin,
            IVDrawable sphere,
            Matrix translation,
            Direction direction)
        {
            var t = direction.IsNorthSouth
                ? Matrix.Scaling(0.6f, 0.6f, 0.8f)
                : Matrix.Scaling(0.8f, 0.6f, 0.6f);
            var off = direction.DirectionAsVector3()*-0.3f;
            t *= Matrix.Translation(off);

            effect.World = t*translation;
            effect.Texture = skin;
            effect.DiffuseColor = Vector4.One;
            sphere.Draw(effect);
        }

        public void Draw(GameTime gameTime)
        {
            Draw(_effect, _eggSkin, _sphere, _world, Whereabouts.Direction);
        }

        public bool TimeToHatch()
        {
            return _timeToHatch < 0;
        }

    }

}
