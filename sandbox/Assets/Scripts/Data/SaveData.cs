using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : ScriptableObject
{
    //default constructor
    public SaveData()
    {
		
	}

    //the player's state will be saved here including:
        //game state
            //current progression
            //triggered events
                //unlocked/locked doors/areas
                //opened/unopened chests
                //defeated/undefeated bosses
            //game statistics
                //distance walked
                //attack info
                    //enemies defeated
                    //number of hits
                        //number of stabs
                        //number of swings
                        //number of power hits
                    //damage given/taken
                    //number of deaths
                //number of quests completed
                //percentage of game completed
        //current area
            //to spawn when loading next time
        //backpack state
            //map state
                //unlocked areas
            //journal state
                //current objectives
                //current hints
                //history of hints and objectives
            //inventory state
                //items
                //gold
        //dialogue state
            //overall progression related to game state
            //individual character dialogue progressions
    //this will be updated/saved whenever changed
        //autosave only to prevent redoing dialogue mistakes, etc.
        //all information required to get the player back to where they were
}
