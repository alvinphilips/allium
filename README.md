### AudioManager

Our Audio Manager is based off the approach shared in class, with a Singleton that manages Audio Sources for the Music and SFX mixer channels, and stores required audio information in *Sound Objects*.

Sound Objects store information regarding the underlying Audio Clip and details such as volume, pitch and whether the clip should be looped or not. The AudioMixer is loaded and referenced as a Resource.

### OptionsManager

The Options Manager handles player preferences, including audio settings, resolution and quality. Settings are stored via PlayerPrefs and loaded to allow for a player's settings to persist through game sessions.



### Asset Management

The background music (Phil Coulter's performance of Eric Bogle's *The Band Played Waltzing Mathilda*) is loaded as an Addressable when the GameManager first loads. This allows for easy asynchronous loading and prevents noticeable load times.