using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Entities;
using SnakeGame.Enums;

namespace SnakeGame.GameEntities
{
    public class Fruit
    {
        public FruitTypes Type { get; set; }

        public int Lifespan { get; set; }

        public int Points { get; set; }

        public Coordinates Position { get; set; }

        public bool IsDiggested { get; private set; }

        public bool IsRotten => Lifespan <= 0;

        public Fruit()
        {
            Lifespan = RandomNumberGenerator.Next(5000, 10000) * 100;
            Points = RandomNumberGenerator.Next(5, 25);
            Type = (FruitTypes)RandomNumberGenerator.Next(0, 4);
            Position = new Coordinates(RandomNumberGenerator.Next(0, 50) * 10, RandomNumberGenerator.Next(0, 50) * 10);
        }

        public void Update(GameTime gameTime)
        {
            Lifespan -= gameTime.TotalGameTime.Milliseconds;
        }

        public void Eat()
        {
            IsDiggested = true;
        }

        public Color GetFruitColor()
        {
            switch (Type)
            {
                case FruitTypes.Banana:
                    return Color.Yellow;
                case FruitTypes.Apple:
                    return Color.Red;
                case FruitTypes.Pear:
                    return Color.Green;
                case FruitTypes.Watermelon:
                    return Color.DarkGreen;
                case FruitTypes.Grape:
                    return Color.Purple;
            }

            throw new System.Exception("Something happened!");
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            var rect = new Texture2D(graphicsDevice, GameConstants.SNAKE_SIZE, GameConstants.SNAKE_SIZE);
            var data = new Color[GameConstants.SNAKE_SIZE * GameConstants.SNAKE_SIZE];
            for (int i = 0; i < data.Length; ++i) data[i] = GetFruitColor();
            rect.SetData(data);
            var coor = new Vector2(Position.X, Position.Y);
            spriteBatch.Draw(rect, coor, Color.White);
        }
    }
}
