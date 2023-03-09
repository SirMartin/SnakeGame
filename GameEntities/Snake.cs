using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Entities;
using SnakeGame.Enums;
using System;


namespace SnakeGame.GameEntities
{
    public class Snake
    {
        public int Length { get; set; }

        public MoveTypes Direction { get; set; }

        public Coordinates Position { get; set; }

        public ContentManager Content
        {
            get { return _content; }
        }
        ContentManager _content;

        public Snake(IServiceProvider serviceProvider) {
            RestartGame();
            _content = new ContentManager(serviceProvider, "Content");
        }

        internal void RestartGame()
        {
            Position = new Coordinates(GameConstants.SNAKE_START_POSITION_X, GameConstants.SNAKE_START_POSITION_Y);
            Direction = MoveTypes.None;
        }

        public void Update(KeyboardState keyboard)
        {
            UpdateDirection(keyboard);

            Move();
        }

        public bool IsAlive()
        {
            int MaxSize = 500;
            if (Position.X < 0 || Position.Y < 0 || Position.X > MaxSize || Position.Y > MaxSize)
                return false;

            return true;
        }

        private void UpdateDirection(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Up))
            {
                Direction = MoveTypes.Up;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                Direction = MoveTypes.Down;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                Direction = MoveTypes.Left;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                Direction = MoveTypes.Right;
            }
        }

        private void Move()
        {
            switch (Direction)
            {
                case MoveTypes.None:
                    break;
                case MoveTypes.Up:
                    Position.Y = Position.Y - GameConstants.SNAKE_SIZE;
                    break;
                case MoveTypes.Right:
                    Position.X = Position.X + GameConstants.SNAKE_SIZE;
                    break;
                case MoveTypes.Down:
                    Position.Y = Position.Y + GameConstants.SNAKE_SIZE;
                    break;
                case MoveTypes.Left:
                    Position.X = Position.X - GameConstants.SNAKE_SIZE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            if (!IsAlive())
                return;

            var rect = new Texture2D(graphicsDevice, GameConstants.SNAKE_SIZE, GameConstants.SNAKE_SIZE);
            var data = new Color[GameConstants.SNAKE_SIZE * GameConstants.SNAKE_SIZE];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            var center = 33;
            // Draw the snake eye.
            data[center] = Color.White;
            data[center + 1] = Color.White;
            data[center + 10] = Color.White;
            data[center + 11] = Color.White;
            rect.SetData(data);
            var coor = new Vector2(Position.X, Position.Y);
            spriteBatch.Draw(rect, coor, Color.White);

            var font = Content.Load<SpriteFont>("Fonts/Arial24");
            // Show Game Over.
            var text = $"{Position.X} {Position.Y}";
            spriteBatch.DrawString(font, text, new Vector2(0, 0), Color.Green);
            }

        
    }
}
