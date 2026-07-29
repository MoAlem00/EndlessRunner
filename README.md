# Endless Runner - Unity Mobile

This is our submission for Task 4 (Final Mobile Integration & Advanced Systems). The game is an endless runner with profiles, themes, difficulty settings, power ups, a daily reward system, and a goals and progression system on top.

## Advanced feature we chose

We went with Option 4, Game Goals & Progression System. The idea is to give the game a sense of long term progress instead of just an endless loop. Players have goals like collecting a certain number of coins in a run, collecting power ups, or reaching a distance, and completing them gives a coin reward. Some of the harder goals also unlock a permanent upgrade that sticks with the player across every future run, like a coin bonus, a starting shield, or a new theme.

## How we split the work

Student A built the GoalManager and the data side of the progression system. That means the Goal scriptable objects, the logic that checks a run's stats against each goal's target while you're playing, and the save and load code that keeps track of goal progress across sessions inside the player's profile. Student A also built the Daily Reward System, so the local notification that reminds a player to come back, and the claim screen that gives them a reward when they open the app.

Student B (me) built the Unity Analytics integration and the other half of the progression system, the Progression UI and the Reward Unlock logic. That covers the screen that shows every goal with its progress and a claim button, and the manager that decides what a claimed goal actually gives the player permanently, whether that's more coins, a shield at the start of a run, or a theme that was previously locked.

## Unity Analytics

We integrated the Unity Analytics package (Unity Gaming Services) and track the following.

Standard metrics:

- Session start, along with the app version and platform
- App version, sent as part of that same session start event

Custom events:

- `run_started`, sent when a run begins, with the difficulty and the run seed
- `run_ended`, sent when a run is over, with the final score, coins, and distance
- `milestone_reached`, sent every 100 meters during a run
- `goal_completed`, sent when a goal is claimed, with the goal id and what kind of reward it gave
- `reward_claimed`, sent whenever coins are actually handed to the player, whether that's from a goal or the daily reward

That's five custom events, more than the three the assignment asked for.

## Progression system in more detail

Every goal is a scriptable object with an id, a description, a type (coins collected, distance reached, or power ups collected), and a target. Most goals just give coins when claimed, but a few of the harder ones are set up to also grant a permanent upgrade instead of only a one time reward. Right now that's collecting 100 coins in a run for a permanent coin bonus, collecting 30 power ups for a starting shield, and reaching 3000 meters to unlock the Space theme, which is locked by default until that goal is claimed.

The Progression screen lists every goal with its current progress and lets the player claim any goal that's been completed. Claiming applies the coin reward right away and, if that goal grants a permanent upgrade, it also takes effect immediately for the next run.

## Unity Analytics Dashboard screenshots

_to-do:_ add screenshots here of the dashboard showing the custom events once the build has run and sent some data :(

## Gameplay video

_to-do:_ add the video link here :(
