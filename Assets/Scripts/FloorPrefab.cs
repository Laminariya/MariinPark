using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorPrefab : MonoBehaviour
{

    public Sprite Active;
    public Sprite NotActive;

    [HideInInspector] public Button Button;
    [HideInInspector] public int Floor;
    
    private TMP_Text _text;

    public void Init(int floor)
    {
        Floor = floor;
        Button = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
        Button.onClick.AddListener(OnActivate);
        _text.text = floor.ToString();
        OnDeactivate();
    }

    private void OnActivate()
    {
        Button.image.sprite = Active;
        _text.color = Color.white;
    }

    public void OnDeactivate()
    {
        Button.image.sprite = NotActive;
        _text.color = Color.black;
    }


}
