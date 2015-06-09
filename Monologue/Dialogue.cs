using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monologue
{
	/// <summary>
	/// This class will control displaying text on the screen
	/// </summary>
	public class Dialogue
	{
		/// <summary>
		/// The settings for the display
		/// </summary>
		public DialogueSettings Settings { get; set; }

		private string[] text;
		private int sinceUpdate = 0;
		private int currentFrame = 0;
		private int frameLength = 0;
		private int randomOffset = 0;
		private bool frameComplete = false;
		private Random random;
		private Vector2 backgroundBounds;
		private KeyboardState previous;

		/// <summary>
		/// Creates a dialogue class with specified settings
		/// </summary>
		/// <param name="s">Settings to be set</param>
		public Dialogue(DialogueSettings s)
		{
			this.Settings = s;
			
			this.random = new Random();
			this.backgroundBounds = new Vector2(s.Background.Bounds.Width, s.Background.Bounds.Height); 
		}

		/// <summary>
		/// This will set the text to be displayed in a box
		/// </summary>
		/// <param name="text">This is an array where each element will be a different frame of text</param>
		public void SetSpeech(string[] text)
		{
			this.text = text;
		}

		/// <summary>
		/// This will update the text to be drawn based on the time difference/key presses.
		/// </summary>
		/// <param name="gameTime">The game time to update based on</param>
		/// <param name="current">The current keypresses</param>
		/// <returns>Whether text is currently being displayed</returns>
		public bool Update(GameTime gameTime, KeyboardState current)
		{
			if (text != null)
			{
				if (current.IsKeyDown(this.Settings.ContinueKey) && previous.IsKeyUp(this.Settings.ContinueKey))
				{
					if (frameLength == text[currentFrame].Length)
					{
						if (currentFrame < text.Length - 1)
						{
							currentFrame++;
							frameLength = 0;
							frameComplete = false;
						}
						else
						{
							text = null;
							return false;
						}
					}
					else
					{
						frameLength = text[currentFrame].Length;
					}
				}

				previous = current;
				sinceUpdate += gameTime.ElapsedGameTime.Milliseconds;

				if (sinceUpdate > this.Settings.TextSpeed + randomOffset)
				{
					if (frameLength < text[currentFrame].Length)
					{
						if (this.Settings.TextSpeed == -1)
						{
							frameLength = text[currentFrame].Length;
						}
						else
						{
							frameLength++;
							randomOffset = random.Next(0, this.Settings.SpeedVariance);
						}

						sinceUpdate = 0;
					}
					else if (!frameComplete || this.Settings.BlinkSpeed > 0)
					{
						frameComplete = !frameComplete;
						sinceUpdate = -this.Settings.BlinkSpeed - this.Settings.TextSpeed - randomOffset;
					}
				}
			}

			return text != null;
		}

		/// <summary>
		/// This will draw the background, text, and possibly the 'end' icon
		/// </summary>
		/// <param name="spriteBatch">Spritebatch to draw with</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			if (text != null)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(this.Settings.Background, this.Settings.BackgroundOffset, Color.White);
				spriteBatch.DrawString(this.Settings.Font, text[currentFrame].Substring(0, frameLength), this.Settings.BackgroundOffset + this.Settings.TextOffset, this.Settings.FontColor);
				if (frameComplete)
					spriteBatch.Draw(this.Settings.NextView, this.Settings.BackgroundOffset + (backgroundBounds - this.Settings.NextViewOffset), Color.White);
				spriteBatch.End();
			}
		}
	}
}
