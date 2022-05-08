# Silent Realm - The Midnight Society

Start scene: Scenes/Title.unity

---

# Gameplay and Controls

## Controls

Player Controls
- \[Arrow Keys/WASD\]: Move and rotate player.
- \[Spacebar\]: Roll (must be moving forward first). Can also be used for the paraglider if off ground.
- \[Hold Shift\]: Run.
- \[X\]: Collect potion from a pot.
- \[G\]: Use paraglider (if high enough above ground).
- \[O\]: Toggle cutscenes on/off (mostly for development).
- \[ESC\]: Open the pause menu.

Analog controls are also supported
- \[Left Stick/WASD\]: Move forward and rotate player.
- \[Left Button\]: Roll (must be moving forward first).
- \[Hold Bottom Button\]: Run.
- \[Top Button\]: Collect potion from a pot.
- \[Start (Options for PS)\]: Open the pause menu.

Title Screen Buttons
- Start Game: Begin the level.
- Credits: Show the team and resources used for the project.
- Controls: Show the controls for the game (for PC).
- Exit: Quit game.

## Gameplay

Goal: Collect all potions from the pots and return to the starting circle to win.

Details
- The player begins in the Starting Circle.
- Walk out of the circle to start the game. The strong enemies activate and chase after you (see Chase State below).
- Run up to a glowing potion pot to collect it ('X'). There is a 'Potions' label on the HUD to indicate your progress.
- There is a time indicator in the top left. If it runs out, the Chase is activated. Collecting a potion restores the time.
- Once all potions are collected, you must return to the starting circle to win.
- When returning to the Starting Circle, time will be stopped, but running into traps will reactivate the enemies!

Chase State
- When active, a cutscene plays and the strong enemies awaken and chase the Player.
- It activates when you first leave the Starting circle, the time runs out, or you run into a patrolling guard light or trap.
- Deactivate it by collecting a potion or winning the game.

Enemies (activate during the Chase state)
- Golem: large rock enemy that runs after you on the ground. Uses NavMesh and attacks when Player is nearby.
- Ghost: Ignores gravity and walls to fly towards you. Swings sword at Player.
- Both of these one-shot the player. They move back to their original positions when you deactivate the Chase.

Traps and Obstacles
- Patrolling guard: follows a set path and initates the Chase state if the Player runs into its spotlight.
- Pink Mist: activates the Chase state if the player comes into contact with it.
- Timer: if the timer hits zero, the Chase state is initiated

---

# Manifest

## Nate Harper
- Animations and sounds for Golem
- Animations and sounds for Ghost
- The long hall/room
- The "Banquet Hall"
- The "Torture Chamber"
- The box, barrel, chair and table rigid body objects
- DualShock controller support
- Enemy AI
- Light patrols

Scripts
- LightController.cs - Controls when the lights turn off and on in the scene
- GolemEnemyController.cs - AI for the Golems
- PatrollingLightTrigger.cs - Raycasts for the patrolling light enemy to trigger a chase
- PatrollingEnemyController.cs - AI for the patrolling light enemy
- GhostEnemyController.cs - AI for the ghost enemy
- GolemStepEmitter.cs - Sounds for the golem when he walks
- ResetPosition.cs - Resets the objects positions when chase state ends

## Anh Ngyuen
- Implement Title scene and pause menu.
- Implement witch NPC and idle animation.
- Added initial idle, chase, title, audio.
- Added RigidBody objects throughout the dungeon.
- Decorated multiple rooms in the dungeon.
- Created prison and blacksmith rooms.
- Recorded gameplay videos.

Scripts
- AudioManager.cs: add method for changing the audio clip for the background music.
- CreditButtonScript.cs: show credits text on title screen after button click.
- PauseMenuScript.cs: listen for key press and toggle Canvas for pause menu.

## Kasie Okpala
- Set up Cinemachine and 3rd-person virtual camera to follow the player.
- Create reusable moving platform. Player can optionally walk and stand on top.
- Created initial moving platform room and elevator.

