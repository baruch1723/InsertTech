![Diagram](https://github.com/baruch1723/InsertTech/assets/29302311/4f3887c8-0b31-453d-969b-fc2b70600c5a)
[Demo download](https://drive.google.com/uc?export=download&id=1FJdj3I3IHy4ws23T1FP58C2jUWlC-8eK)
<h1>Components</h1>

**Level Manager**


Description: The Level Manager is responsible for setting up and starting levels. It keeps track of level progress throughout the game.
Functions:
Initializes level settings.
Monitors and updates the progress of the current level.
Coin Factory
Description: The Coin Factory creates coin prefabs used in the game.
Functions:
Generates coin objects.
Manages the properties and behaviors of coins.

**Game Manager**

Description: The Game Manager oversees the overall game flow and state.
Functions:
Controls game states (e.g., start, pause, end).
Coordinates between different components to ensure smooth gameplay.

**Level view controller**

Description: The level view controller manages the display of game information to the player.
Functions:
Updates score, health, and other game stats on the screen.
Provides visual feedback to the player.

**Camera Controller**

Description: The Camera Controller manages the camera behavior within the game.
Functions:
Adjusts camera angles and positions.
Ensures the player has an optimal view of the game environment.

**Coin Collector**

Description: The Coin Collector handles the collection of coins by the player.
Functions:
Detects collisions with coins.
Updates the player's coin count.

**User Controller**

Description: The User Controller manages user inputs and interactions within the game.
Functions:
Processes player inputs.
Controls player movements and actions.
Parachute Controller
Description: The Parachute Controller manages the parachute behavior within the game.
Functions:
Controls the deployment and descent of parachutes.
Ensures smooth landing mechanics.

**Main Menu Manager**

Description: The Main Menu Manager handles the main menu interface of the game.
Functions:
Provides options for starting the game, loading saved data, and adjusting settings.
Navigates between different menu screens.
Pause Manager
Description: The Pause Manager handles the game's pause functionality.
Functions:
Pauses and resumes the game.
Provides options to access settings or return to the main menu during a pause.
