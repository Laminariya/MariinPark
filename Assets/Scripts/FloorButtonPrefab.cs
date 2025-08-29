using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorButtonPrefab : MonoBehaviour
{
    [HideInInspector] public Button Button;
    private TMP_Text _text;

    public void Init(int floor)
    {
        Button = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
        _text.text = floor+" этаж";
        Deactive();
    }

    public void Active()
    {
        Button.image.color = Color.white;
        _text.color = Color.white;
    }

    public void Deactive()
    {
        Button.image.color = Color.clear;
        _text.color = Color.black;
    }
}
