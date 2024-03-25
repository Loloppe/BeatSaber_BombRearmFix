# BeatSaber_BombRearmFix

This plugin fix a longstanding bug where bombs removal in internal memory during gameplay rearm all bombs when they shouldn't.
Normally, bombs become disabled (can't be cut) after they reach the Half Jump Mark based on the player HMD position (around 1m).
If you try to hit bombs from the side/back, they won't get cut, but after one or two seconds, it will break and future bombs will get cut if you try again (this is per map).  

Note: The bug will still be there in multiplayer lobby, as it use different code for that. 
