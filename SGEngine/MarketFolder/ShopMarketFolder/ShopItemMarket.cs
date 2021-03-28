using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Обработчик событий UI товара
/// </summary>
public class ShopItemMarket : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    /// <summary>
    /// Контейнер информации о товаре
    /// </summary>
    private GameItem gameItem { get; set; }

    /// <summary>
    /// Менеджер магазинов для возаимодействия
    /// </summary>
    public MarketManager marketBase { private get; set; }

    private Image mainImage;

    /// <summary>
    /// Устанавливает контейнер с информацией о товаре
    /// </summary>
    /// <param name="item"></param>
    public void SetItemMarket(GameItem item) {
        gameItem = item;
        gameItem.Price = UnityEngine.Random.Range(8, 21);
        transform.GetChild(2).GetComponent<Text>().text = gameItem.Id.ToString();
        transform.GetChild(3).GetComponent<Text>().text = gameItem.Name.ToString();

        mainImage = gameObject.transform.GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        var result = marketBase.TryToBuy(gameItem);
        SetUiForBuy(result); 
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mainImage.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mainImage.color = Color.white;
    }

    private void SetUiForBuy(EnumActionMarketItem enumActionMarketItem) {
        switch (enumActionMarketItem) {
            case EnumActionMarketItem.NotSold:
                mainImage.color = Color.red;
                break;
            case EnumActionMarketItem.Sold:
                BuyItemUI();

                break;
            case EnumActionMarketItem.SoldOut:
                BuyItemUI();
                break;
            default:
                break;
        }
    
    }

    private void BuyItemUI() {
        enabled = false;
        mainImage.color = Color.gray;
    }
}
