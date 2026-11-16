Project 2
CPSC 386


1. Game Description:

This game is a 2D vertical platformer developed in Unity 6. The player controls a character who must climb platforms while avoiding rising lava and moving cave enemies. The player can move left and right, double jump, and collect crystals to unlock a portal.

Collecting three crystals unlocks the portal and allows the player to complete the main game. After reaching the portal, the player can choose Endless Mode, where platforms, enemies, and crystals continue spawning as the player climbs higher. Crystals collected in Endless Mode earn points, and the highest score is saved.

The game includes a main menu, procedurally generated platforms, a health system, animations, sound effects, particle effects, pause functionality, and persistent high-score tracking.

2. Controls:

- A / D: Move the player left and right
- Spacebar: Jump and double jump
- Escape: Pause or resume gameplay
- Mouse: Navigate menus and adjust audio settings

3. Scenes:

MainMenu: Contains the game title, Start button, Quit button, and Settings menu. The Settings menu includes separate sliders for music and sound effects. The Start button loads the Caves scene.

Caves: Contains the player, starting platforms, rising lava, collectible crystals, moving enemies, and generated platforms. The scene also includes a health display, height counter, crystal counter, saved records, pause menu, Game Over screen, and Win screen. After completing the main objective, the player can continue in Endless Mode.

4. Script Summary:

- PlayerController.cs: Uses the Input System and Rigidbody2D for player movement, jumping, and double jumping.
- PlayerAnimation.cs: Updates the player's Animator based on movement, jumping, and falling.
- JumpStateEffect.cs: Uses StateMachineBehaviour to change the player's color while jumping.
- PlayerAnimationEvents.cs: Uses an Animation Event to trigger a footstep function during the running animation.
- PlayerHealth.cs: Tracks player health, handles enemy damage, and provides temporary protection after being hit.
- CaveEnemy.cs: Moves enemies horizontally and damages the player when touched.
- PlatformGenerator.cs: Generates platforms, crystals, portals, and enemies as the player climbs.
- PlatformColor.cs: Changes the appearance of generated platforms.
- LavaController.cs: Controls the rising lava and its movement during gameplay.
- LavaWave.cs: Adds movement to the lava's appearance.
- LavaWarning.cs: Displays a warning when the lava begins rising.
- PuzzleController.cs: Tracks collected crystals, unlocks the portal, and manages Endless Mode crystal points.
- PuzzleState.cs: Uses a separate C# class to track unique crystal IDs.
- CrystalData.cs: Stores crystal information, including color and point values, using ScriptableObjects.
- CrystalPickup.cs: Handles crystal collection, sound effects, and particle effects.
- PortalExit.cs: Detects when the player enters the unlocked portal.
- GameFlow.cs: Manages the game's win and lose conditions, pause functionality, restarting, and Endless Mode.
- GameOverMessage.cs: Changes the Game Over message depending on whether the player loses to lava or enemies.
- AudioManager.cs: Manages background music, sound effects, and saved audio settings.
- VolumeUI.cs: Connects the music and sound effects sliders to the AudioManager.
- MainMenuUI.cs: Handles Main Menu buttons and Settings menu functionality.
- CameraFollow.cs: Makes the camera follow the player as they climb.

5. Project Requirements
------------------------------------------------------

1. Scenes: MainMenu and Caves are connected through SceneManager.LoadScene().
2. Input System: The player uses a PlayerInput component and Input Actions asset for movement and jumping.
3. 2D Physics: Rigidbody2D and Collider2D components handle movement, platforms, crystals, enemies, and lava interactions.
4. Puzzle State: Players must collect three unique crystals to unlock the portal. A separate PuzzleState class tracks crystal collection.
5. ScriptableObjects: BlueCrystalData and PurpleCrystalData store different crystal colors and point values.
6. Animation: The player uses an Animator Controller with Idle, Run, Jump, and Fall animations, including a Blend Tree.
7. Animation Event and StateMachineBehaviour: An Animation Event calls the footstep function during running, while JumpStateEffect changes the player's color during jumping.
8. Procedural Generation: Platforms are generated using seeded random values to create different heights and horizontal positions. Enemies and crystals also spawn during gameplay.
9. Particle Effects: Collecting crystals activates a particle burst effect.
10. Audio: Background music and sound effects are managed through a persistent AudioManager and AudioMixer, with separate volume controls.
11. PlayerPrefs: The game saves the player's best height, highest Endless Mode crystal score, and audio volume preferences.
12. Health and Game Over: The player has three health points and loses one when touching an enemy. The rising lava causes Game Over, and the displayed message changes depending on the situation.
13. Pause Menu: The player can pause, resume, restart, or return to the Main Menu.
14. Endless Mode: After completing the main objective, players can continue climbing while collecting crystals for additional points.

------------------------------------------------------

6. Procedural Generation and Endless Mode Explanation:

The PlatformGenerator script creates new platforms as the player climbs higher. It uses a random seed along with minimum and maximum spacing values to determine the positions of the platforms. This allows the game to continue generating new areas without having to manually place every platform.

Crystals and cave enemies are also generated at certain platform intervals. Older objects below the player are removed to help prevent unnecessary objects from accumulating.

In the main game, the player must collect three crystals to unlock the portal. After reaching the portal, the player can enter Endless Mode instead of ending the game.

During Endless Mode, the game continues generating platforms, enemies, and crystals. Blue crystals are worth 5 points, while purple crystals are worth 10 points. The player's highest crystal score is saved using PlayerPrefs.

This allows the player to continue climbing and collecting points while trying to avoid the rising lava and cave enemies.

7. Assets, Packages, and References:

The game uses Unity's built-in 2D physics, Input System, Animator, AudioMixer, particle systems, UI components, and rendering tools.

The purple cave enemy sprite was created using Aseprite by me and the other art assets were made by me and my siblings. Other visual elements were created using Unity's built-in tools and materials.

The implementation draws on course lecture examples covering platformers, player movement, puzzle state tracking, ScriptableObjects, procedural generation, animations, StateMachineBehaviour, Animation Events, particle effects, audio management, and PlayerPrefs.

Some assistance from ChatGPT was used for troubleshooting with the lava mechanics and error handling, along with online tutorials from youtube.  

Background music and sound effects were imported from youtube and the sound effects were from an online website. 

https://www.myinstants.com/
https://www.youtube.com/watch?v=dgha9S39Y6M
https://www.youtube.com/watch?v=_CQ_ehoRfsE




8. GitHub Repository:

https://github.com/maslam3-git/MyProj2

9. Build and Testing:

Unity version: 6000.0.83f1 LTS

Build platform: Windows

Testing: The main menu, player movement, double jumping, crystal collection, portal unlocking, moving enemies, and health system have been tested during development. The Animation Event was verified through messages in the Unity Console, and both music and sound effects sliders were tested successfully.

The game also includes saved height and crystal-point records, a pause menu, Win and Game Over screens, and Endless Mode.

Final testing of the standalone Windows build should be completed before submission to confirm that all gameplay features and menu buttons work correctly outside the Unity Editor.