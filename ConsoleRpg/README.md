# Console RPG Final Project Template

Welcome to your **semester-long RPG project**! This template will grow with you as you apply everything you’ve learned in class.

## 🗺️ Overview

This repository is structured to support your growth from basic file-based input/output to a full-blown EF Core RPG with interface-driven architecture.

---

## 📂 Project Structure

```
ConsoleRpgFinalProject/ 
├── ConsoleRpg/              # UI and game loop logic 
│   ├── Helpers/             # Console-based UI helpers 
│   ├── Services/            # Console logic helpers 
│   ├── Decorators/          # Decorator pattern implementations 
│   ├── Program.cs           # Entry point (manual dependency wiring) 
│   └── Startup.cs           # (For later: Dependency injection setup) 
│ ├── ConsoleRpgEntities/      # Models, Interfaces, and Data Access 
│   ├── Data/                # GameContext, PlayerDao, and interfaces 
│   ├── Models/              # Player and other entity classes 
│   └── Interfaces/          # Contracts for data access and services 
│ ├── .github/workflows/       # GitHub Classroom autograding 
└── README.md                # This file
```

---

## 🧩 Core File Responsibilities

### ConsoleRpg Project

- **Program.cs**
  - The main entry point for the application.
  - Manually creates and wires up all dependencies (no DI container yet).
  - Instantiates the game context, output manager, menu manager, player DAO, player service, and wraps the player service with the auto-save decorator.
  - Passes all dependencies to `GameEngine` and starts the game loop.

- **GameEngine.cs**
  - Orchestrates the main game flow and user interaction.
  - Handles the main menu, game loop, and delegates business logic to services.
  - Does not perform data access or persistence directly.

- **Helpers/MenuManager.cs**
  - Manages the main menu and user selection logic.
  - Uses `OutputManager` for all user prompts and menu displays.

- **Helpers/OutputManager.cs**
  - Handles all console output and input.
  - Provides methods for logging, displaying tables (like player lists), and prompting for user input.
  - Uses Spectre.Console for rich, consistent console UI.

- **Services/PlayerService.cs**
  - Contains business logic for player operations (e.g., leveling up, adding players).
  - Logs actions using `OutputManager`.
  - Delegates data access to `IPlayerDao`.
  - Does not handle persistence directly.

- **Services/OutputService.cs**
  - Simple wrapper to allow writing output via the `OutputManager`.
  - Implements `IOutputService` for abstraction.

- **Decorators/AutoSavePlayerServiceDecorator.cs**
  - Implements the decorator pattern for `IPlayerService`.
  - Adds automatic persistence (calls `SaveChanges` on the context) after mutating operations.
  - Wraps the real `PlayerService` to separate business logic from persistence.

---

### ConsoleRpgEntities Project

- **Data/GameContext.cs**
  - Implements `IContext`.
  - Handles all data storage and persistence (currently file-based via CSV).
  - Loads and saves player data, but does not contain business logic.

- **Data/PlayerDao.cs**
  - Implements `IPlayerDao`.
  - Provides data access methods for `Player` entities (add, get, list).
  - Uses `GameContext` as the backend.

- **Models/Player.cs**
  - Represents the player entity.
  - Pure data model: contains only properties and a `ToString` for display.
  - No business logic or persistence code.

- **Data/IPlayerDao.cs, Data/IContext.cs, etc.**
  - Define interfaces for data access and context operations.
  - Support SOLID principles and make the codebase extensible and testable.

---

## 🚀 Getting Started

> Begin by modifying `Program.cs` to add your own menu options and game logic.
> As you implement SOLID principles, add interfaces to `/Interfaces` and apply them in your services and models.

- Start with file-based data
- Later, switch to database-backed storage via `GameContext`
- Use interfaces (`IPlayer`, `IMonster`, `IAbility`) to support clean architecture

---

## 🎓 Final Project Requirements

﻿### FINAL PRESENTATION - ConsoleRPG EF Core Application

