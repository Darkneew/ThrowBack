using UnityEngine.UIElements;
using UnityEngine;

public class ChatScript : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("Close").clicked += () => GameState.Main.EndDialogue();
    }
}
