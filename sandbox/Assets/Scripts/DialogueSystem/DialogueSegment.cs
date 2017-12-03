using System.Collections.Generic;

public class DialogueSegment
{
    public int id; //the id of the segment
    public string text; //the text associated with the segment
    public int type; //the type of segment TODO: omit for just choices

    public int next; //the id of the next segment
    public List<DialogueChoice> choices; //the set of choices given the segment
    //public <type> action; //the action as a result of the segment TODO: implement
}
