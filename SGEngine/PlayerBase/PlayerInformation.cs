using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation
{

    private static PlayerInformation singltonPlayerInformation;

    public static PlayerInformation playerInformation {
        get {
            if (singltonPlayerInformation == null)
                return singltonPlayerInformation = new PlayerInformation();
            else
                return singltonPlayerInformation;
        }
        private set { }
    }


    private int money = 10;
    public int GetPlayerMoney() {
        return money;
    }
}
