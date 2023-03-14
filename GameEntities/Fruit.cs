using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Entities;
using SnakeGame.Enums;
using System;

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

        public ContentManager Content
        {
            get { return _content; }
        }
        ContentManager _content;

        private Texture2D sprite;
        private Rectangle drawRectangle;
        private Rectangle sourceRectangle;

        public Fruit(IServiceProvider serviceProvider)
        {
            Lifespan = RandomNumberGenerator.Next(5000, 10000) * 100;
            Points = RandomNumberGenerator.Next(5, 25);
            Type = (FruitTypes)RandomNumberGenerator.Next(0, Enum.GetNames(typeof(FruitTypes)).Length - 1);
            Position = new Coordinates(RandomNumberGenerator.Next(0, 50) * 10, RandomNumberGenerator.Next(0, 50) * 10);
            // Set the content manager.
            _content = new ContentManager(serviceProvider, "Content");

            Initialize();
        }

        private void Initialize()
        {
            SetFruitSprite();

            SetDrawRectangle();

            SetSourceRectangle();
        }

        private void SetDrawRectangle()
        {
            var spritedScaledWith = sprite.Width / GameConstants.SNAKE_SIZE;
            var spriteScaledHeight = sprite.Height / GameConstants.SNAKE_SIZE;
            var x = Position.X;
            var y = Position.Y;
            drawRectangle = new Rectangle((int)(x - spritedScaledWith / 2), (int)(y - spriteScaledHeight / 2), (int)spritedScaledWith, (int)spriteScaledHeight);
        }

        private void SetSourceRectangle()
        {
            //var buttonWidth = 10;
            //sourceRectangle = new Rectangle(0, 0, buttonWidth, sprite.Height);
            sourceRectangle = new Rectangle(0, 0, GameConstants.SNAKE_SIZE, GameConstants.SNAKE_SIZE);
        }

        public void Update(GameTime gameTime)
        {
            Lifespan -= gameTime.TotalGameTime.Milliseconds;
        }

        public void Eat()
        {
            IsDiggested = true;
        }

        public void SetFruitSprite()
        {
            switch (Type)
            {
                case FruitTypes.Banana:
                    sprite = Content.Load<Texture2D>("Images/banana");
                    break;
                case FruitTypes.Kiwi:
                    sprite = Content.Load<Texture2D>("Images/kiwi");
                    break;
                case FruitTypes.Pear:
                    sprite = Content.Load<Texture2D>("Images/pear");
                    break;
                case FruitTypes.Pineapple:
                    sprite = Content.Load<Texture2D>("Images/pineapple");
                    break;
                case FruitTypes.Plum:
                    sprite = Content.Load<Texture2D>("Images/plum");
                    break;
                case FruitTypes.Strawberry:
                    sprite = Content.Load<Texture2D>("Images/strawberry");
                    break;
                case FruitTypes.Watermelon:
                    sprite = Content.Load<Texture2D>("Images/watermelon");
                    break;
                default:
                    throw new Exception("Something happened!");
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, null, Color.White);
            //var rect = new Texture2D(graphicsDevice, GameConstants.SNAKE_SIZE*5, GameConstants.SNAKE_SIZE*5);
            //var data = new Color[GameConstants.SNAKE_SIZE *5 * GameConstants.SNAKE_SIZE * 5];
            //for (int i = 0; i < data.Length; ++i) data[i] = Color.Pink;
            //rect.SetData(data);
            //var coor = new Vector2(Position.X, Position.Y);
            //spriteBatch.Draw(rect, coor, Color.White);
        }
    }
}