Scripts
- MovePlatform.cs: multi-use script for moving object between waypoints. Supports speed, a trigger for detecting objects on top, reverse path, rest time, etc.

## Darik Harter
- Initial main character movement and animation.
- Potion pot particles, collection, sound, and button indicator.
- Create dungeon starting area.
- Add game music (title screen, idle, chase game over).
- Add sound effects (death, scoop).
- Create bridge room.
- Add game over screen.
- Switch to new input system.
- Implement shorter version of cutscene.
- Add final potion collected cutscene.
- Improve title screen (new icons and credits panel).

Scripts
- PotScoop.cs: collect potion, play sound, show button indicator when player in range.
- Collectable.cs: increment score on trigger enter with player.
- GameManager.cs: create Singleton class to store global score, timer, etc.
- AudioManager.cs: add event callbacks for changing music based on enemy active state.
- UIController.cs: toggle pot scoop indicator on HUD.
- StartingCircle.cs: start enemy chase when player first exits, handle win condition.

## Darin Harter
- Create initial UI display of time remaining and potions collected.
- Main character physics and root motion animation (running, rolling).
- Created ragdoll from player model. Added ability to toggle death state for player.
- Help create dungeon starting area.
- Golem and ghost sword hitbox and collision with player on attack.
- Win game "cutscene" with music and player auto-moving towards center of circle.
- Player support for moving platforms.
- Paraglider!
- Library and moving platform rooms.
- Player dissolve shader.
- Improved menu functionality.
- Added RigidBody doors with hinges.

Scripts
- PlayerController.cs: handle CharacterController movement, gravity, and ragdoll on death. Set animator parameters for root motion.
- UIController.cs: listen for GameEvents and update text and progress bar.
- GameManager.cs: set up event system for handling game state.
- FootstepEmitter.cs: play a random footstep sound clip. Used as animator event callback for Player.
- PlayerCollisionReporter.cs: report when player collides with enemy hit box. Provide contact point of hit.

---

# Game Requirements

## It must be a 3D Game Feel game!

Clearly defined, achievable, objective/goal?
- Win the game by collecting all pots and returning to the starting circle.
- Lose the game by getting hit by a Golem or Ghost. The level restarts.

Communication of success or failure to the player.
- Cutscene plays with victory music on win.
- Player ragdolls and sad music is played on player death.

Start menu to support Starting Action?
- 'Start Game' button on title screen transitions to the game.

Able to reset and replay on success or failure.
- Level automatically restarts after failure. Winning brings you back to the main menu, where you can start a new game.

## Precursors to Fun Gameplay

Goals/subgoals effectively communicated to player
- Much of the gameplay can be learned by trying and failing. The player will quickly learn to avoid enemies and collect pots to reset time.
- We also have a controls screen that allows the user to see the different actions they can do.

Your game must provide interesting choices to the player
- The player must decide on an optimal path to collect potions to not get stuck in the enemy chase state with no nearby pot to assist.

Your player choices must have consequences (dilemmas)
- Choosing the wrong path to collect potions or avoid enemies can lead to being trapped and hit, restarting the level.

Player choices engage the player with the game world and inhabitants (AI-controlled agents)
- Enemies find the optimal path to reach you using NavMesh (golem) or vector math (ghost).
- Player can walk on moving platforms to reach new areas.
- The scene lighting and threats change depending on the Chase state.

Avoid Fun Killers (micromanagement, stagnation, insurmountable obstacles, arbitrary events, etc.)
- The player is always on the move and enemies are intelligent by beatable.
- Gameplay mechanics and events are consistent.

Your game should achieve balance (balance of resources, strategies, etc., as appropriate)
- Time management and can dictate player success and affect the challenge of the game.

Reward successes, and (threat of) punishment for failure
- Collecting potions restarts time and gives you more freedom to seek the next pot.
- Running out of time or colliding with a trap or patrolling light punishes you by starting the Chase state.

