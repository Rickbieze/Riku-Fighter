using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Riku_fighter
{
    class Menu
    {
        List<Button> buttons = new List<Button>();
        ContentManager content;
        GraphicsDevice graphics;
        public Boolean shouldStartGame { get; set; }

        Button CurrentButton;
        int index = 0;

        // sprite texture
        private Texture2D background;
        private Texture2D logo;

        // Constructor
        public Menu(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.graphics = graphics;
            this.shouldStartGame = false;

            this.background = content.Load<Texture2D>("menubackground");
            this.logo = content.Load<Texture2D>("rikku_fighter_logo");
            addMenuButtons();
            CurrentButton = buttons[0];
        }


        public void Update(GameTime gameTime)
        {
 
        }

        // Draw the items with the given SpriteBatch
        public void Draw(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            Viewport viewport = graphics.Viewport;
            Rectangle logoRect = new Rectangle((viewport.Width / 2) - (600 / 2), 150, 600, 200);

            
            spriteBatch.Draw(background, rect, color);
            spriteBatch.Draw(logo, logoRect, color);
            foreach (var button in buttons)
            {
                button.Update();
                button.Draw(spriteBatch);
            }
        }

        private void addMenuButtons()
        {
            Viewport viewport = graphics.Viewport;
            Texture2D defaultButton = content.Load<Texture2D>("button_play_default");
            Texture2D selectedButton = content.Load<Texture2D>("button_play_selected");

            Texture2D quitDefault = content.Load<Texture2D>("button_quit_default");
            Texture2D quitSelected = content.Load<Texture2D>("button_quit_selected");
            

            Rectangle button = new Rectangle((viewport.Width / 2) - (306 / 2), 350, 306, 64);

            buttons.Add(new Button(button, defaultButton, selectedButton, defaultButton, true));

            buttons.Add(new Button(new Rectangle((viewport.Width / 2) - (306 / 2), 450, 306, 64), quitDefault, quitSelected, quitDefault, false));
        }

        KeyboardState oldState;
        public void keyHandler(KeyboardState state)
        {
            bool keylock = false;

            if (!keylock)
            {
                if (state.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    if (index < buttons.Count - 1)
                    {
                        index++;
                        CurrentButton.Play();
                        CurrentButton.setSelected(false);
                        CurrentButton = buttons[index];
                        CurrentButton.setSelected(true);
                    }
                    else
                    {

                    }
                }
                if (state.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    if (index > 0)
                    {
                        index--;
                        CurrentButton.Play();
                        CurrentButton.setSelected(false);
                        CurrentButton = buttons[index];
                        CurrentButton.setSelected(true);
                    }

                }
                if (state.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    Debug.WriteLine(CurrentButton);
                    Debug.WriteLine(buttons[1]);
                    if (CurrentButton == buttons[0])
                    {
                        shouldStartGame = true;
                        //gameStateManager.Change("worldmap", new Player());
                    }
                    else if (CurrentButton == buttons[1])
                    {
                        App.Current.Exit();
                    }

                }
            }
        }
    }
}
