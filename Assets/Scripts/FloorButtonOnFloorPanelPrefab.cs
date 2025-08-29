using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorButtonOnFloorPanelPrefab : MonoBehaviour
{
    [HideInInspector] public Button Button;
    private TMP_Text _text;
    public int Floor;

    public void Init(int floor)
    {
        Button = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
        Floor = floor;
        _text.text = floor.ToString();
        Deactive();
    }

    public void Active()
    {
        Button.image.color = GameManager.instance.ActiveColor;
        _text.color = Color.white;
    }

    public void Deactive()
    {
        Button.image.color = Color.white;
        _text.color = Color.black;
    }
}
