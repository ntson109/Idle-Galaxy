using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;
using UnityEngine.UI;

public class SpaceShip : MonoBehaviour 
{
    #region === STRUCT ===
    [Serializable]
    public struct Properties
    {
        public string name;

        public int level;

        public int capacity;

        public int transportingTime;

        public long unitPrice;

        public float unlockTime;

        public int unlockCondition;
    }

    public enum StateSpaceShip
    {
        LOCK,
        UNLOCKING,
        UPGRADING,
        WORKING
    }

    #endregion

    public int ID;
    public SpaceShip.Properties properties; //thông số cơ bản
    public StateSpaceShip state;
    public int numberShip;

    [Header("UI")]
    public Text txtName;
    public Text txtLevel;
    public Text txtTimer;
    public Text txtNumber;
    public Text txtMoreShipPrice;
    public Text txtX;

    #region === START VS UPDATE ===
    void Start()
    {
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
    }

    void LateUpdate()
    {

    }

    #endregion

    void ON_START_GAME()
    {

    }

    void SetInfo()
    {

    }


}
