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

        private List<String> messages;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float screenWidth;
        float screenHeight;

        bool gameStarted;
        bool gameOver;

        Texture2D grass;
        Texture2D startGameSplash;
        Texture2D gameOverTexture;
        Texture2D ghost;

        List<SpriteClass> players;

        Random random;

        SpriteFont scoreFont;
        SpriteFont stateFont;
        SpriteFont consoleFont;

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
            messages = new List<string>();
            simulator = new SimulatorFacade();
            base.Initialize();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize; // Attempt to launch in fullscreen mode

            // Get screen height and width, scaling them up if running on a high-DPI monitor.
            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);

            gameStarted = false;
            gameOver = false;

            random = new Random();

            this.IsMouseVisible = true; // Hide the mouse within the app window
            graphics.IsFullScreen = true;
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

            // Load fonts
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
            consoleFont = Content.Load<SpriteFont>("File");

            ghost = Content.Load<Texture2D>("Ghost");
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
                    foreach (var item in list.ToList())
                    {
                        addConsoleMessage(item.FirstName + " was born");
                        SpriteClass i;
                        i = new SpriteClass(Content.Load<Texture2D>("playerForward"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), item);
                        players.Add(i);
                    }
                    simulator.DeleteBabyList();

                    List<Person> deadList = simulator.GetDeadThisRound();
                    foreach (var deadPerson in deadList.ToList())
                    {
                        SpriteClass i;
                        i = new SpriteClass(Content.Load<Texture2D>("playerForward"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), deadPerson);
                        foreach (var livingPerson in players.ToList())
                        {
                            Person p = livingPerson.person;
                            if (p.FirstName == deadPerson.FirstName && p.LastName == deadPerson.LastName && p.Birthdate == deadPerson.Birthdate)
                            {
                                addConsoleMessage(deadPerson.FirstName + " is no longer with us.");
                                // players.Remove(livingPerson);
                                livingPerson.texture = ghost;
                                livingPerson.xSpeed = 0;
                                livingPerson.dX = 0;
                                livingPerson.gravitySpeed = -20f;
                                killPerson();

                                async Task killPerson()
                                {
                                    await Task.Delay(2000);
                                    players.Remove(livingPerson);
                                }
                            }
                        }
                    }
                    simulator.DeleteDeadList();
                    simulator.GetDeadThisRound();
                    day = 0;
                }
            }

            if (gameStarted)
            {
                if (day / 50 == 1)
                {
                    simulator.RunSimulator();
                    day = 0;
                }
                foreach (var person in players)
                {
                    person.Update(gameTime);
                }
                day++;
            }

            // Update animated SpriteClass objects based on their current rates of change
            base.Update(gameTime);
        }

        public void addConsoleMessage(String message)
        {
            if(messages.Count > 20)
            {
                messages.RemoveAt(0);
                messages.Add(message);
            }
            else { 
                messages.Add(message);
            }
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
            
                // Draw the players with the SpriteClass method
                foreach (var person in players)
                {
                    person.Draw(spriteBatch);
                }

                // draw year
                String year = "Current year: "+simulator.getCurrentDate();
                Vector2 yearSize = stateFont.MeasureString(year);
                spriteBatch.DrawString(stateFont, year, new Vector2(screenWidth / 2 - yearSize.X / 2, screenHeight - 800), Color.White);


                // draw stats
                SimulatorStatistics stats = simulator.getSimulatorStatistics();
                String statistics = "Alive Humans: "+ stats.getAlive().ToString() + " Dead Humans: "+stats.getDead().ToString();
                Vector2 statsSize = stateFont.MeasureString(statistics);
                spriteBatch.DrawString(stateFont, statistics, new Vector2(screenWidth / 2 - statsSize.X / 2, screenHeight - 900), Color.White);

                // draw simulator message
                int messageY = 0;
                foreach (var message in messages)
                {
                    Vector2 consoleSize = consoleFont.MeasureString(message);
                    spriteBatch.DrawString(consoleFont, message, new Vector2(screenWidth - consoleSize.X, messageY), Color.White);
                    messageY = messageY + 18;
                }
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

        private async Task initialSpawn()
        {
            foreach (var human in simulator.AliveHumans)
            {
                Debug.WriteLine("-CREATION OF MANKIND-");
                SpriteClass i;
                i = new SpriteClass(Content.Load<Texture2D>("playerForward"), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), human);
                players.Add(i);
            }
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        // Start a new game, either when the app starts up or after game over
        public async void StartGame()
        {
            await initialSpawn();
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
                if(person.person.Gender == Gender.Genders.female)
                {
                    if(person.person.Race.GetType() == typeof(Race.Caucasoid))
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("femaleRight"), Content.Load<Texture2D>("femaleLeft"));
                    }
                    else if(person.person.Race.GetType() == typeof(Race.Negroid))
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("NegFR"), Content.Load<Texture2D>("NegFL"));
                    }
                    else if(person.person.Race.GetType() == typeof(Race.Mongoloid))
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("MongFR"), Content.Load<Texture2D>("MongFL"));
                    }
                    else
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("AusFR"), Content.Load<Texture2D>("AusFL"));
                    }
                }
                else
                {
                    if (person.person.Race.GetType() == typeof(Race.Caucasoid))
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("playerForward"), Content.Load<Texture2D>("playerLeft"));
                    }
                    else if (person.person.Race.GetType() == typeof(Race.Negroid))
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("NegMR"), Content.Load<Texture2D>("NegML"));
                    }
                    else if (person.person.Race.GetType() == typeof(Race.Mongoloid))
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("MongMR"), Content.Load<Texture2D>("MongML"));
                    }
                    else
                    {
                        person.keyHandler(state, SKYRATIO, screenHeight, screenWidth, Content.Load<Texture2D>("AusMR"), Content.Load<Texture2D>("AusML"));
                    }
                }
            }
        }
    }
}
