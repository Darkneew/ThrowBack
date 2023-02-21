using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkIndicator : MonoBehaviour
{

    [SerializeField] private GameObject CharacterTarget;
    private CharacterAction CharacterTargetAction;

    private void Start()
    {
        CharacterTargetAction = CharacterTarget.GetComponent<CharacterAction>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (CharacterTargetAction.talkTarget == null)
        {
            transform.position = new Vector3(0, -10, 0);
        } 
        else
        {
            transform.position = CharacterTargetAction.talkTarget.GetPosition() + Vector3.up * (1.1f + 0.03f * Mathf.Cos(Time.time * Mathf.PI));
        }
    }
}
