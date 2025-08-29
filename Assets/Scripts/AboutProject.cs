using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutProject : MonoBehaviour
{

    public Button b_Back;
    public Button b_AboutProject;
    public Button b_Galereya;
    public Button b_Left;
    public Button b_Right;

    public Sprite s_AboutProject;
    public List<Sprite> s_Galereya = new List<Sprite>();

    private Image _image;
    private int _currentSprite;
    
    public void Init()
    {
        _image = GetComponent<Image>();
        b_AboutProject.onClick.AddListener(OnAboutProject);
        b_Galereya.onClick.AddListener(OnGalereya);
        b_Left.onClick.AddListener(OnLeft);
        b_Right.onClick.AddListener(OnRight);
        b_Back.onClick.AddListener(OnBack);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        OnAboutProject();
    }

    private void OnAboutProject()
    {
        _image.sprite = s_AboutProject;
        b_Left.enabled = false;
        b_Right.enabled = false;
    }

    private void OnGalereya()
    {
        _image.sprite = s_Galereya[0];
        _currentSprite = 0;
        b_Left.enabled = true;
        b_Right.enabled = true;
    }

    private void OnLeft()
    {
        _currentSprite--;
        if (_currentSprite <= 0)
            _currentSprite = 0;
        _image.sprite = s_Galereya[_currentSprite];
    }

    private void OnRight()
    {
        _currentSprite++;
        if (_currentSprite >= s_Galereya.Count)
            _currentSprite = s_Galereya.Count-1;
        _image.sprite = s_Galereya[_currentSprite];
    }
    
    private void OnBack()
    {
        gameObject.SetActive(false);
    }

}
