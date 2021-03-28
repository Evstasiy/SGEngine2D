using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBoostItem : UpgradeBaseObject {
    public UpgradeBoostInfoObject[] BoostObjectsInfo;

    public static UpgradeBoostItem ConvertUpgradeBaseObject(UpgradeBaseObject self) =>
        new UpgradeBoostItem() {
            Id = self.Id,
            Description = self.Description,
            LvlToUnlock = self.LvlToUnlock,
            Name = self.Name,
            Price = self.Price,
            PriceSpecialMoney = self.PriceSpecialMoney,
            TimeToSearch = self.TimeToSearch
        };

    public static implicit operator Save_UpdateBoostObject(UpgradeBoostItem Item) {
        return new Save_UpdateBoostObject { Id = Item.Id };
    }
}

public class UpgradeBoostInfoObject {
    public int idBoostObject;
    public int PercentBoost;
}
