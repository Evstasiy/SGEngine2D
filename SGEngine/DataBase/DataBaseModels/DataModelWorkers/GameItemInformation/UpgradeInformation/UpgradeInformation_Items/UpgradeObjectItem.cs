using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeObjectItem : UpgradeBaseObject {
    public int[] OpenIds;

    public static UpgradeObjectItem ConvertUpgradeBaseObject(UpgradeBaseObject self) =>
        new UpgradeObjectItem() {
            Id = self.Id,
            Description = self.Description,
            LvlToUnlock = self.LvlToUnlock,
            Name = self.Name,
            Price = self.Price,
            PriceSpecialMoney = self.PriceSpecialMoney,
            TimeToSearch = self.TimeToSearch
        };

    public static implicit operator Save_UpdateNewObject(UpgradeObjectItem Item) {
        return new Save_UpdateNewObject { Id = Item.Id };
    }
}
