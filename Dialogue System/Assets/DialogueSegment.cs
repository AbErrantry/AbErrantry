using System.Collections.Generic;

public class DialogueSegment
{
    public int SegmentID;
    public string SegmentText;
    public int SegmentType;

    public int SegmentNextID;
    public List<DialogueChoice> SegmentChoices;
    //public <type> SegmentAction; TODO: implement
}
