using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public TypeMap type;
    public List<MineShaft> lstMineShaft;

    void Start()
    {

    }
    void Update()
    {

    }
}

public enum TypeMap
{
    MOON,
    EARTH
}
