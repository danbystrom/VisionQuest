﻿using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using Serpent;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Larv.Serpent
{
    public class Serpents
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

        public readonly IVDrawable Sphere;

        public readonly Random Rnd = new Random();
        private double _onceASecond;

        public readonly SerpentCamera SerpentCamera;

        public Serpents(
            VisionContent vContent,
            Camera camera,
            IVDrawable sphere,
            PlayingField playingField,
            ShadowMap shadowMap)
        {
            VContent = vContent;
            Sphere = sphere;
            PlayingField = playingField;

            SerpentCamera = new SerpentCamera(
                camera,
                CameraBehavior.FollowTarget);

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
        }

        public Result Update(GameTime gameTime)
        {
            _onceASecond += gameTime.ElapsedGameTime.TotalSeconds;

            if (_onceASecond >= 1)
            {
                _onceASecond = 0;

                if (PlayerEgg == null && (Rnd.NextDouble() < 0.03 || SerpentCamera.Camera.KeyboardState.IsKeyPressed(Keys.D3) ))
                    PlayerSerpent.Fertilize();

                if (Rnd.NextDouble() < 0.03 && !Enemies.Any(_ => _.IsPregnant) && Enemies.Any())
                    Enemies[Rnd.Next(Enemies.Count)].Fertilize();
            }

            PlayerSerpent.Update(SerpentCamera, gameTime);
            if (PlayerEgg == null)
                PlayerEgg = PlayerSerpent.TimeToLayEgg();

            foreach (var enemy in Enemies)
            {
                enemy.Update(SerpentCamera.Camera, gameTime);
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
                if (PlayerSerpent.EatEgg(EnemyEggs[i]))
                {
                    EnemyEggs.RemoveAt(i);
                    continue;
                }

                EnemyEggs[i].Update(gameTime);
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

            return Result.GameOn;
        }

        public void Draw(GameTime gameTime)
        {
            Data.PlayingField.Draw(SerpentCamera.Camera, DrawingReason.Normal, Data.ShadowMap);

            if (PlayerEgg != null)
                PlayerEgg.Draw(SerpentCamera.Camera, DrawingReason.Normal, Data.ShadowMap);
            foreach (var egg in EnemyEggs)
                egg.Draw(SerpentCamera.Camera, DrawingReason.Normal, Data.ShadowMap);

            Data.Ground.Draw(SerpentCamera.Camera, DrawingReason.Normal, Data.ShadowMap);
            Data.Sky.Draw(SerpentCamera.Camera);

            VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.AlphaBlend);
            PlayerSerpent.Draw(SerpentCamera.Camera);
            foreach (var enemy in Enemies)
                enemy.Draw(SerpentCamera.Camera);
            VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.Default);

        }

    }

}
