using System.Collections.Generic;

namespace Dialogue2D
{
    public class DialogueSegment
    {
        public int id; //the id of the segment
        public string text; //the text associated with the segment
        public int next; //the id of the next segment
        public List<DialogueChoice> choices; //the set of choices given the segment
        public List<DialogueAction> actions; //the set of actions taken a result of the segment TODO: implement
    }
}
