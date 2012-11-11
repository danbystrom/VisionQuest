using LibNoise.Xna;
using LibNoise.Xna.Generator;
using LibNoise.Xna.Operator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

        Terrain terrain;

        Random r = new Random();
        BillboardSystem trees;
        BillboardSystem grass;
        BillboardSystem clouds;

        SkySphere sky;
        Water water;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //beginLibNoise
            // Create the module network
            var perlin = new Perlin();
            perlin.Seed = DateTime.Now.Millisecond;
            var rigged = new RiggedMultifractal();
            var add = new Add(perlin, rigged);

            // Initialize the noise map
            var noiseMap = new Noise2D(512, 512, add);
            noiseMap.GeneratePlanar(-1, 1, -1, 1);

            // Generate the textures
            var lnTextureGray = noiseMap.GetTexture(GraphicsDevice, Gradient.Grayscale);
            var lnTextureTerrain = noiseMap.GetTexture(GraphicsDevice, Gradient.Terrain);
            //this.m_textures[2] = m_noiseMap.GetNormalMap(GraphicsDevice, 3.0f);
            //endLibNoise

            camera = new FreeCamera(new Vector3(8000, 6000, 8000),
                MathHelper.ToRadians(45),
                MathHelper.ToRadians(-30),
                GraphicsDevice);

            terrain = new Terrain(lnTextureGray, 30, 4800,
                Content.Load<Texture2D>("grass"), 6, new Vector3(1, -1, 0),
                GraphicsDevice, Content);

            terrain.WeightMap = lnTextureTerrain;
            terrain.RTexture = Content.Load<Texture2D>("sand");
            terrain.GTexture = Content.Load<Texture2D>("rock");
            terrain.BTexture = Content.Load<Texture2D>("snow");
            terrain.DetailTexture = Content.Load<Texture2D>("noise_texture");

            // Positions where trees should be drawn
            List<Vector3> treePositions = new List<Vector3>();

            // Continue until we get 500 trees on the terrain
            for (int i = 0; i < 500; i++) // 500
            {
                // Get X and Z coordinates from the random generator, between
                // [-(terrain width) / 2 * (cell size), (terrain width) / 2 * (cell size)]
                float x = r.Next(-256 * 30, 256 * 30);
                float z = r.Next(-256 * 30, 256 * 30);

                // Get the height and steepness of this position on the terrain,
                // taking the height of the billboard into account
                float steepness;
                float y = terrain.GetHeightAtPosition(x, z, out steepness) + 100;

                // Reject this position if it is too low, high, or steep. Otherwise
                // add it to the list
                if (steepness < MathHelper.ToRadians(15) && y > 2300 && y < 3200)
                    treePositions.Add(new Vector3(x, y, z));
                else
                    i--;
            }

            trees = new BillboardSystem(GraphicsDevice, Content,
                Content.Load<Texture2D>("tree_billboard"), new Vector2(200), 
                treePositions.ToArray());

            trees.Mode = BillboardSystem.BillboardMode.Cylindrical;
            trees.EnsureOcclusion = true;


            // List of positions to place grass billboards
            List<Vector3> grassPositions = new List<Vector3>();

            // Retrieve pixel grid from grass map
            Texture2D grassMap = Content.Load<Texture2D>("grass_map");
            Color[] grassPixels = new Color[grassMap.Width * grassMap.Height];
            grassMap.GetData<Color>(grassPixels);

            // Loop until 1000 billboards have been placed
            for (int i = 0; i < 300; i++)
            {
                // Get X and Z coordinates from the random generator, between
                // [-(terrain width) / 2 * (cell size), (terrain width) / 2 * (cell size)]
                float x = r.Next(-256 * 30, 256 * 30);
                float z = r.Next(-256 * 30, 256 * 30);

                // Get corresponding coordinates in grass map
                int xCoord = (int)(x / 30) + 256;
                int zCoord = (int)(z / 30) + 256;

                // Get value between 0 and 1 from grass map
                float texVal = grassPixels[zCoord * 512 + xCoord].R / 255f;

                // Retrieve height
                float steepness;
                float y = terrain.GetHeightAtPosition(x, z, out steepness) + 50;

                // Randomly place a billboard here based on pixel color in grass
                // map
                if ((int)((float)r.NextDouble() * texVal * 10) == 1)
                    grassPositions.Add(new Vector3(x, y, z));
                else
                    i--;
            }

            // Create grass billboard system
            grass = new BillboardSystem(GraphicsDevice, Content,
                Content.Load<Texture2D>("grass_billboard"), new Vector2(100),
                grassPositions.ToArray());

            grass.Mode = BillboardSystem.BillboardMode.Cylindrical;
            grass.EnsureOcclusion = false;


            List<Vector3> cloudPositions = new List<Vector3>();

            // Create 20 "clusters" of clouds
            for (int i = 0; i < 20; i++)
            {
                Vector3 cloudLoc = new Vector3(
                    r.Next(-8000, 8000),
                    r.Next(4000, 6000),
                    r.Next(-8000, 8000));

                // Add 10 cloud billboards around each cluster point
                for (int j = 0; j < 10; j++)
                {
                    cloudPositions.Add(cloudLoc +
                        new Vector3(
                            r.Next(-3000, 3000),
                            r.Next(-300, 900),
                            r.Next(-1500, 1500)));
                }
            }

            clouds = new BillboardSystem(GraphicsDevice, Content,
                Content.Load<Texture2D>("cloud2"), new Vector2(2000),
                cloudPositions.ToArray());

            clouds.Mode = BillboardSystem.BillboardMode.Spherical;
            clouds.EnsureOcclusion = false;

            sky = new SkySphere(Content, GraphicsDevice,
                Content.Load<TextureCube>("clouds"));
            sky.SetClipPlane(null);

            water = new Water(Content, GraphicsDevice,
                new Vector3(0, 1600, 0), new Vector2(256 * 30));

            water.Objects.Add(sky);
            water.Objects.Add(terrain);

            lastMouseState = Mouse.GetState();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            updateCamera(gameTime);

            base.Update(gameTime);
        }

        void updateCamera(GameTime gameTime)
        {
            // Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            // Rotate the camera
            ((FreeCamera)camera).Rotate(deltaX * .005f, deltaY * .005f);

            Vector3 translation = Vector3.Zero;

            // Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;

            // Move 4 units per millisecond, independent of frame rate
            translation *= 10 * 
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Move the camera
            ((FreeCamera)camera).Move(translation);

            // Update the camera
            camera.Update();

            // Update the mouse state
            lastMouseState = mouseState;
        }

        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            water.PreDraw(camera, gameTime);
            
            GraphicsDevice.Clear(Color.Black);

            sky.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            terrain.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            water.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            trees.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Up, 
                ((FreeCamera)camera).Right);

            grass.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Up,
                ((FreeCamera)camera).Right);

            clouds.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Up,
                ((FreeCamera)camera).Right);

            base.Draw(gameTime);
        }
    }
}
