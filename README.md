# LAN FPS
  
#### 3D First person shooter with online multiplayer 
<br/>&nbsp;&nbsp;&nbsp;&nbsp;a [***Lit Lab Production***](https://www.litlabproductions.com)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;Built with [Unity3D](https://github.com/Unity-Technologies) <br>

<br/>&nbsp;&nbsp;&nbsp;&nbsp;**Note:** This application was developed with the help of a Brackey's tutorial found here: <br>
&nbsp;&nbsp;&nbsp;&nbsp; https://www.youtube.com/watch?v=UK57qdq_lak  <br>

***

## Table of contents
- [LAN FPS](#lan-fps)
  - [Development](#development)
    - [Player Movement](#player-movement)
    - [Shooting](#shooting)

***

## Development 

### Player Movement
  * **W**, **A**, **S**, **D** to walk around
  * Move **mouse** to look around<br><br>
![move](https://user-images.githubusercontent.com/34845402/134239996-089da7ed-ad54-4cb5-b538-738040f9bd60.gif)
 <br><br>

  * **Shift** to use the jetpack <br><br>
![jet](https://user-images.githubusercontent.com/34845402/134240408-93de09e6-546f-40f4-9aba-cfd23483f23f.gif)
      * *The jetpack fuel bar can be found on the bottom-left side of the screen*
<br><br><br>

### Shooting 
  * **Left Trigger** to fire <br>
  * When a fire input is received an explosion particle effect will play near the end of the gun barrel <br><br>

  * **Bullet Collision** 
      * Bullets travel via 3D raycast <br><br>
![1](https://user-images.githubusercontent.com/34845402/134242779-54b4e7cf-5cb8-4aa3-b9fd-a0cc5102427c.gif)
      * *If the receiving environment game object containts a **dirt** layer it will cause a particle effect to play at an angle perpendicular to the corresponding game objects surface (As seen above)* <br><br>

  * **Player Health & Respawn** 
      * When a player object is hit by a bullet raycast its health will be reduced
      * If player health falls below **one** they will die, causing the body to disappear, a death animation to play and finally the player to respawn with full health at one of four locations a few seconds later <br><br>
![ezgif com-gif-maker2](https://user-images.githubusercontent.com/34845402/134246613-fa63353c-a9d3-43cd-9de3-e0ba19185de8.gif)
![ezgif com-gif-maker3](https://user-images.githubusercontent.com/34845402/134246621-cb12eb2c-717f-423e-bd97-7af440632061.gif)
![ezgif com-gif-maker4](https://user-images.githubusercontent.com/34845402/134246718-81fc0f99-29e6-4ad4-8947-3333807a2d34.gif)
<br><br><br>

***

<br/>
Thanks for reading!<br/><br/>
 
If you like what you see give this repo  
a star and share it with your friends.

Your support is greatly appreciated!<br/><br/>


[***David Guido***](https://www.litlabproductions.com/resume-view) :rocket:  
[***Lit Lab Productions***](https://www.litlabproductions.com)
<br/><br/>
