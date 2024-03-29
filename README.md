# BeatSaber_BombRearmFix

This plugin fix a longstanding bug where bombs removal in internal memory during gameplay rearm all bombs when they shouldn't. Normally, bombs become disabled (can't be cut) after they reach the Half Jump Mark based on the player HMD position (around 1m). If you try to hit bombs from the side/back, they won't get cut, but after the first bomb get despawned, this will break and future bombs will get cut if you try again (this is per map).

Note:
- Include a setting menu to turn on/off the plugin.
- Using this plugin will cause even more memory leak while playing maps with a lot of bombs (not really lag but high RAM usage).
- This plugin doesn't work in multiplayer.

Expected gameplay behaviour of a bomb according to the current game logic:
- Bomb approach the player.
- Half Jump Mark (in front of the player) is reached, noteJumpDidPassHalfEvent get triggered, which trigger HandleDidPassHalfJump from BombNoteController.
- _cuttableBySaber.canBeCut become false (disabled). That bomb cannot be cut anymore by the player.
- Repeat.
  
Reality:
- Expected gameplay behaviour happen for one bomb (or more).
- After the internal despawn of the first bomb, all upcoming bombs can always be cut at all time until the player leave the map.
- Repeat.

Bug explanation:
- At the end of NoteJump, noteJumpDidFinishEvent get triggered, which also trigger HandleNoteDidFinishJump.
- This method run HandleNoteControllerNoteDidFinishJump, which run Despawn, which in turn run DespawnInternal.
- This method clean the bomb from _activeItems and _memoryPool, which seems to activate that bug. ???
