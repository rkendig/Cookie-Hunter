# Cookie-Hunter
Ryan Kendig

Computer Game Programming - Final Project

Starting Scene: Main Menu 

## Short description
Cookie Hunter is a 3D platformer inspired by [this](https://www.youtube.com/watch?v=tw7Os-_BJMo) episode of Clueless Gamer by Conan O'Brien. Conan is NOT a gamer and hence comments that he wishes for a simple game involving one simple button, "eat cookie." I took this inspiration and ran with it. In Cookie Hunter, the player is dropped into a forest with one objective, collect and eat as many delicious cookies as possible before time runs out. It is important to eat as many as possible, because when the sun goes down, the cookies that the player did not eat come to life and haunt them. The player must survive the night so that they can continue to munch on delicious cookies in the morning. Every cookie eaten adds to the player’s score, so they should eat as many as they can! 

## Known Bugs and Issues
- Sometimes the cookie AI lags and bugs during phase two. This is most likely due to too many navMeshAgent calculations occuring at once. I took precautions to combat this, but sometimes an agent will get caught on a single calculation and will remain totally unresponsive until it is finished calculating
- The fog that triggers once you enter the water (that gives the blue underwater effect) will sometimes lag and glitch the game slightly.
- It is also possible for the player to jump on vertical surfaces (that in all reality make no sense to be able to jump upon). 
- Using the built in developer shortcut of the ‘z’ key will skip the game to the start of the next phase, but it will not rotate the sun and moon properly and may cause other unforseen bugs.
 
## Video Link:
Here is a [video demonstration](https://youtu.be/R7i6WLlpLeQ)


## Fuller description: 
These are just general features and topics that should be recognized and discussed. They are not presented in any sort of ordered fashion. 

- The game is loaded into a Main Menu. This menu is placed on a replica of the terrain object used in the game and because of this the background is live, animated, and realistic. Here, the player can either start up the game, quit, or seek help, which gives them information about the objective of the game, how it works, and the controls. 
- The game is structured to contain two phases signified by a time limit and a day/night cycle. This cycle was tricky to figure out, and involved rotating the two light objects (sun and moon) around the terrain respective to the time left. Important calculations had to be run to turn on and off their light components at precise times. 
- Creating the terrain of the game was time consuming and meticulous. I wanted my game to look good, so I spend a lot of time fixing lighting, creating trees, configuring water effects, and the like. Though the terrain is pregenerated and remains consistent through each game play, the rest of the game (the cookies) are not.  
	- The terrain boasts features such as hills and mountains, 2 types of water, 2 types of flowers, 6 types of trees, a wind zone, 3 types of grass, 2 types of logs, and a sun & moon 
	- The detailed water was tricky and getting light reflections and wave effects correct was very interesting but I mostly went with the default settings.
	- I added a fog effect to make entering the water more realistic. The proper triggers, colliders, and scripts took me a while to figure out. There is a water that does not have a fog effect (like you are under water). This water is left the way it is, because I wanted the effect of having a false water that upon investigation revealed a trove of cookies.  
- Controlling the movement of the player and the camera was important to me. I wanted it to be fluid and true to the first person RPG feel. The general implementation was largely taken from the source I cited, but I ended up taking out the ability to zoom in and out in order to not bug and clip with my terrain.
- I created my own prefab cookie objects (5 of them), complete with colliders, rigid bodys, scripts, and triggers. Cookie prefabs have a small box collider and rigid body so they will fall onto and lay on the ground. They then have a larger capsule collider that acts as a trigger for the player to interact with and pick them up. It was VERY tough and took me a long time to figure out what combination of colliders, rigidbodies, triggers, and character controllers would allow me to have dynamic interactable objects. I had to find a method to notify the game manager that the player was standing over a cookie or next to an interactable tree and that if they hit "f" they would interact. It had to do this in a optimal way as well, as having a hundred simultaneous "OnTriggerEnter" and OnCollision... method calls was severely lowering my ramerate. Finally I figure out a way to have minimal trigger and collision method calls by passing around a GameObject to my Player script that had details about any interactable object the player was next to.
- The cookies are spawned randomly according to certain percents/proportions. There are also a certain proportion of cookies in trees that the player can find. Placing cookies in trees and bushes was very hard to figure out. Finally, I figure out a way to grab random tree locations and scale and translate them to coordinates I could use. Then, I had the problem of getting the interactable trees and if the player was near one, prompt the player to search for a cookie. In the end, I was able to attach triggers to each tree location to create interactable trees and bushes. Also, getting the prefabs to shoot out in a direction that was guaranteed to be visible to the player was initially hard but using coordinates relative to the player's transform made it trivial. 
- The player is spawned in one of 4 random spawn locations
- There is a fully integrated Main Menu and Pause Menu ● 
- ‘z’ will short-cut to the next phase of the game
- Cookie chasing AI during phase 2 was VERY TRICKY as I had a LOT of cookie prefabs that I wanted to make into navAgents. I used a baked NavMesh to control them but even baking the navmesh with the right settings was difficult as the process was completely new to me. I wanted the NavmeshAgents to behave in certain unique ways and having all of them calculating shortestPaths to the player not only slowed down the game immensely, but it also rendered them useless as chasers. They would just sit there while they calculated how to get to the player. It was no fun. At this time, trying to figure out my bugs and how to optimize their behavior was time consuming. I knew I didn’t want them to look for a new path every frame, but the physic update() and late update() were not working either. Eventually, NATHAN LI gave me a brilliant idea (that I ended up using). The cookie chasers that the player cannot see just do nothing. NavMeshAgent behavior is culled at a certain distance. This is simple in concept, but was vital to optimizing my game and having it run smoothly.
- Patrolling was very tricky. Configuring and optimizing the nav mesh and nav agent behavior was key. I had to figure out where and when I wanted to call SetPath, as it was causing very bad latency issues not with the fps but with the behavior of chasers. To patrol, Navmesh agents pick a random point on the terrain and move there, lest they are ‘aggroed’ by the player.
- There are 4 different types of cookies, and each have a different type of chasing behaviors. 
	- Brown Cookies patrol until the player gets in range at which they chase at the same speed of the player 
	- White Cookies are thesame as Brown, but they have a short range for aggro and are slightly faster than the brown. It is also possible to break their aggro on the player if the player gets far enough away. 
	- Violet Cookies are hooked onto the player from the get-go and add intensity to the game as they will constantly chase the player no matter what. 
	- Green Cookies lay on the ground and hide until the player gets close to them. At this point, they jump out and try to catch the player by surprise. They are tricky, and the are fast. It is also possible to break their aggro on the player if the player gets far enough away.  
 
 ## Relevant Files
Assets is the only real directory with relevant code
 - "Scenes" contains the starting Unity Scene -> Main Menu
 - "Scipts" contains all the C# scripts that manipulate and control the game objects of the scene. Without them, there would be no game. 
 - Everything else in Assets is mostly prefabs, textures, models, and the like.
 
 ## External resources 
- The Unity Manual  
	- Unity API Manual used for all sorts of things, including data structures like Dictionaries, Arrays, Quaternions, and Vector3's, as well as getting IO/Keystrokes, attaching components to gameobjects, Terrain objects, Navmeshs, Navmesh Agents, Main Menu implementation, Lighting, and tons more 
- Water generation and implementation [guide](https://answers.unity.com/questions/442734/how-do-i-make-the-area-unde%20r-my-water-a-plane-look.html)
- How to utilize a Wind Zone [guide](https://www.youtube.com/watch?v=Lz8wEsvCWMs) 
- Patrol AI [guide](https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/) particularly the post by user Cnc96  - How to create menus [guide](https://www.youtube.com/watch?v=zc8ac_qUXQY)
- Asset Store
	- Standard Assets Package 
		- Water, Trees, Textures, SpeedTrees, and Fonts 
	- TextMeshPro package
	- LowPoly Terrain Pack 1 (not sure I ended up using this) 
	- Low Poly Environment (not sure I used this either)
	- NatureStarterKit 2
		- Trees, Bushes, Textures, Skybox, and Materials 
