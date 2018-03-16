﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    class SpriteClass
    {
        const float HITBOXSCALE = .5f; // experiment with this value to make the collision detection more or less forgiving

        private int rows;
        private int columns;
        private float size;
        private int currentFrame;
        private Vector2 position;
        private int totalFrames;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 180;

        // sprite texture
        public Texture2D texture
        {
            get;
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
        public SpriteClass(Texture2D texture, Vector2 position, int size, int rows, int columns, float scale)
        {
            this.size = size;
            this.texture = texture;
            this.rows = rows;
            this.columns = columns;
            this.position = position;

            currentFrame = 0;

            this.scale = scale;

            totalFrames = rows * columns;

            // Load the specified texture
            //var stream = TitleContainer.OpenStream(textureName);
            //texture = Texture2D.FromStream(graphicsDevice, stream);
        }

        // Update the position and angle of the sprite based on each rate of change and the time elapsed
        public void Update(GameTime gameTime)
        {
            this.x += this.dX * gameTime.ElapsedGameTime.Milliseconds /1000;
            this.y += this.dY * gameTime.ElapsedGameTime.Milliseconds /1000;
            this.angle += this.dA * gameTime.ElapsedGameTime.Seconds;
            //Debug.WriteLine(gameTime);

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
        }

        // Draw the sprite with the given SpriteBatch
        public void Draw(SpriteBatch spriteBatch)
        {
            int width = texture.Width / columns;
            int height = texture.Height / rows;
            int row = (int)((float)currentFrame / columns);
            int column = currentFrame % columns;
            int widthSize = width * (int)size;
            int heightSize = height * (int)size;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width * (int)size, height * (int)size);
            Vector2 spritePosition = new Vector2(this.x, this.y);
            spriteBatch.Draw(texture, spritePosition, sourceRectangle, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
            //Debug.WriteLine(row + " :: " + column);


            //Debug.WriteLine((this.x).ToString() + " x::y " + (this.y).ToString());
            // Determine the position vector of the sprite


            // Draw the sprite
            //spriteBatch.Draw(texture, spritePosition, null, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        // Detect collision between two rectangular sprites
        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (this.x + this.texture.Width * this.scale * HITBOXSCALE / 2 < otherSprite.x - otherSprite.texture.Width * otherSprite.scale / 2) return false;
            if (this.y + this.texture.Height * this.scale * HITBOXSCALE / 2 < otherSprite.y - otherSprite.texture.Height * otherSprite.scale / 2) return false;
            if (this.x - this.texture.Width * this.scale * HITBOXSCALE / 2 > otherSprite.x + otherSprite.texture.Width * otherSprite.scale / 2) return false;
            if (this.y - this.texture.Height * this.scale * HITBOXSCALE / 2 > otherSprite.y + otherSprite.texture.Height * otherSprite.scale / 2) return false;
            return true;
        }

    }
}
