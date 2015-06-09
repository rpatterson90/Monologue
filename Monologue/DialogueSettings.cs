using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Monologue
{
	/// <summary>
	/// This class contains everything that can be customized in a conversation window
	/// </summary>
	[Serializable]
	public class DialogueSettings
	{
		[NonSerialized]
		private SpriteFont font;
		/// <summary>
		/// The font to display the text with.
		/// </summary>
		public SpriteFont Font { get { return this.font; } set { this.font = value; } }

		[NonSerialized]
		private Color fontColor;
		private int r, g, b, a;
		/// <summary>
		/// The color for the font to be colored. Default: black
		/// </summary>
		public Color FontColor
		{
			get { return this.fontColor; }
			set
			{
				this.fontColor = value;
				this.r = value.R;
				this.g = value.G;
				this.b = value.B;
				this.a = value.A;
			}
		}

		[NonSerialized]
		private Texture2D background;
		/// <summary>
		/// The background for the dialogue. This will determine the size of the background. Default: (screen width, 150) transparent
		/// </summary>
		public Texture2D Background { get { return this.background; } set { this.background = value; } }


		[NonSerialized]
		private Texture2D nextTexture;
		/// <summary>
		/// The texture to display to move to the next frame/close the dialogue. Default: (10, 10) Black square
		/// </summary>
		public Texture2D NextView { get { return this.nextTexture; } set { this.nextTexture = value; } }

		/// <summary>
		/// The speed in milliseconds for each character to be displayed. If it is -1, it will display all the text at once. Default: 50ms
		/// </summary>
		public int TextSpeed { get; set; }

		/// <summary>
		/// The speed the icon for next frame will blink (if this is 0, it will not blink). Default: 0
		/// </summary>
		public int BlinkSpeed { get; set; }

		/// <summary>
		/// The key to hit to change the screen/show all the text. Default: Space bar
		/// </summary>
		public Keys ContinueKey { get; set; }
		
		/// <summary>
		/// This is the maximum variance for a character to be displayed (a random number from 0 to this number is added to text speed for the true speed) Default: 0
		/// </summary>
		public int SpeedVariance { get; set; }

		[NonSerialized]
		private Vector2 backgroundOffset;
		private float bOffsetX;
		private float bOffsetY;
		/// <summary>
		/// This is the offset for the background/text to be displayed. Default: (0,0) (top of the screen)
		/// </summary>
		public Vector2 BackgroundOffset 
		{ 
			get { return this.backgroundOffset; } 
			set 
			{
				this.backgroundOffset = value;
				this.bOffsetX = value.X;
				this.bOffsetY = value.Y;
			} 
		}

		[NonSerialized]
		private Vector2 textOffset;
		private float tOffsetX;
		private float tOffsetY;
		/// <summary>
		/// This is the offset from the offset for the text to be displayed. Default: (20,20)
		/// </summary>
		public Vector2 TextOffset
		{
			get { return this.textOffset; }
			set
			{
				this.textOffset = value;
				this.tOffsetX = value.X;
				this.tOffsetY = value.Y;
			}
		}

		[NonSerialized]
		private Vector2 nextViewOffset;
		private float nOffsetX;
		private float nOffsetY;
		/// <summary>
		/// This is the offset from the background offset for the icon to the next frame (it is subtracted from the bottom right corner). Default: (25,25)
		/// </summary>
		public Vector2 NextViewOffset
		{
			get { return this.nextViewOffset; }
			set
			{
				this.nextViewOffset = value;
				this.nOffsetX = value.X;
				this.nOffsetY = value.Y;
			}
		}

		/// <summary>
		/// Creates a Dialogue settings with a default texture for the background and next arrow
		/// </summary>
		/// <param name="f">The font to display the text with</param>
		/// <param name="graphics">The graphics device for the display</param>
		public DialogueSettings(SpriteFont f, GraphicsDevice graphics)
			: this()
		{
			this.Font = f;

			Color[] colors = new Color[100];
			for (int i = 0; i < 100; i++)
			{
				colors[i] = Color.Black;
			}

			this.NextView = new Texture2D(graphics, 10, 10);
			this.NextView.SetData(colors);

			int defaultHeight = 150;
			colors = new Color[graphics.Viewport.Width * defaultHeight];
			for (int i = 0; i < graphics.Viewport.Width * defaultHeight; i++)
			{
				colors[i] = Color.Transparent;
			}

			this.Background = new Texture2D(graphics, graphics.Viewport.Width, defaultHeight);
			this.Background.SetData(colors);

			this.BackgroundOffset = new Vector2(0, graphics.Viewport.Height - defaultHeight);
		}

		/// <summary>
		/// Creates a Dialogue settings with custom background and next arrows.
		/// </summary>
		/// <param name="f">The font to display the text with</param>
		/// <param name="b">The texture for the background</param>
		/// <param name="n">The texture for the next arrow</param>
		public DialogueSettings(SpriteFont f, Texture2D b, Texture2D n)
			: this()
		{
			this.Font = f;
			this.Background = b;
			this.NextView = n;
		}

		/// <summary>
		/// Private constructor to set the defaults
		/// </summary>
		private DialogueSettings()
		{
			this.BackgroundOffset = new Vector2(0);
			this.BlinkSpeed = 0;
			this.TextSpeed = 50;
			this.ContinueKey = Keys.Space;
			this.SpeedVariance = 0;
			this.TextOffset = new Vector2(20);
			this.NextViewOffset = new Vector2(25);
			this.FontColor = Color.Black;
		}

		/// <summary>
		/// This will save the current settings to the file specified. It will not save images or fonts.
		/// </summary>
		/// <param name="filename">The filepath to save the settings to</param>
		/// <exception cref="IOException">This will throw exceptions for any file related exceptions</exception>
		/// <returns>Whether saving was a success</returns>
		public bool Save(string filename)
		{
			BinaryFormatter formatter = new BinaryFormatter();

			Stream stream = File.Open(filename, FileMode.Create);
			formatter.Serialize(stream, this);
			stream.Close();

			return true;
		}

		/// <summary>
		/// This will load a settings object from the file specified. It will not load images or fonts.
		/// </summary>
		/// <param name="filename">The filepath to load from</param>
		/// <exception cref="IOException">This will throw exceptions for any file related exceptions</exception>
		/// <returns>The object that was loaded</returns>
		public static DialogueSettings Load(string filename)
		{
			BinaryFormatter formatter = new BinaryFormatter();

			Stream stream = File.Open(filename, FileMode.Open);
			var settings = (DialogueSettings)formatter.Deserialize(stream);
			settings.fontColor = new Color(settings.r, settings.g, settings.b, settings.a);
			settings.BackgroundOffset = new Vector2(settings.bOffsetX, settings.bOffsetY);
			settings.TextOffset = new Vector2(settings.tOffsetX, settings.tOffsetY);
			settings.NextViewOffset = new Vector2(settings.nOffsetX, settings.nOffsetY);
			stream.Close();

			return settings;

		}
	}
}
