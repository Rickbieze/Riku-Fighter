using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Riku_fighter
{
    public class Game1 : Game
    {
        // The ratio of the screen that is sky versus ground
        const float SKYRATIO = 3f / 3f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int score = 0;

        float screenWidth;
        float screenHeight;

        bool spaceDown;
        bool gameStarted;
        bool gameOver;

        Texture2D grass;
        Texture2D startGameSplash;
        Texture2D gameOverTexture;

        SpriteClass dino;
        
        Random random;

        SpriteFont scoreFont;
        SpriteFont stateFont;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";  // Set the directory where game assets can be found by the ContentManager
        }


        // Give variables their initial states
        // Called once when the app is started
        protected override void Initialize()
        {
            base.Initialize();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize; // Attempt to launch in fullscreen mode

            // Get screen height and width, scaling them up if running on a high-DPI monitor.
            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);

            spaceDown = false;
            gameStarted = false;
            gameOver = false;

            random = new Random();

            score = 0;

            this.IsMouseVisible = false; // Hide the mouse within the app window

        }


        // Load content (eg.sprite textures) before the app runs
        // Called once when the app is started
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            grass = Content.Load<Texture2D>("grass");
            startGameSplash = Content.Load<Texture2D>("start-splash");
            gameOverTexture = Content.Load<Texture2D>("game-over");

            // Construct SpriteClass objects
            dino = new SpriteClass(Content.Load<Texture2D>("test"), new Vector2(857, 1672), 5, 4, 1, ScaleToHighDPI(5f));

            // Load fonts
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
        }


        // Unloads any non ContentManager content
        protected override void UnloadContent()
        {
        }


        // Updates the logic of the game state each frame, checking for collision, gathering input, etc.
        protected override void Update(GameTime gameTime)
        {
            KeyboardHandler(); // Handle keyboard input

            // Stop all movement when the game ends
            if (gameOver)
            {
                dino.dX = 0;
                dino.dY = 0;
            }

            // Update animated SpriteClass objects based on their current rates of change
            dino.Update(gameTime);

            base.Update(gameTime);
        }


        // Draw the updated game state each frame
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen

            spriteBatch.Begin(); // Begin drawing

            // Draw grass
            spriteBatch.Draw(grass, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);

            if (gameOver)
            {
                // Draw game over texture
                spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, screenHeight / 4 - gameOverTexture.Width / 2), Color.White);

                String pressEnter = "Press Enter to restart!";

                // Measure the size of text in the given font
                Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White);

                // If the game is over, draw the score in red
                spriteBatch.DrawString(scoreFont, score.ToString(), new Vector2(screenWidth - 100, 50), Color.Red);
            }

            // If the game is not over, draw it in black
            else spriteBatch.DrawString(scoreFont, score.ToString(), new Vector2(screenWidth - 100, 50), Color.Black);

            // Draw broccoli and dino with the SpriteClass method
            dino.Draw(spriteBatch);

            if (!gameStarted)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);

                String title = "Riku Fighter";
                String pressSpace = "Press Space to start";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.ForestGreen);
                spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White);
            }

            spriteBatch.End(); // Stop drawing

            base.Draw(gameTime);
        }

        // Scale a number of pixels so that it displays properly on a High-DPI screen, such as a Surface Pro or Studio
        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }

        // Start a new game, either when the app starts up or after game over
        public void StartGame()
        {
            // Reset dino position
            dino.x = screenWidth / 2;
            dino.y = screenHeight * SKYRATIO;

            score = 0; // Reset score
        }


        // Handle user input from the keyboard
        void KeyboardHandler()
        {
            KeyboardState state = Keyboard.GetState();
            
            // Quit the game if Escape is pressed.
            if (state.IsKeyDown(Keys.Escape)) Exit();

            // Start the game if Space is pressed.
            // Exit the keyboard handler method early, preventing the dino from jumping on the same keypress.
            if (!gameStarted)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    gameStarted = true;
                    spaceDown = true;
                    gameOver = false;
                }
                return;
            }

            // Restart the game if Enter is pressed
            if (gameOver)
            {
                if (state.IsKeyDown(Keys.Enter))
                {
                    StartGame();
                    gameOver = false;
                }
            }

            dino.keyHandler(state, SKYRATIO, screenHeight, screenWidth);
        }
    }
}
