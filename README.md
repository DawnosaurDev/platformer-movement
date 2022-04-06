**IF YOU ARE GETTING MISSING SCRIPT ERRORS, SEE BELOW :D**

# platformer-movement

Everything from the video and now more!
Now Includes: Dashes, Wall Jumps, Slide and State Machine

## Guide to the Scripts
### Player Data

  A scriptable object that contains all the parameters for your player
  eg: maxRunSpeed, jumpForce etc
  
### Input Handler
  Manages all the player's input, with the new Input System. The player scripts, by default, **REQUIRE** the new Input System package (which you can import via the packet manager). 
  **However** if you wish to use the standard Input System just change all the references to the Input Handler (in the player movement script) to the standard input system
eg: InputHandler.instance.MoveInput => Input.GetAxisRaw("Horizontal")

  On the other hand, if you use my Input Handler I would recommend this (old but great) video explaining how the new Input System works: https://youtu.be/Pzd8NhcRzVo
  
### Advanced Player (recommended)

  1) My recommened platformer script
  2) Has all the features shown in the video, with some bonus stuff
  3) Great if you're looking to upgrade your platformer script 

### State Machine Player
  1) The most complex and expandable platformer system
  2) A Finite State Machine is a great architecture if you're planning to work on your game for a long time and increase the complexity
  3) Allows full incredible flexibility
  4) Can require more time to implement and upkeep


### Basic Player (very broken lol)
  1) Ignore for now
  2) Will just be a very standard platformer controller as you may see in a tutorial such as https://youtu.be/QGDeafTx5ug. Videos like this are great for beginners, but hopefully my take on platformer movement can highlight some of the great improvements you can make to create a fluid, precise charater.
  3) Will later be used to show contrast between player types

## About Me :D
**My Discord:** https://discord.gg/XkuKREkdVf Best place to ask me questions or chat :D

**My Twiiter:** https://twitter.com/DawnosaurDev Keep up with my game dev journey and new platformer mechanics!

Thanks so much for checking this out <3

Dawnosaur 
