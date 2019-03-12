using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MineShaftJSON
{
    public int typeMap;

    public int ID;

    public int level;

    public int timeCurrent;

    public int numberMine;

    public long buyMoreMinePrice;

    public int capacity;

    public int miningTime;

    public int state;

    public List<float> timeUpgradeSpecial;

    public List<int> stateUpgradeSpecial;

    public bool isAutoWorking;

    public float timeUnlocking;

    public float timeUpgradeLevel;
}

[System.Serializable]
public class MapJSON
{
    public int typeMap;

    public long totalAmount;

    public long totalMoney;

    public TransporterJSON transporter;
}

[System.Serializable]
public class TransporterJSON
{
    public int level;

    public long capacity;
}

[System.Serializable]
public class BoostJSON
{
    public int type;

    public float time;
}