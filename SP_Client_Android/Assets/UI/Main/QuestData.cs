using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questName;
    public int[] questNpcId;

    public QuestData(string name, int[] npc)
    {
        questName = name;
        questNpcId = npc;
    }
}