#### Basic Required functionality:
- **Add a new Character to the database**
  - Prompt the user to enter details for your character (e.g. Name, Health, Attack, and Defense).
  - Save the updated record to the database.
- **Edit an existing Character**
  - Allow users to update attributes like Health, Attack, and Defense.
  - Save the updated record to the database.
- **Display all Characters**
  - Include any relevant details to your character
- **Search for a specific Character by name**
  - Perform a **case-insensitive** search.
  - Display detailed information about the Character, as above.
- **Logging** (should already be in place)
  - Log all user interactions, such as adding, editing, or displaying data.

#### **"C" Level (405/500 points):**
1. **Include all necessary required features.**
2. **Add Abilities to a Character**
   - Allow users to add Abilities to existing Characters.
   - Prompt for related Ability details (for example, Name, Attack Bonus, Defense Bonus, etc).
   - Associate the Ability with the Character and save it to the database.
   - Output the results of the add to confirm back to the user.
3. **Display Character Abilities**
   - For a selected Character, display all their Abilities.  
   - Include the added properties from their abilities in the output (example, as above, Name, Attack Bonus, Defense Bonus, etc).
4. **Execute an ability during an attack**
   - When attacking ensure the ability is executed and displays the appropriate output.

#### **"B" Level (445/500 points):**
1. **Include all required and "C" level features.**
2. **Add new Room**  
   - Prompt the user to enter a Room name, Description, and other needed properties
   - Optionally add a character, player, etc, to that room.
   - Save the Room to the database.
   - Output the results of the add to confirm back to the user.
3. **Display details of a Room**  
   - Display all associated properties of the room.
   - Include a list of any inhabitants in the Room.  
   - Handle cases where the Room has no Characters gracefully.
4. **Navigate the Rooms**
   - Allow the character to navigate through the rooms and display room details upon entering.
      - Room details may include, for example, name, description, inhabitants, special features, etc.
   - ***Note it is not necessary to display a map as provided during the midterm.***

#### **"A" Level (475/500 points):**
1. **Include all required, "C" and "B" level features.**
2. **These features might represent if you were an "admin" character in the game.**
   - **List characters in the room by selected attribute:**  
     - Allow users to find the Character in the room matching criteria (e.g. Health, Attack, Name, etc).
   - **List all Rooms with all characters in those rooms**  
     - Group Characters by their Room and display them in a formatted list.
3. **Find a specific piece of a equipment and list the associated character and location**
   - Allow a user to specify the name of an item and output the following,
      - Character holding the item
      - Location of the character

#### **"A+" Stretch Level (500/500 points):**
##### The sky is the limit here!  Be creative!
1. **Include all "C", "B", and "A" level features.**
2. **Stretch Feature: Implement something creative of your own making**
   - This can be **anything** including such things as,
      - Interface improvements
      - Database improvements
      - Architectural changes
      - New feature ideas,
         - mini "quest" system
         - enhanced combat system
         - spell casting system
         - item collection system
         - equipment swapping
         - other character types for providing details
         - etc.
---

### Submission Requirements:
1. Submit the following:
   - A video demonstrating the full functionality of your application (approximately 5 minutes).  I recommend using Canvas Studio.
   - A link to your GitHub repository and your database connection string.
   - A README file, 
      - quickly describe the features you added at each grade level and grading level you attempted to achieve
      - include any final comments on the class in the provided README file
2. Use in-class examples and provided resources to complete the assignment.
3. Handle all user errors gracefully (e.g., invalid input, database issues) and log all errors.

#### Most of all.. Have fun!
---

### Notes for Students:
- This assignment integrates everything you've learned about Entity Framework Core, LINQ, SOLID principles, and advanced console programming.
- Use provided .sql files or seed data to prepopulate your database with sample Items and Abilities.
- Remember to create new migrations for any database changes and use `dotnet ef database update` to apply them.

---

## 💡 Tips

- Stick to **small commits** with meaningful messages
- Use the `ConsoleRpgEntities` project for logic and the `ConsoleRpg` for interaction
- Test early and often
- Use interfaces to keep your design open to expansion

Good luck, and build something epic!

