using UnityEngine;
using TMPro;

public class ModelDialog : MonoBehaviour {

    public TextMeshProUGUI text;

    [HideInInspector]
    public bool isVisible = true;

    public void Start() {
        SetVisibility(false);
    }

    public void SetVisibility(bool pIsVisible) {
        gameObject.SetActive(pIsVisible);
        isVisible = pIsVisible;
    }

    public void Set(string pMessage) {
        SetVisibility(true);
        text.text = pMessage;
    }

    public void OnClose() {
        SetVisibility(false);
    }
}
