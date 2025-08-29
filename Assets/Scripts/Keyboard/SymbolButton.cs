using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymbolButton : MonoBehaviour
{
    public bool IsCapital = true;
    [HideInInspector] public TextMeshProUGUI Symbol;
    private Button _button;
    private KeyboardPanel _keyboard;


    public void Init(KeyboardPanel keyboardPanel)
    {
        _keyboard = keyboardPanel;
        Symbol = GetComponentInChildren<TextMeshProUGUI>();
        Symbol.enabled = false;
        //Debug.Log("Init text");
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _keyboard.AddInput(Symbol.text[0].ToString());
    }

}