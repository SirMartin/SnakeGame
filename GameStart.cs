using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.GameEntities;

namespace SnakeGame
{
    public class GameStart : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Snake _theSnake;

        public GameStart()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set initial resolution.
            _graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _theSnake = new Snake(Services);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        int elapsedTime = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_theSnake.IsAlive())
            {
                // Check for restart option.
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    _theSnake.RestartGame();
                }
                return;
            }

            // Check if we need to make update or not.
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime < 200)
                return;

            elapsedTime = 0;

            _theSnake.Update(Keyboard.GetState());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start the sprite batch.
            _spriteBatch.Begin();

            var font = Content.Load<SpriteFont>("Fonts/Arial24");
            if (!_theSnake.IsAlive())
            {
                // Show Game Over.
                var text = $"GAME OVER";
                _spriteBatch.DrawString(font, text, new Vector2(250, 250), Color.Pink);
            }

            _theSnake.Draw(_graphics.GraphicsDevice, _spriteBatch);

            // End the sprite batch.
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}