In-game learning/training opportunities (e.g. gate restricts player to safe area until basic proficiency in new skill is demonstrated, etc.)
- The level design was done in a way to allow the easier rooms to be accesses first and the more difficult rooms to be further away from the start. It was also designed as such to give the player a change to experience each trigger in a relatively "safe" environment before having to meet more difficult challenges.
- In general, the player learns by trying and failing. The game may be easier as the player learns the dungeon and paths.

Appropriate progression of difficulty
- The first pot is located rihgt by the starting circle.
- The level becomes progressively more challenging as you collect pots (must locate the next from a lower pool of pots).

## 3D Character/Vehicle with Real-Time Control
Character control is a predominant part of gameplay and not simply a way to traverse between non-“Game Feel” interactions.
- Player is almost always active and controllable besides during cutscenes or death.

Your character is not a complete prepackaged asset from a 3rd party
It is ok to use models and animations from another source, but your team should configure animation control and input processing
- We programmed the Character Controller and created an animator from scratch for the player.
- Used a free 3D model and root motion animations from Mixamo and a Basic Motions package.

Utilize a character/vehicle controlled by the player with engaging animations that react to the player’s inputs.
- Our player uses a Humanoid avatar with a blend tree for forward and backward movement. There are also actions for Rolling and Idle.
- Pressing Shift increases the VelZ animator parameter and makes the player run.

