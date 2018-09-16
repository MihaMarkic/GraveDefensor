using GraveDefensor.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GraveDefensor.Shared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GraveDefensorGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 mousePosition;
        Texture2D mouseTexture;

        public GraveDefensorGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenInfo.Default.Width,
                PreferredBackBufferHeight = ScreenInfo.Default.Height,
                SupportedOrientations = DisplayOrientation.LandscapeLeft,
                IsFullScreen = ScreenInfo.Default.IsFullScreen
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap;
            if (ScreenInfo.Default.HasMouse)
            {
                mousePosition = new Vector2(
                    graphics.GraphicsDevice.Viewport.Width / 2,
                    graphics.GraphicsDevice.Viewport.Height / 2);
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (ScreenInfo.Default.HasMouse)
            {
                mouseTexture = Content.Load<Texture2D>("cross");
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (ScreenInfo.Default.HasMouse)
            {
                mouseTexture.Dispose();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (ScreenInfo.Default.HasMouse)
            {
                MouseState state = Mouse.GetState();

                if (graphics.GraphicsDevice.Viewport.Bounds.Contains(state.X, state.Y))
                {
                    // Update our sprites position to the current cursor location
                    mousePosition.X = state.X;
                    mousePosition.Y = state.Y;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (ScreenInfo.Default.HasMouse)
            {
                spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
