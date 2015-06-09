# Monologue
This is a project to easily display text or dialogue in a Monogame project in the fashion of an old-school RPGs (text displaying character by character).

### Features
* Lots of settings to modify how the text is displayed. The only required setting is the font sprite, but you can also specify: 
  * font color 
  * background texture
  * texture to be displayed after all the text is shown (hereafter called the next icon)
  * ffsets for text, background, and the next icon
  * text speed
  * variance to text speed
  * blinking of the next icon
  * the key to continue the text
* XML Serilization for loading/saving of the settings

### How to use
First, add the project to your Monogame project and add the reference to the project in your main game. Then, initialize your object (at least with a font) and any settings you want.

Then when you want to have the text displayed you will call the Dialogue's SetSpeech, like so:
```
dialogue.SetSpeech(new string[] 
  { 
		"Lorem ipsum dolor sit amet, consectetur adipiscing elit.", 
		"Aenean viverra magna nec\nipsum tempor convallis.", 
		"Integer ac ante at nisi ultricies molestie in nec lacus" 
	});
```
Each element in the string array will be a new 'screen' to be displayed that the player will have to continue past. Currently, you will have to put your own newline characters in (as I did in the 2nd line), but hopefully will be added to allow it soon.

In your update loop, you will have to call the Dialogue's update method. This will return whether it has displayed text or not so that you can disable any other code that you want while the text is being updated.

In your draw loop, you will have to call the Dialogue's draw method. This method is safe to call even when there isn't any text to be displayed.


### Possible Upcoming Extensions
* Automatically adjusting the text to screen size with newlines and new pages based on the bounds.
* Making input more modular: Allowing multiple keys to continue and gamepads
* Adding support for menus/choices in these dialogues
* Set a settings file when you set text to work as a temporary style
* Update the size when screen resolution changes
* Set background and font after loading (perhaps pushing content loading into the dialogue settings (and we could serialize filenames)
* Add projects for a sample game and tests
* New settings? Maybe option for displaying all the text once or having faster/slower text for specific words
