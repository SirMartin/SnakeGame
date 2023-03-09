using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Entities;
using SnakeGame.Enums;
using System;

namespace SnakeGame.GameEntities
{
    public class BodyPart
    {
        public Coordinates Position { get; set; }

        public BodyPart(int x, int y)
        {
            Position = new Coordinates(x, y);
        }

        public void Update(Coordinates previousBodyPartCoordinates)
        {
            var direction = GetMovementDirection(previousBodyPartCoordinates);

            Move(direction);
        }

        private MoveTypes GetMovementDirection(Coordinates previousBodyPartCoordinates)
        {
            if (previousBodyPartCoordinates.X == Position.X)
            {
                if (previousBodyPartCoordinates.Y < Position.Y) {
                    return MoveTypes.Up;
                }else
                {
                    return MoveTypes.Down;
                }
            }
            else if (previousBodyPartCoordinates.Y == Position.Y)
            {
                if (previousBodyPartCoordinates.X < Position.X)
                {
                    return MoveTypes.Left;
                }
                else
                {
                    return MoveTypes.Right;
                }
            }

            return MoveTypes.Pause;
        }

        private void Move(MoveTypes direction)
        {
            switch (direction)
            {
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
            var rect = new Texture2D(graphicsDevice, GameConstants.SNAKE_SIZE, GameConstants.SNAKE_SIZE);
            var data = new Color[GameConstants.SNAKE_SIZE * GameConstants.SNAKE_SIZE];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            rect.SetData(data);
            var coor = new Vector2(Position.X, Position.Y);
            spriteBatch.Draw(rect, coor, Color.White);
        }
    }
}