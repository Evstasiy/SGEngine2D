using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MarketManager : MonoBehaviour {

    IMarketBase currentMarket;
    [SerializeField]
    private GameObject MarketItem;
    [SerializeField]
    private GameObject contentZone;

    /// <summary>
    /// Метод закрытия магазина с очисткой контента
    /// </summary>
    /// <returns></returns>
    public bool CloseMarket() {
        try {
            CleanContentZone();
            return true;
        } catch (Exception ex) {
            Debug.LogError($"Во время закрытия магазина {currentMarket} произошла ошибка:{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Метод установки магазина и открытия с очисткой контента
    /// </summary>
    /// <param name="market">Магазин для открытия</param>
    public void SetMarket(EnumMarkets market) {
        switch (market) {
            case EnumMarkets.ShopMarket:
                currentMarket = ShopMarket.shopMarket;
                break;
            default:
                Debug.LogError("В MarketControl отсутствует назначеный элемент для открытия. Enum name: " + market.ToString());
                return;
        }
        currentMarket.SetShopItem(MarketItem);
        OpenMarket(EnumTypeMarketOpen.DefaultOpen);
    }

    /// <summary>
    /// Метод открытия магазина по типу
    /// </summary>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    public void OpenMarket(EnumTypeMarketOpen typeMarketOpen) {
        if (currentMarket == null) {
            Debug.LogError("Магазин не установлен! Enum name: " + typeMarketOpen.ToString());
            return;
        }
        CleanContentZone();
        currentMarket.InstanceItemsInContentZone(contentZone.transform, this, typeMarketOpen);
    }

    /// <summary>
    /// Покупка объекта из магазина
    /// </summary>
    /// <param name="item">Покупаемый объект</param>
    /// <returns>Возвращает EnumActionMarketItem с событием покупки</returns>
    public EnumActionMarketItem TryToBuy(IItem item) {
        var result = currentMarket.TryToBuy(item);
        if (result != EnumActionMarketItem.NotSold) {
            currentMarket.BuyItem(item);
            return result;
        } else
            return result;
    }

    /// <summary>
    /// Удаляет все объекты contentZone, сохраненные ранее в кеше
    /// </summary>
    private void CleanContentZone() {
        try {
            for (int i = 0; i < contentZone.transform.childCount; i++) {
                Destroy(contentZone.transform.GetChild(i).gameObject);
            }
        } catch (Exception ex) {
            Debug.LogError("Во время удаления объекта из contentItems произошла ошибка:" + ex.Message);
        }
        
    }
}

