using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void GameEvent();

public enum GamePeriod
{
    Running,
    Starting,
    Ending,
    Talking
}
public class GameState : MonoBehaviour
{
    public float RunTime { get; set; } = 30;
    public float GossipRange { get; set; } = 20; // distance direct gossips can reach
    public float GossipageLowerLimit { get; set; } = 0.3f; // number under which there is no gossip
    public float GossipageUpperLimit { get; set; } = 2f; // number on top of which everyone gossips

    static public GameState Main { get; private set; }

    public GamePeriod State { get; private set; } = GamePeriod.Starting;

    public List<NPCScript> NPCs { get; private set; } = new List<NPCScript>();

    [SerializeField] private GameObject TalkUI;
    [SerializeField] private GameObject GeneralUI;
    private Label TimerUI;

    private PriorityList<GameEvent> endEvents;
    private PriorityList<GameEvent> startEvents;
    private PriorityList<GameEvent> pauseEvents;
    private PriorityList<GameEvent> unpauseEvents;

    private float _startTime;
    private float _deltaRunTime;

    public void DisplayUI (GameObject UI, bool Hide)
    {
        if (Hide) UI.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container").style.display = DisplayStyle.Flex;
        else UI.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container").style.display = DisplayStyle.None;
    }

    public void Gossip (float ActionImportance, float Rebellion, float Scariness) // parameters ranged between 0 and 1, except scariness between -1 and 1
    {
        float gossipage = 0;
        int categories = 0;
        foreach (NPCScript script in NPCs) 
        { 
            if (Vector3.Distance(script.transform.position, this.transform.position) < GossipRange) 
            { 
                gossipage += script.Popularity;
                categories |= (int)script.Category;
            } 
        }
        gossipage *= ActionImportance;
        if (gossipage < GossipageLowerLimit) return;
        else if (gossipage > GossipageUpperLimit) { foreach (NPCScript script in NPCs) { script.RunGossip(ActionImportance, Rebellion, Scariness); } }
        else
        {
            foreach (NPCScript script in NPCs)
            {
                if (((int) script.Category & categories) != 0) script.RunGossip(ActionImportance, Rebellion, Scariness);
            }
        }
    }
  
    public GameState()
    {
        Main = this;
        startEvents = new PriorityList<GameEvent>();
        endEvents = new PriorityList<GameEvent>();
        pauseEvents = new PriorityList<GameEvent>();
        unpauseEvents = new PriorityList<GameEvent>();
    }

    public void AddNPC (NPCScript npc) { NPCs.Add(npc); }

    public void StartDialogue()
    {
        State = GamePeriod.Talking;
        DisplayUI(TalkUI, true);
    }

    public void EndDialogue()
    {
        State = GamePeriod.Running;
        DisplayUI(TalkUI, false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (TimerUI != null) TimerUI.text = Mathf.Floor(_deltaRunTime - Time.fixedTime + _startTime).ToString();
        if ((State == GamePeriod.Running || State == GamePeriod.Talking) && Time.fixedTime - _startTime > _deltaRunTime)
        {
            State = GamePeriod.Ending;
            DisplayUI(GeneralUI, false);
            if (State == GamePeriod.Talking) EndDialogue();
            PriorityList<GameEvent>.Node n = endEvents.Beginning;
            while (n != null) 
            {
                n.Object();
                n = n.Next;
            }
            StartCoroutine(Ending());
        }
        else if (State == GamePeriod.Starting)
        {
            DisplayUI(GeneralUI, true);
            TimerUI = GeneralUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("Timer");
            DisplayUI(TalkUI, false);
            PriorityList<GameEvent>.Node n = startEvents.Beginning;
            while (n != null)
            {
                n.Object();
                n = n.Next;
            }
            _startTime = Time.fixedTime;
            _deltaRunTime = RunTime;
            State = GamePeriod.Running;
        } 
    }

    private IEnumerator Ending()
    {
        DisplayUI(TalkUI, false);
        yield return new WaitForSeconds(1f);
        State = GamePeriod.Starting;
        yield return null;
    }

    public void addStartEvent (GameEvent e, float priority) {startEvents.Insert(e, priority);} 
    public void addEndEvent(GameEvent e, float priority) { endEvents.Insert(e, priority); }
    public void addPauseEvent(GameEvent e, float priority) { pauseEvents.Insert(e, priority); }
    public void addUnpauseEvent(GameEvent e, float priority) { unpauseEvents.Insert(e, priority); }

}
