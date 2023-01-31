using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    private bool pressedTalkButton;

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
        if (Input.GetAxis("Talk") > 0.5 && pressedTalkButton == false)
        {
            pressedTalkButton = true;
            if (GameState.Main.State == GamePeriod.Running)
            {
                GameState.Main.StartDialogue();
            }
            else if (GameState.Main.State == GamePeriod.Talking)
            {
                GameState.Main.EndDialogue();
            }
        }
        else if (pressedTalkButton == true && Input.GetAxis("Talk") < 0.5)
        {
            pressedTalkButton = false;
        }
        
    }
}