Player has Game Feel style control: continuous, dynamic, low latency, variable/analog-style control of character movement majority of time.
Fluid animation. Continuity of motion (no instantaneous pose changes, no glitching, unintended teleporting, a ground-based character stuck floating in air, etc.
Aim for low latency responsiveness to control inputs
- Both keyboard and analog controls are supported
- Modifier-button (Shift) for increasing run speed.
- Responsive transitions between idle, walking, running, and rolling.

Humanoid characters should not slide or moonwalk. Use root motion, possibly with IK corrections!
- The Player uses root motion for walking, running, and rolling. Foot IK is turned on.
- The Golems also use root motion and do not slide

Camera is smooth, always shows player and obstacles in a useful way, automated following of character or intuitively controllable as appropriate.
Camera has limited passing through walls, near clipping plane cutting through objects (e.g. player model), etc.
- Camera moves forward when an obstacle is in between it and the Player.
- Good field of view for seeing the level, enemies, and potion pots.
- It is a 3rd-Person follow camera using Cinemachine.

Auditory feedback on character state and actions (e.g. footsteps, landing, collisions, tire squeal, engine revs, etc.)
- Sounds are emitted on every Player and Golem footstep.
- The Golem's get progressively louder as they approach the player
- The Ghost's emit a "spooky sigh" as they get close to the player.

Coupling with physics simulation via animation curves, animation callback events, handoff to ragdoll simulation, IK adjustments to animations, vehicles that break up in wrecks, etc.
- Player goes into ragdoll state when hit by a Golem or Ghost.
- Animation callback events for footsteps and enemy attack animations.

## 3D World with Physics and Spatial Simulation
You have synthesized a new environment rather than just using a downloaded level from the asset store.
- We built our own dungeon level from scratch using the Prefabs from our Low Poly Dungeon pack.

Both graphically and auditorily represented physical interactions
Consider every single cause and effect interaction and make sure there is associated audio
- Sounds played on enemy sword swings, footsteps, potion collected, Chase state changes, etc.
- Particle effects for hits, potion pots, torches.
- Cutscenes have audio and music.

Graphics aligned with physics representation (e.g. minimal clipping through objects)
- Player has limited clipping through objects.
- The Golem enemy follow the NavMesh path and have minimal clipping.
- The Ghosts were specifically designed to be able to go through objects

Appropriate boundaries to confine player to playable space.
- Player cannot exit the dungeon normally. Confined by walls, floors, and ceilings.

Interactive scripted objects (buttons that open doors, pressure plates, jump pads, computer terminals, etc.)
- Pink mist obstacle detects player collision and starts enemy chase state
- When the player walks through spotlights, the chase action is triggered
- Potion pots bring up "Press X" when nearby and increment pots collected on button press.

3D simulation with six degrees of freedom movement of rigid bodies
Simulated Newtonian physics rigid body objects (crates, boulders, detritus, etc.)
- Basic rigidbody objects like barrels and chairs movable by player.
- Several rigid body objects are scattered throughout the dungeon, more for visual/psychological effect rather than game play interaction

Animated objects using Mecanim (can be used for non-humanoid animations too!), programmatic pose changes, kinematic rigid body settings, etc.
- Player, Ghost, and Golem use Mecanim for movement and attack animations.
- Pot "Press X" indicator programatically appears when Player is nearby.

State changing or destroyable objects (glass pane that shatters, boulder that breaks into bits, bomb that blows up, etc.)
- Movement and attacks are controlled by simple state machines for Ghost and Golem.

Interactive environment
- Potion pots, exitting and entering Starting Circle at start and end of game trigger events, moving platform starts when player is on top.

## Real-time NPC Steering Behaviors / Artificial Intelligence
It is ok to use models and animations from another source, but your team should configure animation control and AI steering and decision making
- We used Golem and Ghost models from the Unity Asset Store, as well as obstacle models from objects that came in the LowPoly pack.
- We made our own animation controllers and configured a Humanoid avatar for the Golem to work with Mixamo animations (sword swing).

Multiple AI states of behavior (e.g. idle, patrol, pathfinding, maniacal laughter, attack-melee, attack-ranged, retreat, reload, etc.)
- Enemies have a statue state (Chase not active) where they do not chase the player and stand still.
- Cutscene state for coming alive and starting to move.
- While active, they switch between running after the player and performing melee attacks.

Smooth steering/locomotion of NPCs.
- Golem mixes NavMesh with root motion to find the optimal path while avoiding foot sliding.
- Ghost constantly steers towards the Player. We may add predictive targeting based on Player velocity later.

Predominantly root motion for humanoid characters.
- Golem uses root motion for walking/running.

Reasonably effective and believable AI?
Perceived as a fair opponent? You may need to implement a perceptual model for your AI.
Difficulty of the player engaging the enemy appropriate?
- Enemies are reasonably intelligent with tracking and attacking the Player.
- The game can be won with careful evasion by the Player.

Sensory feedback of AI state? (e.g. animation, facial expression, dialog/sounds, and/or thought bubbles, etc., identify passive or aggressive AI)
- Enemy footsteps play as it runs towards you. These get louder with close proximity to the Player.
- Sword slashing sounds can be heard when it is attacking.
- When Chase is not active, they stand as statues to identify them as non-threats.
- During Chase, the eyes glow and their animations indicate movement and aggression.

AI interacts with and takes advantage of the environment? (presses buttons, takes cover from attack, collects resources, knocks over rigid body objects, etc.)
- The Golem enemies use NavMesh to avoid obstacles and walls.
- Golems may interact with rigidbody objects (barrels, crates, etc), mainly by crashing through them.

## Polish
Overall UI
- We have a Title scene with action buttons.
- In-game Credits and Asset licensing is still a work in progress.
- We have a pause menu (press ESC) with an Exit Game button. The world stops and BG music gets quieter while paused.
- We fade in and out between start menu and game scenes.
- Many decorations scattered throughout the scenes
- Particle effects added when enemies teleport back to state
- Although bugs may be present, all known bugs were removed.

Environment Acknowledges Player
- Pot "Press X" indicator shows when Player is nearby
- Moving platform starts moving when the Player is on top (depending on the setting/type of platform).
- Particle effects for potion pots, torches (blow out during Chase state), end-scene teleporter, etc.
- Sounds for most observable events (besides player rolling, moving platforms, other small details)
- Rigid body objects are influenced by player
- Spotlights detect the player

---

# Unity Packages

- Cinemachine
- NavMesh
- Input System
- TextMeshPro

# Asset Credits

We created our own Animator Controllers for every asset, but utilized many premade animations.

Low Poly Dungeons
- Used for main dungeon environment and props. Built our own dungeon using these pieces.
https://assetstore.unity.com/packages/3d/environments/dungeons/low-poly-dungeons-176350

Animated Hero Character
- Our main character.
https://poly.pizza/m/DgOCW9ZCRJ

Basic Motions Free
- Walk, run, and sprint root motion animations for our main character.
https://assetstore.unity.com/packages/3d/animations/basic-motions-free-154271

Stylized LowPoly Golem Pack
- Our main ground enemy.
https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/stylized-lowpoly-golem-pack-163000

Medieval Fantasy - Ghosts
- Our main flying enemy.
https://assetstore.unity.com/packages/3d/characters/creatures/medieval-fantasy-ghosts-168629

NPC Character Models
https://quaternius.com/packs/ultimatedanimatedcharacter.html

Mixamo animations
- Various root motion animations for rolling, Golem taunting, etc.

Cartoon Particle FX
- Enemy hit indicators, light beam portal, other particle effects.
https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-free-109565

Reggae One font
https://fonts.google.com/specimen/Reggae+One

Lowpoly characters
https://quaternius.com/packs/ultimatedanimatedcharacter.html

Paraglider Model
https://www.cgtrader.com/free-3d-models/aircraft/historic-aircraft/botw-style-paraglider

Dissolve Shader
https://assetstore.unity.com/packages/vfx/shaders/simple-dissolve-shader-123865

## Sounds

Factory Ambiance (background music)
- Quiet music that plays in-game when enemy chase is not active.
https://opengameart.org/content/factory-ambiance

Juhani Junkala - Boss Battle Music (background music)
- Upbeat music that plays in-game when enemy chase is active.
https://opengameart.org/content/boss-battle-music

Steeps of Destiny (background music)
- Title screen music.
https://opengameart.org/content/steeps-of-destiny

OpenGameArt - Win Music 2
- Played when you return to the starting circle after collecting all pots.
https://opengameart.org/content/win-music-2

OpenGameArt - sad theme 2
- Played on results screen after winning.
https://opengameart.org/content/party-and-gameover-loop

Footstep sound clips
- Emitted during player footsteps.
https://opengameart.org/content/footsteps-leather-cloth-armor

RPG Sound Pack
- Bubble sounds for potion scoop
https://opengameart.org/content/rpg-sound-pack

Clank Sound
- Sound when golem walks
https://soundbible.com/tags-clank.html

Sword Swing Sounds and Ghost Sound from
https://freesound.org/people/XxChr0nosxX/sounds/268227/
https://freesound.org/people/HorrorAudio/sounds/431981/

Death sounds
https://opengameart.org/content/male-gruntyelling-sounds

Game over
https://opengameart.org/content/game-over-theme

Transition to chase state sound
https://opengameart.org/content/metal-hit

Enemy activation sound
https://opengameart.org/content/bad-sound-1

Start button sound
https://opengameart.org/content/gui-sound-effects

Torch blow out sound
https://opengameart.org/content/torch-fire-spell

# Code References

Messenger.cs utility class for broadcasting GameEvents and setting up listeners.
- Used for informing components of certain events like game won, potion collected, enemy chase activated, etc.
Credit to Magnus Wolffelt and Julie Iaccarino. Found in Unity in Action book.

Unity3D Ragdoll Physics - Jason Weimann
- Inspiration for CopyTransform method for transferring position and rotation of animated model to ragdoll model.
https://www.youtube.com/watch?v=kux48zqUQY8&t=547s

Third Person Movement YouTube Tutorial
- Helped with getting the main character to move using a CharacterController.
https://www.youtube.com/watch?v=4HpC--2iowE&t=1036s
