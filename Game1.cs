using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AlchemyWizard
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Texture2D _pixel;

        private int _hp, _maxHp;
        private int _mp, _maxMp;
        private KeyboardState _prevKeyboard;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            _maxHp = 100;
            _hp = _maxHp;
            _maxMp = 50;
            _mp = _maxMp;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currKeyboard = Keyboard.GetState();

            if (_hp > 0)
            {
                if (currKeyboard.IsKeyDown(Keys.H) && _prevKeyboard.IsKeyUp(Keys.H))
                    _hp = MathHelper.Max(0, _hp - 10);
                if (currKeyboard.IsKeyDown(Keys.H) && currKeyboard.IsKeyDown(Keys.LeftShift))
                    _hp = MathHelper.Min(_maxHp, _hp + 10);
                if (currKeyboard.IsKeyDown(Keys.M) && _prevKeyboard.IsKeyUp(Keys.M))
                    _mp = MathHelper.Max(0, _mp - 5);
                if (currKeyboard.IsKeyDown(Keys.M) && currKeyboard.IsKeyDown(Keys.LeftShift))
                    _mp = MathHelper.Min(_maxMp, _mp + 5);
            }

            if (currKeyboard.IsKeyDown(Keys.Escape))
                Exit();

            _prevKeyboard = currKeyboard;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            DrawBar("HP", _hp, _maxHp, new Vector2(20, 20), Color.Green);
            DrawBar("MP", _mp, _maxMp, new Vector2(20, 70), Color.Blue);

            if (_hp <= 0)
            {
                string gameOverText = "gameover";
                Vector2 textSize = _font.MeasureString(gameOverText);
                Vector2 position = new Vector2(
                    (GraphicsDevice.Viewport.Width - textSize.X) / 2,
                    (GraphicsDevice.Viewport.Height - textSize.Y) / 2
                );
                _spriteBatch.DrawString(_font, gameOverText, position, Color.Red);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBar(string name, int current, int max, Vector2 topLeft, Color color)
        {
            int barWidth = 200;
            int barHeight = 20;
            float percent = (float)current / max;

            string text = $"{name}: {current}/{max}";
            Vector2 textSize = _font.MeasureString(text);
            _spriteBatch.DrawString(_font, text, topLeft, Color.White);

            Vector2 barPos = topLeft + new Vector2(0, textSize.Y + 5);
            _spriteBatch.Draw(_pixel, new Rectangle((int)barPos.X, (int)barPos.Y, barWidth, barHeight), Color.Gray);

            int fillWidth = (int)(barWidth * percent);
            if (fillWidth > 0)
                _spriteBatch.Draw(_pixel, new Rectangle((int)barPos.X, (int)barPos.Y, fillWidth, barHeight), color);

            _spriteBatch.Draw(_pixel, new Rectangle((int)barPos.X, (int)barPos.Y, barWidth, barHeight), Color.White * 0.8f);
        }
    }
}