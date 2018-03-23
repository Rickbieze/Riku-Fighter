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

        Button CurrentButton;
        int index = 0;

        // sprite texture
        public Texture2D texture
        {
            get;
        }

        // Constructor
        public Menu(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.graphics = graphics;

            this.texture = content.Load<Texture2D>("menu");
            addMenuButtons();
            CurrentButton = buttons[0];
        }

        // Update the position and angle of the sprite based on each rate of change and the time elapsed
        public void Update(GameTime gameTime)
        {
 
        }

        // Draw the sprite with the given SpriteBatch
        public void Draw(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(texture, rect, color);
            foreach (var button in buttons)
            {
                button.Update();
                button.Draw(spriteBatch);
            }
        }

        private void addMenuButtons()
        {
            Viewport viewport = graphics.Viewport;
            Texture2D defaultButton = content.Load<Texture2D>("newgame_default");
            Texture2D selectedButton = content.Load<Texture2D>("newgame_selected");

            Texture2D quitDefault = content.Load<Texture2D>("quit_default");
            Texture2D quitSelected = content.Load<Texture2D>("quit_selected");

            Rectangle button = new Rectangle((viewport.Width / 2) - (306 / 2), 150, 306, 64);

            buttons.Add(new Button(button, defaultButton, selectedButton, defaultButton, true));

            buttons.Add(new Button(new Rectangle((viewport.Width / 2) - (306 / 2), 250, 306, 64), quitDefault, quitSelected, quitDefault, false));
        }

        // Detect collision between two rectangular sprites

        KeyboardState oldState;
        public void keyHandler(KeyboardState state)
        {
            Debug.WriteLine(index);
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
            }
        }
    }
}
