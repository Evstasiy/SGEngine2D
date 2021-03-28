﻿using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopMarket : IMarketBase {

    public GameObject marketItem;
    private DataBaseManager dataBaseManager;
    private PlayerInformation playerInformation;

    private static ShopMarket singltonShopMarket;
    public static ShopMarket shopMarket { 
        get {
            if (singltonShopMarket == null)
                return singltonShopMarket = new ShopMarket();
            else 
                return singltonShopMarket;
        }
        private set { }
    }

    public ShopMarket() {
        Debug.Log("Shop!");
        dataBaseManager = DataBaseManager.dataBaseManager;
        playerInformation = PlayerInformation.playerInformation;
    }

    public void SetShopItem(GameObject itemMarket) {
        if (this.marketItem == null)
            marketItem = itemMarket;
    }

    public List<GameObject> InstanceItemsInContentZone(Transform contentZone, MarketManager marketManager, EnumTypeMarketOpen typeMarketOpen) {
        var allGameItems = GetItemsForOpen(typeMarketOpen);
        var gameItemsObjs = new List<GameObject>();
        foreach (var gameItem in allGameItems) {
            var itemObject = Object.Instantiate(marketItem, Vector3.zero, Quaternion.identity, contentZone);
            itemObject.GetComponent<ShopItemMarket>().SetItemMarket(gameItem);
            itemObject.GetComponent<ShopItemMarket>().marketBase = marketManager;
            gameItemsObjs.Add(itemObject);
        }
        return gameItemsObjs;
    }

    /// <summary>
    /// Предоставляет товар исходя из типа открытия магазина
    /// </summary>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    /// <returns>Товары</returns>
    private List<GameItem> GetItemsForOpen(EnumTypeMarketOpen typeMarketOpen) {
        var allGameItems = dataBaseManager.GameItemWorker.allGameItems;
        switch (typeMarketOpen) {
            case EnumTypeMarketOpen.DefaultOpen:
                allGameItems = allGameItems.OrderBy(x => x.Name).ToList();
                break;
            case EnumTypeMarketOpen.OrderByPrice:
                allGameItems = allGameItems.OrderBy(x => x.Id).ToList();
                break;
            default:
                //allGameItems.OrderBy(x => x.Name).ToList();
                allGameItems = allGameItems.OrderByDescending(x => x.Id).ToList();
                break;
        }
        return allGameItems;
    }

    public EnumActionMarketItem TryToBuy(IItem itemMarket) {
        GameItem item = (GameItem)itemMarket;
        if (item.Price <= playerInformation.GetPlayerMoney()) {
            return EnumActionMarketItem.Sold;
        } else {
            return EnumActionMarketItem.NotSold;
        }
    }
    
    public bool BuyItem(IItem itemMarket) {
        GameItem item = (GameItem)itemMarket;
        return dataBaseManager.GameItemWorker.AddItemInSaveFile(item);
    }
}
