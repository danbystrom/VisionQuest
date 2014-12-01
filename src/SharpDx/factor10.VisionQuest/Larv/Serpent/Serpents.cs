using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
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

        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;

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

            _spriteBatch = new SpriteBatch(vContent.GraphicsDevice);
            _spriteFont = vContent.Content.Load<SpriteFont>("fonts/blackcastle");

            Camera = camera;

            PlayerSerpent = new PlayerSerpent(
                vContent,
                playingField,
                Sphere);
            Restart();
        }

        public void Restart()
        {
            PlayerSerpent.Restart(PlayingField.PlayerWhereaboutsStart, 1);

            Enemies.Clear();
            EnemyEggs.Clear();
            Frogs.Clear();
            PlayerEgg = null;

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    VContent,
                    PlayingField,
                    PlayingField.EnemyWhereaboutsStart,
                    Sphere,
                    i * 1.5f,
                    2);
                Enemies.Add(enemy);
            }

            for (var i = 0; i < 3; i++)
                Frogs.Add(new Frog(
                    Data.VContent,
                    VContent.LoadPlainEffect(@"Effects\SimpleTextureEffect"),
                    this,
                    Data.Ground));
        }

        public Result GameStatus()
        {
            if (Enemies.All(e => e.SerpentStatus != SerpentStatus.Alive) && !EnemyEggs.Any())
                return Result.LevelComplete;
            return PlayerSerpent.SerpentStatus != SerpentStatus.Finished ? Result.GameOn : Result.PlayerDied;
        }

        private bool _paused;

        public override void Update(Camera camera, GameTime gameTime)
        {
            _paused ^= Camera.KeyboardState.IsKeyPressed(Keys.P);
            if (_paused)
                return;

            if (Camera.KeyboardState.IsKeyPressed(Keys.Z))
                foreach (var enemy in Enemies)
                    enemy.SerpentStatus = SerpentStatus.Ghost;
            if (Camera.KeyboardState.IsKeyPressed(Keys.X))
                PlayerSerpent.AddTail();

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
                    PlayerSerpent.SerpentStatus = SerpentStatus.Ghost;
                if (enemy.SerpentStatus == SerpentStatus.Alive && PlayerSerpent.EatAt(enemy))
                    enemy.SerpentStatus = SerpentStatus.Ghost;

                if (enemy.EatEgg(PlayerEgg))
                    PlayerEgg = null;

                var egg = enemy.TimeToLayEgg();
                if (egg != null)
                    EnemyEggs.Add(egg);
            }
            Enemies.RemoveAll(e => e.SerpentStatus == SerpentStatus.Finished);

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

                if (PlayerEgg != null && frog.DistanceSquared(PlayerEgg) < 0.4f)
                    PlayerEgg = null;
                EnemyEggs.RemoveAll(_ => frog.DistanceSquared(_) < 0.4f);

                if (PlayerSerpent.EatFrog(frog, true))
                    frog.Restart();
                else if (Enemies.Any(enemy => enemy.EatFrog(frog)))
                    frog.Restart();
            }
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

            var allSerpents = new List<BaseSerpent> { PlayerSerpent };
            allSerpents.AddRange(Enemies);

            foreach (var serpent in allSerpents.Where(_ => _.SerpentStatus == SerpentStatus.Alive))
                serpent.Draw(camera, drawingReason, shadowMap);

            if (drawingReason != DrawingReason.ShadowDepthMap && allSerpents.Any(_ => _.SerpentStatus == SerpentStatus.Ghost))
            {
                VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.AlphaBlend);
                foreach (var serpent in allSerpents.Where(_ => _.SerpentStatus == SerpentStatus.Ghost))
                    serpent.Draw(camera, drawingReason, shadowMap);
                VContent.GraphicsDevice.SetBlendState(VContent.GraphicsDevice.BlendStates.Default);
            }

            var w = VContent.GraphicsDevice.BackBuffer.Width;
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_spriteFont, "Score: 000 000 000", new Vector2(10, 5), Color.LightYellow, 0, Vector2.Zero, 2.1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_spriteFont, "Scene: 1", new Vector2((w - _spriteFont.MeasureString("Scene: 1").X * 2.1f)/2, 5), Color.LightYellow, 0, Vector2.Zero, 2.1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_spriteFont, "Lives left: 0", new Vector2(w - _spriteFont.MeasureString("Lives left: 0").X * 2.1f, 5) - 10, Color.LightYellow, 0, Vector2.Zero, 2.1f, SpriteEffects.None, 0);
            _spriteBatch.End();

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

    }

}
