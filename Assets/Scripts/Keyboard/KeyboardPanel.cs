using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardPanel : MonoBehaviour
{

    public enum Language
    {
        Eng, Rus
    }
    
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private GameObject _kb_eng;
    [SerializeField] private GameObject _kb_num1;
    [SerializeField] private GameObject _kb_num2;
    
    public List<Sprite> Sprites = new List<Sprite>();

    private bool _isCapital;
    private List<SymbolButton> _symbolButtons = new List<SymbolButton>();
    private Language _language;
    
    public void Init()
    {
        _symbolButtons.AddRange(_kb_eng.GetComponentsInChildren<SymbolButton>());
        _symbolButtons.AddRange(_kb_num1.GetComponentsInChildren<SymbolButton>());
        _symbolButtons.AddRange(_kb_num2.GetComponentsInChildren<SymbolButton>());
        foreach (var symbolButton in _symbolButtons)
        {
            symbolButton.Init(this);
        }
    }

    public void SetInputField(TMP_InputField inputField)
    {
        _input = inputField;
    }

    public string GetEmail()
    {
        return _input.text;
    }

    public void ClearEmail()
    {
        _input.text = "";
    }

    public void Show()
    {
        OffKB();
        _kb_eng.SetActive(true);
        _isCapital = false;
        _language = Language.Eng;
    }

    public void OnShift()
    {
        if (_isCapital)
        {
            OffShift();
            return;
        }
        _isCapital = true;
        foreach (var button in _symbolButtons)
        {
            if (button.IsCapital)
            {
                button.Symbol.text = char.ToUpper(button.Symbol.text[0]).ToString();
            }
        }
    }

    public void AddInput(string symbol)
    {
        OffShift();
        _input.text += symbol;
    }

    public void Erase()
    {
        if(_input.text.Length==0) return;
        _input.text = _input.text.Substring(0, _input.text.Length - 1);
    }

    public void GoToKeyboard(GameObject keyboard)
    {
        OffKB();
        keyboard.SetActive(true);
        
    }

    private void OffShift()
    {
        foreach (var button in _symbolButtons)
        {
            if (button.IsCapital)
            {
                button.Symbol.text = char.ToLower(button.Symbol.text[0]).ToString();
            }
        }

        _isCapital = false;
    }

    public void SwitchLanguage()
    {
        if (_language == Language.Eng)
        {
            //GoToKeyboard(_kb_rus);
            _language = Language.Rus;
        }
        else if (_language == Language.Rus)
        {
            GoToKeyboard(_kb_eng);
            _language = Language.Eng;
        }
    }

    public void GoToLanguage()
    {
        GoToKeyboard(_kb_eng);
    }
    

    private void OffKB()
    {
        _kb_eng.SetActive(false);
        _kb_num1.SetActive(false);
        _kb_num2.SetActive(false);
    }

}
