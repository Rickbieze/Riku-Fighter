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
        public enum GameStates { playing, idle, paused }

        private List<String> messages;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float screenWidth;
        float screenHeight;

        bool gameStarted;

        GameStates currentGameState;

        Texture2D background;
        Texture2D startGameSplash;
        Texture2D pauseBackGround;

        Texture2D femaleRight;
        Texture2D femaleLeft;

        Texture2D NegFR;
        Texture2D NegFL;

        Texture2D MongFR;
        Texture2D MongFL;

        Texture2D AusFR;
        Texture2D AusFL;

        Texture2D NegMR;
        Texture2D NegML;

        Texture2D MongMR;
        Texture2D MongML;

        Texture2D AusMR;
        Texture2D AusML;

        Texture2D playerForward;
        Texture2D playerLeft;

        Texture2D ghost;

        List<Sprite> players;

        Random random;

        SpriteFont scoreFont;
        SpriteFont stateFont;
        SpriteFont consoleFont;

        SimulatorFacade simulator;
        int year = 0;
        int selectedPersonIndex = 0;

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
            currentGameState = GameStates.idle;

            base.Initialize();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize; // Attempt to launch in fullscreen mode

            // Get screen height and width, scaling them up if running on a high-DPI monitor.
            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);

            gameStarted = false;

            random = new Random();

            this.IsMouseVisible = true; // Hide the mouse within the app window
            graphics.IsFullScreen = true;
            players = new List<Sprite>();
        }


        // Load content (eg.sprite textures) before the app runs
        // Called once when the app is started
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            background = Content.Load<Texture2D>("grass");
            startGameSplash = Content.Load<Texture2D>("StartScreen");
            pauseBackGround = Content.Load<Texture2D>("pausebg");

            femaleRight = Content.Load<Texture2D>("femaleRight");
            femaleLeft = Content.Load<Texture2D>("femaleLeft");

            NegFR = Content.Load<Texture2D>("NegFR");
            NegFL = Content.Load<Texture2D>("NegFL");

            MongFR = Content.Load<Texture2D>("MongFR");
            MongFL = Content.Load<Texture2D>("MongFL");

            AusFR = Content.Load<Texture2D>("AusFR");
            AusFL = Content.Load<Texture2D>("AusFL");

            NegMR = Content.Load<Texture2D>("NegMR");
            NegML = Content.Load<Texture2D>("NegML");

            MongMR = Content.Load<Texture2D>("MongMR");
            MongML = Content.Load<Texture2D>("MongML");

            AusMR = Content.Load<Texture2D>("AusMR");
            AusML = Content.Load<Texture2D>("AusML");

            playerForward = Content.Load<Texture2D>("playerForward");
            playerLeft = Content.Load<Texture2D>("playerLeft");

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
            // currentGameState = gameState.getCurrentState();
            if (currentGameState == GameStates.playing)
            {
                simulate();
                foreach (var person in players)
                {
                    person.Update(gameTime);
                }
            }
            if (currentGameState == GameStates.paused)
            {
                Debug.WriteLine("paused.");
            }

            // Update animated SpriteClass objects based on their current rates of change
            base.Update(gameTime);
        }

        private void simulate()
        {
            if (year / 50 == 1)
            {
                Task simulate = new Task(simulator.RunSimulator);
                simulate.Start();
                List<Person> list = simulator.GetBabiesThisRound();
                foreach (var item in list.ToList())
                {
                    List<Texture2D> spriteSheets = calculateCorrectSpriteSheet(item);
                    addConsoleMessage(item.FirstName + " was born");
                    Sprite i;
                    i = new Sprite(spriteSheets.ElementAt(0), spriteSheets.ElementAt(1), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), item);
                    players.Add(i);
                }
                simulator.DeleteBabyList();

                List<Person> deadList = simulator.GetDeadThisRound();
                foreach (var deadPerson in deadList.ToList())
                {
                    foreach (var livingPerson in players.ToList())
                    {
                        Person p = livingPerson.person;
                        if (p.FirstName == deadPerson.FirstName && p.LastName == deadPerson.LastName && p.Birthdate == deadPerson.Birthdate)
                        {
                            addConsoleMessage(deadPerson.FirstName + " is no longer with us.");
                            livingPerson.currentSpriteSheet = ghost;
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
                year = 0;
            }
            year++;
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

            if (currentGameState == GameStates.idle)
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
            if (currentGameState == GameStates.playing)
            {
                // Draw background
                spriteBatch.Draw(background, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);
            
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
                    messageY = messageY + 19;
                }

            }
            
            if(currentGameState == GameStates.paused)
            {
                int initialY = 500;
                var screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
                var textureCenter = new Vector2(pauseBackGround.Width / 2, pauseBackGround.Height / 2);
                spriteBatch.Draw(pauseBackGround, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                List<String> details = getPersonDetailString(players[selectedPersonIndex].person);
                Debug.WriteLine(screenHeight / 2);
                foreach (var item in details)
                {
                    String detail = item;
                    Vector2 consoleSize = consoleFont.MeasureString(detail);
                    spriteBatch.DrawString(consoleFont, detail, new Vector2(screenWidth / 2 - consoleSize.X / 2, initialY), Color.White);
                    initialY = initialY + 19;

                }
                


                // spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.ForestGreen);

            }

            spriteBatch.End(); // Stop drawing

            base.Draw(gameTime);
        }

        private List<String> getPersonDetailString(Person person)
        {

            List<String> details = new List<string>();

            details.Add("Name: " + person.FirstName + " " + person.LastName);
            details.Add("Birthday: " + person.Birthdate.ToString() + " (age: " + person.Age + ")");
            details.Add("Gender: " + person.Gender.ToString());
            details.Add("Father: " + person.Father.FirstName + " " + person.Father.LastName);

            return details;
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
            setSpriteSheets();
            await Task.Delay(TimeSpan.FromSeconds(1));

        }
        private void setSpriteSheets()
        {
            var spriteSheets = new List<Texture2D>();
            foreach (var human in simulator.AliveHumans)
            {
                spriteSheets = calculateCorrectSpriteSheet(human);
                Sprite person;
                person = new Sprite(spriteSheets.ElementAt(0), spriteSheets.ElementAt(1), new Vector2(857, 1672), 4, 1, 8, ScaleToHighDPI(1.7f), human);
                players.Add(person);
            }
        }

        private List<Texture2D> calculateCorrectSpriteSheet(Person person)
        {
            List<Texture2D> result = new List<Texture2D>();
            if (person.Gender == Gender.Genders.female)
            {
                if (person.Race.GetType() == typeof(Race.Caucasoid))
                {
                    result.Add(femaleLeft);
                    result.Add(femaleRight);

                }
                else if (person.Race.GetType() == typeof(Race.Negroid))
                {
                    result.Add(NegFL);
                    result.Add(NegFR);
                }
                else if (person.Race.GetType() == typeof(Race.Mongoloid))
                {
                    result.Add(MongFL);
                    result.Add(MongFR);
                }
                else
                {
                    result.Add(AusFL);
                    result.Add(AusFR);
                }
            }

            else
            {
                if (person.Race.GetType() == typeof(Race.Caucasoid))
                {
                    result.Add(playerLeft);
                    result.Add(playerForward);
                }
                else if (person.Race.GetType() == typeof(Race.Negroid))
                {
                    result.Add(NegML);
                    result.Add(NegMR);
                }
                else if (person.Race.GetType() == typeof(Race.Mongoloid))
                {
                    result.Add(MongML);
                    result.Add(MongMR);
                }
                else
                {
                    result.Add(AusML);
                    result.Add(AusMR);
                }
            }
            return result;
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
            if (currentGameState == GameStates.idle)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    currentGameState = GameStates.playing;
                }
                return;
            }

            if (currentGameState == GameStates.playing)
            {
                if (state.IsKeyDown(Keys.Tab))
                {
                    currentGameState = GameStates.paused;

                }
                return;
            }
            if(currentGameState == GameStates.paused)
            {
                if (state.IsKeyDown(Keys.Enter))
                {
                    currentGameState = GameStates.playing;
                }
                if (state.IsKeyDown(Keys.Right))
                {
                    selectedPersonIndex = 1;
                }
            }

        }
    }
}
