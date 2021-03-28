using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : IItem {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsLock { get; set; }
    public int Price { get; set; }

    public int GetId()
    {
        return Id;
    }

    public static implicit operator SaveItem(GameItem gameItem) {
        return new SaveItem { Id = gameItem.Id, isLock = gameItem.IsLock };
    }
}
