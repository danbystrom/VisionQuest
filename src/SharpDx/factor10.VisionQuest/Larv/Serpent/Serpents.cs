using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using factor10.VisionThing;
using Serpent;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Larv.Serpent
{
    public class Serpents : ClipDrawable
    {
        public enum Result
        {
            GameOn,
            PlayerDied,
            LevelComplete
        }

        public readonly VisionContent VContent;

        public readonly PlayingField PlayingField;

        public readonly PlayerSerpent PlayerSerpent;
        public Egg PlayerEgg;
        public readonly List<EnemySerpent> Enemies = new List<EnemySerpent>();
        public readonly List<Egg> EnemyEggs = new List<Egg>();
        public readonly List<Frog> Frogs = new List<Frog>();

        public readonly IVDrawable Sphere;

        public readonly Random Rnd = new Random();
        private double _onceASecond;

        public readonly Camera Camera;

        public Serpents(
            VisionContent vContent,
            Camera camera,
            IVDrawable sphere,
            PlayingField playingField)
            : base(vContent.LoadPlainEffect("effects/simplebumpeffect"))
        {
            VContent = vContent;
            Sphere = sphere;
            PlayingField = playingField;

            Camera = camera;

            PlayerSerpent = new PlayerSerpent(
                vContent,
                playingField,
                Sphere);

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    vContent,
                    playingField,
                    playingField.EnemyWhereaboutsStart,
                    Sphere,
                    i*1.5f,
                    2);
                Enemies.Add(enemy);
            }

            for (var i = 0; i < 3; i++)
                Frogs.Add(new Frog(
                    Data.VContent,
                    vContent.LoadPlainEffect(@"Effects\SimpleTextureEffect"),
                    this,
                    Data.Ground));
        }

        public Result Update(GameTime gameTime)
        {
            var result = Result.GameOn;

            if (Camera.KeyboardState.IsKeyPressed(Keys.Z))
                foreach (var enemy in Enemies)
                    enemy.SerpentStatus = SerpentStatus.Ghost;

            _onceASecond += gameTime.ElapsedGameTime.TotalSeconds;
            if (_onceASecond >= 1)
            {
                _onceASecond = 0;

                if (PlayerEgg == null && (Rnd.NextDouble() < 0.03 || Camera.KeyboardState.IsKeyPressed(Keys.D3)))
                    PlayerSerpent.Fertilize();

                if (Rnd.NextDouble() < 0.03 && !Enemies.Any(_ => _.IsPregnant) && Enemies.Any())
                    Enemies[Rnd.Next(Enemies.Count)].Fertilize();
            }

            PlayerSerpent.Update(Camera, gameTime);
            if (PlayerEgg == null)
                PlayerEgg = PlayerSerpent.TimeToLayEgg();

            foreach (var enemy in Enemies)
            {
                enemy.Update(Camera, gameTime);
                if (enemy.EatAt(PlayerSerpent))
                {
                    PlayerSerpent.SerpentStatus = SerpentStatus.Ghost;
                    result = Result.PlayerDied;
                }
                if (enemy.SerpentStatus == SerpentStatus.Alive && PlayerSerpent.EatAt(enemy))
                    enemy.SerpentStatus = SerpentStatus.Ghost;

                if (enemy.EatEgg(PlayerEgg))
                    PlayerEgg = null;

                var egg = enemy.TimeToLayEgg();
                if (egg != null)
                    EnemyEggs.Add(egg);
            }
            Enemies.RemoveAll(e => e.SerpentStatus == SerpentStatus.Finished);
            if (Enemies.All(e => e.SerpentStatus != SerpentStatus.Alive))
                return Result.LevelComplete;

            for (var i = EnemyEggs.Count - 1; i >= 0; i--)
            {
                EnemyEggs[i].Update(gameTime);

                if (PlayerSerpent.EatEgg(EnemyEggs[i]))
                {
                    EnemyEggs.RemoveAt(i);
                    continue;
                }

                if (!EnemyEggs[i].TimeToHatch())
                    continue;
                Enemies.Add(new EnemySerpent(
                    VContent,
                    PlayingField,
                    EnemyEggs[i].Whereabouts,
                    Sphere,
                    0,
                    0));
                EnemyEggs.RemoveAt(i);
            }

            foreach (var frog in Frogs)
            {
                frog.Update(Camera, gameTime);

                if (PlayerSerpent.EatFrog(frog, true))
                    frog.Restart();
                else if (Enemies.Any(enemy => enemy.EatFrog(frog)))
                    frog.Restart();
            }

            return PlayerSerpent.SerpentStatus == SerpentStatus.Alive ? Result.GameOn : Result.PlayerDied;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            Data.PlayingField.Draw(camera, drawingReason, shadowMap);

            Data.Ground.Draw(camera, drawingReason, shadowMap);
            Data.Sky.Draw(camera, drawingReason, shadowMap);

            if (PlayerEgg != null)
                PlayerEgg.Draw(camera, drawingReason, shadowMap);
            foreach (var egg in EnemyEggs)
                egg.Draw(camera, drawingReason, shadowMap);
            foreach (var frog in Frogs)
                frog.Draw(camera, drawingReason, shadowMap);

            var serpents = new List<BaseSerpent> {PlayerSerpent};
            serpents.AddRange(Enemies);

            foreach (var serpent in serpents.Where(_ => _.SerpentStatus == SerpentStatus.Alive))
                serpent.Draw(camera, drawingReason, shadowMap);

            if (drawingReason != DrawingReason.ShadowDepthMap && serpents.Any(_ => _.SerpentStatus == SerpentStatus.Ghost))
            {
                VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.AlphaBlend);
                foreach (var serpent in serpents.Where(_ => _.SerpentStatus == SerpentStatus.Ghost))
                    serpent.Draw(camera, drawingReason, shadowMap);
                VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.Default);
            }

            return true;
        }

    }

}
