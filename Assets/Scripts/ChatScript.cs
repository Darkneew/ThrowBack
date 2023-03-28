using UnityEngine.UIElements;
using UnityEngine;
using System.IO;
using System.Text.Json;

public class ChatDB
{
    // fill
}

public class ChatScript : MonoBehaviour
{
    public ChatDB chatDB;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("Close").clicked += () => GameState.Main.EndDialogue();
    }

    private void Start()
    {
        //chatDB = JsonSerializer.Deserialize<ChatDB>(File.ReadAllText(@"../chatDB.json"));
    }
}
