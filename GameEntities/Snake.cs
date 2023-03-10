using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Entities;
using SnakeGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace SnakeGame.GameEntities
{
    public class Snake
    {
        public MoveTypes Direction { get; set; }

        public Coordinates Position { get; set; }

        public List<BodyPart> BodyParts { get; set; }

        public ContentManager Content
        {
            get { return _content; }
        }
        ContentManager _content;

        public Snake(IServiceProvider serviceProvider)
        {
            RestartGame();
            _content = new ContentManager(serviceProvider, "Content");

            BodyParts = new List<BodyPart>();
        }

        public Rectangle GetCollisionRectangle()
        {
            return new Rectangle(Position.X, Position.Y, GameConstants.SNAKE_SIZE, GameConstants.SNAKE_SIZE);
        }

        internal void RestartGame()
        {
            Position = new Coordinates(GameConstants.SNAKE_START_POSITION_X, GameConstants.SNAKE_START_POSITION_Y);
            Direction = MoveTypes.Pause;
        }

        public void Update()
        {
            if (Direction != MoveTypes.Pause)
            {
                // Update body parts.
                for (int i = BodyParts.Count - 1; i >= 0; i--)
                {
                    Coordinates prevBodyPartCoordinates;
                    if (i == 0)
                    {
                        prevBodyPartCoordinates = Position;
                    }
                    else
                    {
                        prevBodyPartCoordinates = BodyParts[i - 1].Position;
                    }
                    BodyParts[i].Update(prevBodyPartCoordinates);
                }
            }

            Move();
        }

        public bool IsAlive()
        {
            // Boundaries Collision
            if (Position.X < 0 || Position.Y < 0 || Position.X > GameConstants.WINDOW_WIDTH || Position.Y > GameConstants.WINDOW_HEIGHT)
                return false;

            // Itself Collision.
            for (var i = 0; i < BodyParts.Count; i++)
            {
                if (Position.X == BodyParts[i].Position.X && Position.Y == BodyParts[i].Position.Y)
                    return false;
            }

            return true;
        }

        public bool IsMoving()
        {
            return Direction != MoveTypes.Pause;
        }

        internal void UpdateDirection(KeyboardState keyboard)
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
                case MoveTypes.Pause:
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

            DrawHead(graphicsDevice, spriteBatch);

            DrawBody(graphicsDevice, spriteBatch);
        }

        private void DrawHead(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
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
        }

        private void DrawBody(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            foreach (var item in BodyParts)
            {
                item.Draw(graphicsDevice, spriteBatch);
            }
        }

        internal void Grow()
        {
            var lastBodyPart = BodyParts.LastOrDefault();
            var newPositionX = lastBodyPart != null ? lastBodyPart.Position.X : Position.X;
            var newPositionY = lastBodyPart != null ? lastBodyPart.Position.Y : Position.Y;
            switch (Direction)
            {
                case MoveTypes.Up:
                    newPositionY += GameConstants.SNAKE_SIZE;
                    break;
                case MoveTypes.Right:
                    newPositionX -= GameConstants.SNAKE_SIZE;
                    break;
                case MoveTypes.Down:
                    newPositionY -= GameConstants.SNAKE_SIZE;
                    break;
                case MoveTypes.Left:
                    newPositionX += GameConstants.SNAKE_SIZE;
                    break;
            }
            BodyParts.Add(new BodyPart(newPositionX, newPositionY));
        }
    }
}
