using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public float maxTalkRange;

    private bool pressedTalkButton;

    public NPCScript talkTarget = null;

    private void Start()
    {
        GameState.Main.addStartEvent(Init, 0);
    }

    private void Init()
    {
        pressedTalkButton = false;
    }

    void Update()
    {
        float minDist = maxTalkRange + 1;
        NPCScript minNpc = null;
        foreach (NPCScript npc in GameState.Main.NPCs)
        {
            if (Vector3.Distance(npc.GetPosition(), transform.position) < minDist)
            {
                minNpc = npc;
                minDist = Vector3.Distance(npc.GetPosition(), transform.position);
            }
        }
        talkTarget = minNpc;
        if (Input.GetAxis("Talk") > 0.5 && pressedTalkButton == false)
        {
            if (talkTarget != null)
            {
                pressedTalkButton = true;
                if (GameState.Main.State == GamePeriod.Running)
                {
                    GameState.Main.StartDialogue();
                }
            }
        }
        else if (pressedTalkButton == true && Input.GetAxis("Talk") < 0.5)
        {
            pressedTalkButton = false;
        }
        
    }
}
