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

        public PlayingField PlayingField { get; private set; }

        public PlayerSerpent PlayerSerpent { get; private set; }

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

        public int Scene { get; private set; }

        public Serpents(
            VisionContent vContent,
            Camera camera,
            IVDrawable sphere,
            int scene)
            : base(vContent.LoadPlainEffect("effects/simplebumpeffect"))
        {
            VContent = vContent;
            Sphere = sphere;

            _spriteBatch = new SpriteBatch(vContent.GraphicsDevice);
            _spriteFont = vContent.Content.Load<SpriteFont>("fonts/blackcastle");

            Camera = camera;

            Restart(scene);
        }

        public void Restart(int scene)
        {
            if(PlayingField!=null)
                PlayingField.Dispose();
            PlayingField = new PlayingField(
                VContent,
                VContent.Content.Load<Texture2D>(@"Textures\woodfloor"),
                scene);

            if(Scene!=scene)
                Data.Ground.GeneratePlayingField(PlayingField);
            Scene = scene;

            PlayerSerpent = new PlayerSerpent(
                VContent,
                PlayingField,
                Sphere);

            Enemies.Clear();
            EnemyEggs.Clear();
            Frogs.Clear();
            PlayerEgg = null;

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    VContent,
                    PlayingField,
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
            if (PlayerSerpent.SerpentStatus == SerpentStatus.Alive && Enemies.All(e => e.SerpentStatus != SerpentStatus.Alive) && !EnemyEggs.Any())
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
            if (Camera.KeyboardState.IsKeyPressed(Keys.Q))
                PlayerSerpent.Fertilize();

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
                var newSerpent= new  EnemySerpent(
                    VContent,
                    PlayingField,
                    Sphere,
                    0,
                    0);
                newSerpent.Restart(PlayingField, EnemyEggs[i].Whereabouts);
                EnemyEggs.RemoveAt(i);
                Enemies.Add(newSerpent);
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
            PlayingField.Draw(camera, drawingReason, shadowMap);
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
            var text1 = string.Format("Score: {0:000 000}", 0);
            var text2 = string.Format("Scene: {0}", Scene + 1);
            var text3 = string.Format("Lives left: {0}", 1);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_spriteFont, text1, new Vector2(10, 5), Color.LightYellow, 0, Vector2.Zero, 2.1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_spriteFont, text2, new Vector2((w - _spriteFont.MeasureString(text2).X * 2.1f) / 2, 5), Color.LightYellow, 0, Vector2.Zero, 2.1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_spriteFont, text3, new Vector2(w - _spriteFont.MeasureString(text3).X * 2.1f, 5) - 10, Color.LightYellow, 0, Vector2.Zero, 2.1f, SpriteEffects.None, 0);
            _spriteBatch.End();

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

        public override void Dispose()
        {
            if (PlayingField != null)
                PlayingField.Dispose();
            base.Dispose();
        }

    }

}
