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

        private Fruit _theFruit;

        private int _totalPoints;

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
            RandomNumberGenerator.Initialize();

            _theSnake = new Snake(Services);

            _theFruit = new Fruit();

            _totalPoints = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        int movementElapsedTime = 0;
        int fruitElapsedTime = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_theSnake.IsAlive())
            {
                // Check for restart option.
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    RestartGame();
                }
                return;
            }

            if (_theSnake.IsMoving())
            {
                GenerateFruits(gameTime);
                _theFruit?.Update(gameTime);
            }

            UpdateSnake(gameTime);

            CheckSnakeEatFruit();

            base.Update(gameTime);
        }

        private void RestartGame()
        {
            _totalPoints = 0;
            movementElapsedTime = 0;
            fruitElapsedTime = 0;
            _theSnake.RestartGame();
        }

        private void CheckSnakeEatFruit()
        {
            if (_theFruit != null && _theSnake.Position.X == _theFruit.Position.X && _theSnake.Position.Y == _theFruit.Position.Y)
            {
                _totalPoints += _theFruit.Points;
                _theFruit.Eat();
                _theSnake.Grow();
            }
        }

        private void GenerateFruits(GameTime gameTime)
        {
            if (_theFruit != null)
            {
                if (_theFruit.IsRotten || _theFruit.IsDiggested)
                {
                    _theFruit = null;
                }
            }

            fruitElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (fruitElapsedTime < GameConstants.GENERATE_FRUIT_TIME)
                return;

            fruitElapsedTime = 0;

            if (_theFruit == null)
                _theFruit = new Fruit();            
        }

        private void UpdateSnake(GameTime gameTime)
        {
            _theSnake.UpdateDirection(Keyboard.GetState());

            // Check if we need to make update or not.
            movementElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (movementElapsedTime < GameConstants.MOVEMENT_TIME)
                return;

            movementElapsedTime = 0;

            _theSnake.Update();
        }

        private void DrawPoints()
        {
            var font = Content.Load<SpriteFont>("Fonts/Arial24");
            // Show Snake Coordinates
            var text = _totalPoints.ToString();
            _spriteBatch.DrawString(font, text, new Vector2(0, 0), Color.Green);
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

            _theFruit?.Draw(_graphics.GraphicsDevice, _spriteBatch);

            DrawPoints();

            // End the sprite batch.
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}