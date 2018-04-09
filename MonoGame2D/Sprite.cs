using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Riku_fighter
{
    public class Sprite
    {
        private int rows;
        private int columns;
        private float size;
        private int currentFrame;
        private Vector2 position;
        private int totalFrames;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 180;
        public Person person;
        public float xSpeed;
        public float yJump;
        public float gravitySpeed;
        private float screenHeight;
        private float screenWidth;
        private const float SKYRATIO = 3f / 3.5f;

        public Texture2D currentSpriteSheet
        {
            get;
            set;
        }

        public Texture2D leftSpriteSheet
        {
            get;
            set;
        }
        public Texture2D rightSpriteSheet
        {
            get;
            set;
        }

        // x coordinate of the center of the sprite
        public float x
        {
            get;
            set;
        }

        // y coordinate of the center of the sprite
        public float y
        {
            get;
            set;
        }

        // Angle of the sprite around central axis
        public float angle
        {
            get;
            set;
        }

        // Rate of change of x per second
        public float dX
        {
            get;
            set;
        }

        // Rate of change of y per second
        public float dY
        {
            get;
            set;
        }

        // Rate of change of angle per second
        public float dA
        {
            get;
            set;
        }

        // Scale of the texture, where 1 is its true size
        public float scale
        {
            get;
            set;
        }

        // Constructor
        public Sprite(Texture2D leftSpriteSheet, Texture2D rightSpriteSheet, Vector2 position, int size, int rows, int columns, float scale, Person person)
        {
            this.size = size;
            currentSpriteSheet = leftSpriteSheet;
            this.rows = rows;
            this.columns = columns;
            this.position = position;
            this.person = person;

            currentFrame = 0;

            this.scale = scale;

            totalFrames = rows * columns;

            int rnd = new Probability().ProbabilitySpeed();

            xSpeed = ScaleToHighDPI(rnd);
            yJump = ScaleToHighDPI(-1200f);
            gravitySpeed = ScaleToHighDPI(50f);

            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);

            this.leftSpriteSheet = leftSpriteSheet;
            this.rightSpriteSheet = rightSpriteSheet;

        } 

        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }

        // Update the position and angle of the sprite based on each rate of change and the time elapsed
        public void Update(GameTime gameTime)
        {
            this.x += this.dX * gameTime.ElapsedGameTime.Milliseconds /1000;
            this.y += this.dY * gameTime.ElapsedGameTime.Milliseconds /1000;
            this.angle += this.dA * gameTime.ElapsedGameTime.Seconds;

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                currentFrame++;
                timeSinceLastFrame = 0;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
            Move();
        }

        // Draw the sprite with the given SpriteBatch
        public void Draw(SpriteBatch spriteBatch)
        {
            int width = currentSpriteSheet.Width / columns;
            int height = currentSpriteSheet.Height / rows;
            int row = (int)((float)currentFrame / columns);
            int column = currentFrame % columns;
            int widthSize = width * (int)size;
            int heightSize = height * (int)size;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Vector2 spritePosition = new Vector2(this.x, this.y);
            spriteBatch.Draw(currentSpriteSheet, spritePosition, sourceRectangle, Color.White, this.angle, new Vector2(54, currentSpriteSheet.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
         }

        private void Move()
        {
            // Set game floor
            if (y > screenHeight * SKYRATIO)
            {
                dY = 0;
                y = screenHeight * SKYRATIO;
            }

            // Set right edge
            if (x >= screenWidth)
            {
                x = screenWidth;
                dX = xSpeed * -1;
                currentSpriteSheet = leftSpriteSheet;
            }

            // Set left edge
            if (x <= 80)
            {
                x = 80;
                dX = xSpeed;
                currentSpriteSheet = rightSpriteSheet;
            }

            // Accelerate the player downward each frame to simulate gravity.
            dY += gravitySpeed;
        }
    }
}
