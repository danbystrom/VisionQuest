using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using Serpent;
using Serpent.Serpent;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace NextGame.Serpent
{
    public class Serpents
    {
        public readonly VisionContent VContent;

        public readonly PlayingField PlayingField;

        public readonly PlayerSerpent PlayerSerpent;
        public Egg PlayerEgg;
        public readonly List<EnemySerpent> Enemies = new List<EnemySerpent>();
        public readonly List<Egg> EnemyEggs = new List<Egg>();

        public readonly IVDrawable Sphere;

        public readonly Random Rnd = new Random();
        private double _onceASecond;

        public Serpents(
            VisionContent vContent,
            IVDrawable sphere,
            MouseManager mouseManager,
            KeyboardManager keyboardManager,
            PointerManager pointerManager,
            PlayingField playingField)
        {
            VContent = vContent;
            Sphere = sphere;
            PlayingField = playingField;

            PlayerSerpent = new PlayerSerpent(
                vContent,
                mouseManager,
                keyboardManager,
                pointerManager,
                playingField,
                Sphere);

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    vContent,
                    playingField,
                    playingField.EnemyWhereaboutsStart,
                    Sphere,
                    PlayerSerpent.Camera,
                    i);
                Enemies.Add(enemy);
            }
        }

        public void Update(GameTime gameTime)
        {
            _onceASecond += gameTime.ElapsedGameTime.TotalSeconds;

            if (_onceASecond >= 1)
            {
                _onceASecond = 0;

                if (PlayerEgg == null && Rnd.NextDouble() < 0.03)
                    PlayerSerpent.Fertilize();

                if (Rnd.NextDouble() < 0.03 && !Enemies.Any(_ => _.IsPregnant) && Enemies.Any())
                    Enemies[Rnd.Next(Enemies.Count)].Fertilize();
            }

            PlayerSerpent.Update(gameTime);
            if (PlayerEgg == null)
                PlayerEgg = PlayerSerpent.TimeToLayEgg();

            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
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
            {
                if (PlayerSerpent.SerpentStatus == SerpentStatus.IsHome)
                {
                    if (PlayerEgg != null)
                    {
                        PlayerSerpent.Restart(PlayerEgg.Whereabouts, 0);
                        PlayerEgg = null;
                    }
                    else
                    {
                        
                    }
                }
                if (PlayerSerpent.PathFinder == null)
                {
                    PlayerSerpent.PathFinder = new PathFinder(PlayingField, PlayingField.PlayerWhereaboutsStart);
                    PlayerSerpent.Camera.CameraBehavior = CameraBehavior.Static;
                }
            }

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
                    PlayerSerpent.Camera,
                    0));
                EnemyEggs.RemoveAt(i);
            }

        }

        public void Draw(GameTime gameTime)
        {
            Data.PlayingField.Draw(PlayerSerpent.Camera.Camera);

            if (PlayerEgg != null)
                PlayerEgg.Draw(gameTime);
            foreach (var egg in EnemyEggs)
                egg.Draw(gameTime);

            Data.Ground.Draw(PlayerSerpent.Camera.Camera);
            Data.Sky.Draw(PlayerSerpent.Camera.Camera);

            VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.AlphaBlend);
            PlayerSerpent.Draw(gameTime);
            foreach (var enemy in Enemies)
                enemy.Draw(gameTime);
            VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.Default);

        }

    }

}
