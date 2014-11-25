﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            PlayingField playingField,
            ShadowMap shadowMap)
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
            shadowMap.ShadowCastingObjects.Add(PlayerSerpent);

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    vContent,
                    playingField,
                    playingField.EnemyWhereaboutsStart,
                    Sphere,
                    i);
                Enemies.Add(enemy);
            }
            shadowMap.ShadowCastingObjects.AddRange(Enemies);

            for (var i = 0; i < 3; i++)
                Frogs.Add(new Frog(
                    Data.VContent,
                    vContent.LoadPlainEffect(@"Effects\SimpleTextureEffect"),
                    new Whereabouts(),
                    Data.Ground));
            shadowMap.ShadowCastingObjects.AddRange(Frogs);

        }

        public Result Update(GameTime gameTime)
        {
            _onceASecond += gameTime.ElapsedGameTime.TotalSeconds;

            if (_onceASecond >= 1)
            {
                _onceASecond = 0;

                if (PlayerEgg == null && (Rnd.NextDouble() < 0.03 || Camera.KeyboardState.IsKeyPressed(Keys.D3) ))
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
                    PlayerSerpent.Restart(PlayingField.PlayerWhereaboutsStart, 1);
                }
                else if (enemy.SerpentStatus == SerpentStatus.Alive && PlayerSerpent.EatAt(enemy))
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
                    0));
                EnemyEggs.RemoveAt(i);
            }

            for (var i = Frogs.Count - 1; i >= 0; i--)
            {
                Frogs[i].Update(Camera, gameTime);

                if (PlayerSerpent.EatFrog(Frogs[i], true))
                    EnemyEggs.RemoveAt(i);
                else if (Enemies.Any(enemy => enemy.EatFrog(Frogs[i])))
                    EnemyEggs.RemoveAt(i);
            }

            return Result.GameOn;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            Data.PlayingField.Draw(camera, drawingReason, Data.ShadowMap);

            if (PlayerEgg != null)
                PlayerEgg.Draw(camera, drawingReason, Data.ShadowMap);
            foreach (var egg in EnemyEggs)
                egg.Draw(camera, drawingReason, Data.ShadowMap);
            foreach (var frog in Frogs)
                frog.Draw(camera, drawingReason, Data.ShadowMap);

            Data.Ground.Draw(camera, drawingReason, Data.ShadowMap);
            Data.Sky.Draw(camera);

            VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.AlphaBlend);
            PlayerSerpent.Draw(camera);
            foreach (var enemy in Enemies)
                enemy.Draw(camera);
            VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.Default);

            return true;
        }

    }

}