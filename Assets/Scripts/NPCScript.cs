using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category 
{ 
    Merchant, 
    Bum,
    Politic,
    Religious,
    LowWorker,
    HighWorker,
    Government
}

public class NPCScript : MonoBehaviour
{
    public string Name { get;} = "Bob";
    // NPC Constant (range between 0 and 1)
    public float FearPropension { get; private set; }
    public float Rebellion { get; private set; }
    public float Calm { get; private set; }
    public float Popularity { get; private set; }
    public float Perception { get; private set; }
    
    public Category Category { get; private set; }

    // Player dependant variables (range between -1 and 1)
    public float PoliticalAgreement { get; private set; }
    public float Fear { get; private set; }
    public float Animosity { get; private set; }

    private Animator _animator;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        GameState.Main.addPauseEvent(Pause, 1);
        GameState.Main.addUnpauseEvent(Unpause, 1);
    }

    public void Pause()
    {
        _animator.speed = 0f;
    }

    public void Unpause()
    {
        _animator.speed = 1f;
    }

    public NPCScript (float fearPropension, float calm, float popularity, float animosity, float rebellion, float perception, Category category)
    {
        FearPropension = fearPropension;
        Popularity = popularity;
        Calm = calm;
        Rebellion = rebellion;
        Perception = perception;
        Category = category;

        Animosity = 0;
        PoliticalAgreement = 0;
        Fear = 0;

        GameState.Main.AddNPC(this);
    }

    public void RunGossip (float importance, float rebellion, float scariness)
    {
        float politicalMatch = Mathf.Sqrt((1 - rebellion * 2) * (1 - Rebellion * 2));
        PoliticalAgreement += politicalMatch * (1 - Mathf.Abs(PoliticalAgreement)) * importance;
        Fear += scariness * (1 - Mathf.Abs(Fear)) * importance;
        Animosity += Calm * (politicalMatch + scariness) / 2 * (1 - Mathf.Abs(Animosity)) * importance;
    }
}
