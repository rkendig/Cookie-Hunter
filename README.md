# Cookie-Hunter
Ryan Kendig, 114082367
Final Project installment 1.0


Starting Scene: Scene 1

Required elements: 

Additional elements: 
	
Known issues: 
	- There are certain spots where it may be possible to break out of the map. Namely the north West corner
	- AI pathfinding can lag
	- Sometimes Canvas sticks around for somereason
	- Player will not respect slopes and slide down them properly

External resources:
	- The Unity Manual
		-API for Dictionaries, Arrays, Vector3's, getting Keystrokes, attatching components to gameobjects, 
		using math.lerp, and countless small implementation details. Trees, Water, Terrain, Character controllers  
 	- Handout
		- Jumping tips implementation
		- Animation speeds and transitions
	- Youtube tutorial on implementing random forest generation 
	- Water generation - https://answers.unity.com/questions/442734/how-do-i-make-the-area-under-my-water-a-plane-look.html 
	- How to utilize a Wind Zone https://www.youtube.com/watch?v=Lz8wEsvCWMs
	- Asset Store
		- standard Assets
	- Patrol AI https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/ particularly the post by Cnc96

Challenges faced/Lessons Learned: I learned alot about how to use Mixamo models and attach animations to them. It took me a while
	to get the correct Rig set-up and animation avatars, but through trial and error I got it. Lerping the granny was difficult
	and getting her to face the right direction (but slowly) was tricky. I also learned alot about colliders and triggers 
	because I used them in several places.



What I have done:
	- When I first began I was using a large tile as the ground for my game and all the environmental objects like
	trees were prefabs. I quickly realized trying to lay each thing into the scene one by one was not gonna fly. Thus, I 
	changed my approach to utilize a "terrain" object. Then I ran into problems with painting trees because the tree
	prefabs I wanted to use would not work. Finally, after I was able to exchange almost all my assets for different ones 
	that would work with my terrain object, I was able to quickly and efficiently create a random map. 
		- This map is not completely random however, as some items are manually placed
	- I added invisible walls as well as visual mountains that border and bound the game area/forest.
	- I added verticality in the form of hills and mountains
	- I added 3 different types of grass, 3 ground textures, 2 flowers, 6 types of trees, and 2 different types of water
	- I did not need 2 types of water, but I was exploring the capabilities of more advanced implementations of water
	and ended up creating two ponds. One of the ponds has an underwater effect, the other one doesn't. This is not a oversight
	it is a choice, as the one without the underwater effect is simply a trick pond, meant to hide things. 
	- Although most of the code to generate realistic fluid water I found off a internet post, It was still tricky to implment
	in the form I needed. I had to figure out how to utilize propper triggers, where to put scripts, and other minor details
	that I'm sure purely were unique to how I wanted to use the water and where I wanted to put it. 
	- Controling the movement of the player and the camera was tricky as well. One problem had to do with ther terrain 
	object being opaque on one side but see-though on the other. The character could glitch the camera by zooming out and
	 panning upward in which case they would see the underside on the terrain. In the end, I took the zoom function out. 
	- The camera will not pan with the right-click on the mouse + movement.
	- The player can be moved by using the aswd controls.
	- There is a wind zone set to very specific setting to provide a slight breeze to trees
	- Grass wind was done via the Terrain game Object
	- Cookie prefabs have a small box collider and rigid body so they will fall onto and lay on the ground. They then have a
		larger capsule collider that acts as a trigger for the player to interact with and pick them up. 
	- There is a Game Controller object and associated script that will spawn the pickups based on some given parameters.
	-It was VERY tough to figure out what combination of colliders, rigidbodies, triggers, and character controllers would 	
	allow me to have dynamic interable objects. I had to find a method to notify the game manager that the player was standing
	over a cookie or next to an interable tree and that if they hit "f" they would interact. It had to do this in a optimal
	way as well, as having a hundred simoultaneous "OnTriggerEnter" and OnCollision... method calls was severely lowering my
	framerate (and tbh fps is still kinda low) but finally I figure out a way to have minimal trigger and collison method calls
 	by passing around a GameObject to my Player script that had details about any interable object the player was next to.
	- Placing cookies in trees and bushes was very hard to figure out. Finally, I figure out a way to grab random tree locations
		and scale and translate them to coordniates I could use. The KEY line was: 
		treePos = Vector3.Scale(terrainData.GetTreeInstance(Random.Range(0, terrainData.treeInstances.Length - 1)).position, terrainData.size) + terrain.transform.position;
	- I was able to attach triggers to tree to create interable trees and bushes. Getting the prefabs to shoot out in a direction
	that was guarenteed to be visible to the player was initally hard but using coordinates relative to the player's transform
	helped out a lot. 
	-There is a secret hidden cookie
	- 3 different spawn locations
-	-Patrolling was Very Hard. configuring and optimizing the nav mesh and nav agent behavior was key. I had to figure out where 
	and when I wanted to call SetPath, as it was causing very bad latency issues not with the fps but with the behavior of chasers.
	chasers wouldnt patrol, they wouldnt chase, sometimes they wouln't even move. Debuggin the process was very stressful. Finally,
	I devised the idea of using precomputed paths for patrolling but that didn't work either. In the end I just used very small patrol
	radius as well as I cut down on the number of obstacles in my terrain to make path finding easier. 
	-To do: esc UI, Main Menu
