﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : ScriptableObject
{
    //default constructor
    public DialogueData()
    {
		
	}
	
    //this is the dialogue database reference in memory
	    //load in the dialogue data
            //key-> character and dialogue
            //collection of characters
                //collection of dialogues (already set up in DialogueManager-> move here)      
    //whenever we initiate a dialogue, we consult the key to locate the 
        //conversation in question. Then, we start that conversation.
}