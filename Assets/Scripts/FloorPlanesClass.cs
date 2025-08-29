using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class FloorPlanesClass : MonoBehaviour
{
    
    public List<FloorsPlaneStartNumber> FloorsPlane = new List<FloorsPlaneStartNumber>();
    public Transform FloorParent;

    private FloorPlanesPrefab _currentPrefab;
    private FloorPlanesPrefab _currentPrefabOnCartFlat;
    
    public void Init()
    {
        
    }

    public void Hide()
    {
        if(_currentPrefab != null)
            Destroy(_currentPrefab.gameObject);
    }

    public void CreateFloor(int korpus, int floor)
    {
        if(_currentPrefab!=null)
            Destroy(_currentPrefab.gameObject);
        int startFlat = FloorsPlane[korpus - 1].StartFlat;
        for (int i = 0; i < FloorsPlane[korpus-1].FloorPlanesPrefabs.Count; i++)
        {
            if (i + 2 == floor)
            {
                FloorPlanesPrefab floorPrefab =
                    Instantiate(FloorsPlane[korpus - 1].FloorPlanesPrefabs[i], FloorParent)
                        .GetComponent<FloorPlanesPrefab>();
                foreach (var myBuilding in GameManager.instance.MyData.MyBuilders)
                {
                    if (myBuilding.Korpus == korpus)
                    {
                        floorPrefab.Init(myBuilding, floor);
                    }
                }

                _currentPrefab = floorPrefab;
            }

            startFlat += FloorsPlane[korpus - 1].FloorPlanesPrefabs[i].FlatSprite.Count;
        }
    }
    
    public void CreateFloorOnCartFlat(int korpus, int floor, int flat, Transform parent)
    {
        if(_currentPrefabOnCartFlat!=null)
            Destroy(_currentPrefabOnCartFlat.gameObject);
        int startFlat = FloorsPlane[korpus - 1].StartFlat;
        for (int i = 0; i < FloorsPlane[korpus-1].FloorPlanesPrefabs.Count; i++)
        {
            if (i + 2 == floor)
            {
                FloorPlanesPrefab floorPrefab =
                    Instantiate(FloorsPlane[korpus - 1].FloorPlanesPrefabs[i], parent)
                        .GetComponent<FloorPlanesPrefab>();
                foreach (var myBuilding in GameManager.instance.MyData.MyBuilders)
                {
                    if (myBuilding.Korpus == korpus)
                        floorPrefab.InitOnCartFlat(myBuilding, floor, startFlat, flat);
                }
                
                _currentPrefabOnCartFlat = floorPrefab;
            }

            startFlat += FloorsPlane[korpus - 1].FloorPlanesPrefabs[i].FlatSprite.Count;
        }
    }


}

[Serializable]
public class FloorsPlaneStartNumber
{
    public int StartFlat;
    public List<FloorPlanesPrefab> FloorPlanesPrefabs = new List<FloorPlanesPrefab>();
}
