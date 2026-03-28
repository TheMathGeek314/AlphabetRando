# AlphabetRando

This Randomizer connection adds every letter and number of the English alphabet to rando, with support for user-defined punctuation and foreign characters.

The letters in the UI of the game itself will not be visible until you obtain their corresponding item.

You can choose whether vowels, consonants, or numbers are separately included. A toggle also exists for dupe items.

When a letter item is obtained, text will only reflect the new letter's visibility when it next gets updated. 
This means that some infrequently-updated text (such as area names on the map) may not update promptly or at all. 
If this is an issue for you, reopening your save should force them to update.

To add custom characters, you can type the desired characters into *CustomCharacters.txt*, which can be found in the AlphabetRando mod folder. 
This file can be reloaded in-game from the mod menu.

## Known Bugs
- At toll locations (stags, maps, elevator pass...), item names may appear blank
  - This was a patch that would previously hard-lock your game, so I guess blank is better?
- If another mod calls *SetText()*, the text may not be retained properly
  - I spent so long trying to fix this and came up with nothing but a headache, I'm sorry
- The characters '<' and '>' are not permitted due to how the game uses them to format strings