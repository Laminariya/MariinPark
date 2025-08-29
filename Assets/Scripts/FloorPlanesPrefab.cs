using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class FloorPlanesPrefab : MonoBehaviour
{

    public int StartNumberFlat;
    public Image FloorPlane;
    public List<Image> FlatSprite = new List<Image>();
    
    public void Init(MyBuilding myBuilding, int floor)
    {
        foreach (var image in FlatSprite)
        {
            image.gameObject.SetActive(false);
        }
        
        foreach (var myObject in myBuilding.myRealtyobjects)
        {
            if (myObject.RealtyObject.floor == floor)
            {
                Debug.Log(myObject.Number +" " + StartNumberFlat + " " + floor);
                FlatSprite[myObject.Number - StartNumberFlat].gameObject.SetActive(true);
                FlatSprite[myObject.Number - StartNumberFlat].GetComponentInChildren<Button>().onClick
                    .AddListener(() => OnClick(myObject));
            }
        }
    }
    
    public void InitOnCartFlat(MyBuilding myBuilding, int floor, int numberFirstFlat, int numberFlat)
    {
        foreach (var image in FlatSprite)
        {
            image.gameObject.SetActive(false);
        }
        
        foreach (var myObject in myBuilding.myRealtyobjects)
        {
            if (myObject.RealtyObject.floor == floor && myObject.Number == numberFlat)
            {
                FlatSprite[myObject.Number - numberFirstFlat].gameObject.SetActive(true);
            }
        }
    }

    private void OnClick(MyRealtyObject myObject)
    {
        GameManager.instance.cartFlatPanel.Show(myObject);
    }

}
