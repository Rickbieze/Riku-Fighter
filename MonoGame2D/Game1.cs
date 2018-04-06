using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public class Game1 : Game
    {
        // The ratio of the screen that is sky versus ground
        const float SKYRATIO = 3f / 3.5f;

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

        List<SpriteClass> players;
        SpriteClass player1;
        SpriteClass player2;

        Random random;

        SpriteFont scoreFont;
        SpriteFont stateFont;

        SimulatorFacade simulator;
        int day = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";  // Set the directory where game assets can be found by the ContentManager
        }


        // Give variables their initial states
        // Called once when the app is started
        protected override void Initialize()
        {

            simulator = new SimulatorFacade();
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

            this.IsMouseVisible = true; // Hide the mouse within the app window

            players = new List<SpriteClass>();

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
            //player1 = new SpriteClass(Content.Load<Texture2D>("playerForward"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f));
            //player2 = new SpriteClass(Content.Load<Texture2D>("playerLeft"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f));

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
            if (gameStarted)
            {
                if (day / 50 == 1)
                {
                    Task simulate = new Task(simulator.RunSimulator);
                    simulate.Start();
                    List<Person> list = simulator.GetBabiesThisRound();
                    foreach (var item in list)
                    {
                        Debug.WriteLine(item.FirstName + " new babies");
                        SpriteClass i;
                        i = new SpriteClass(Content.Load<Texture2D>("playerForward"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), item);
                        players.Add(i);
                    }
                    simulator.deleteBabyList();

                    List<Person> deadList = simulator.GetDeadThisRound();
                    foreach (var deadPerson in deadList)
                    {
                        Debug.WriteLine(deadPerson.FirstName + " DIED");
                        SpriteClass i;
                        i = new SpriteClass(Content.Load<Texture2D>("playerForward"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), deadPerson);
                        foreach (var livingPerson in players.ToList())
                        {
                            Person p = livingPerson.person;
                            if (p.FirstName == deadPerson.FirstName && p.LastName == deadPerson.LastName && p.Birthdate == deadPerson.Birthdate)
                            {
                                Debug.WriteLine("OMG A MATCH " + deadPerson.FirstName + " is dead yo ");
                                players.Remove(livingPerson);
                            }
                        }


                    }
                    simulator.deleteDeadList();
                    simulator.GetDeadThisRound();
                    day = 0;
                }
            }
            // Stop all movement when the game ends
            //if (gameOver)
            //{
            //    player1.dX = 0;
            //    player1.dY = 0;

            //    player2.dX = 0;
            //    player2.dY = 0;
            //}
            if (gameStarted)
            {
                if (day / 50 == 1)
                {
                    simulator.RunSimulator();
                    day = 0;
                    Debug.WriteLine(simulator.getCurrentDate());
                }
                foreach (var person in players)
                {
                    person.Update(gameTime);
                }
                
                //player2.Update(gameTime);
                day++;
            }

            // Update animated SpriteClass objects based on their current rates of change
            base.Update(gameTime);

        }


        // Draw the updated game state each frame
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen

            spriteBatch.Begin(); // Begin drawing

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

            if (!gameStarted)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);

                String title = "Life Simulator";
                String pressSpace = "Press Space to start the simulation";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.ForestGreen);
                spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White);
            }
            if (gameStarted)
            {
                // Draw grass
                spriteBatch.Draw(grass, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);
                // If the game is not over, draw it in black
                spriteBatch.DrawString(scoreFont, score.ToString(), new Vector2(screenWidth / 2, 50), Color.Black);

                // Draw the players with the SpriteClass method
                foreach (var person in players)
                {
                    person.Draw(spriteBatch);
                }
                //player1.Draw(spriteBatch);
                //player2.Draw(spriteBatch);

                // draw year
                String year = simulator.getCurrentDate();
                Vector2 yearSize = stateFont.MeasureString(year);
                spriteBatch.DrawString(stateFont, year, new Vector2(screenWidth / 2 - yearSize.X / 2, screenHeight - 800), Color.White);


                // draw stats
                SimulatorStatistics stats = simulator.getSimulatorStatistics();
                String statistics = "Alive Humans: "+ stats.getAlive().ToString() + " Dead Humans: "+stats.getDead().ToString();
                Vector2 statsSize = stateFont.MeasureString(statistics);

                spriteBatch.DrawString(stateFont, statistics, new Vector2(screenWidth / 2 - statsSize.X / 2, screenHeight - 900), Color.White);

            }


            // Draw the text horizontally centered


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
            foreach (var person in players)
            {
                person.x = 0;
                person.y = screenHeight * SKYRATIO;
            }

            score = 0; // Reset score
        }


        // Handle user input from the keyboard
        public void KeyboardHandler()
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

            foreach (var person in players)
            {
                person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("playerForward"), Content.Load<Texture2D>("playerLeft"));
            }

            //player1.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("playerForward"), Content.Load<Texture2D>("playerLeft"));
            //player2.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("playerForward"), Content.Load<Texture2D>("playerLeft"));
        }
    }
}
