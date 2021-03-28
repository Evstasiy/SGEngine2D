using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation
{
    public class GameItemWorker : IDataBaseWorker
    {
        public List<GameItem> allGameItems { get; private set; }
        public List<GameItem> allGameItemsSave { get; private set; }

        private List<TestItem> xmlGameItemsParameters;
        private List<SaveItem> saveItems;

        private SaveGameInformation saveGameInformation;

        public GameItemWorker(GameParameters xmlGameInformation, SaveGameInformation saveGameInformation) {
            xmlGameItemsParameters = xmlGameInformation.WorldObjects.Items.TestItems;
            this.saveGameInformation = saveGameInformation;
            saveItems = saveGameInformation.Save_WorldObjects.Save_Items.SaveItemList;
        }

        /// <summary>
        /// Соединяет информацию из файла локализации с базовыми игровыми показателями 
        /// </summary>
        /// <returns>Объект с полной информацией и локализацией</returns>
        private List<GameItem> ConvertXmlToGameItemList(GameLocalization gameLocalization) {
            var localiz = gameLocalization.WorldObjects_Localization.Items_Localization.descriptionItems;

            var badIds = CheckAndGetBadIds(localiz.Select(x => x.Id).ToList(), xmlGameItemsParameters.Select(x => x.Id).ToList(), "ConvertXmlToItemsList");
            xmlGameItemsParameters = xmlGameItemsParameters.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

            List<GameItem> gameItems = new List<GameItem>();
            foreach (var paramsItem in xmlGameItemsParameters) {
                try {
                    GameItem gameItem = new GameItem() {
                        Id = Convert.ToInt32(paramsItem.Id),
                        Description = localiz.FirstOrDefault(x => x.Id == paramsItem.Id).MainDescription,
                        Name = localiz.FirstOrDefault(x => x.Id == paramsItem.Id).SecondaryDescription
                    };
                    gameItems.Add(gameItem);
                }
                catch (Exception ex) {
                    Debug.LogError(ex.Message);
                    continue;
                }
            }
            return gameItems;
        }

        /// <summary>
        /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
        /// </summary>
        /// <returns>Объект с полной информацией и локализацией</returns>
        private List<GameItem> ConvertSaveXMLToGameItemList() {
            var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), "ConvertSaveXMLToGameItemList");
            saveItems = saveItems.Where(x => allGameItems.Any(y => y.Id == x.Id)).ToList();

            List<GameItem> gameItems = new List<GameItem>();
            foreach (var paramsItem in saveItems) {
                try {
                    var idItem = Convert.ToInt32(paramsItem.Id);
                    var gameItemInAll = allGameItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                    GameItem gameItem = new GameItem() {
                        Id = idItem,
                        Description = gameItemInAll.Description,
                        Name = gameItemInAll.Name,
                        IsLock = paramsItem.isLock
                    };
                    gameItems.Add(gameItem);
                }
                catch (Exception ex) {
                    Debug.LogError(ex.Message);
                    continue;
                }
            }
            return gameItems;
        }

        //ПЕРЕНЕСТИ В ОТДЕЛЬНЫЙ КЛАСС
        /// <summary>
        /// Сравнивает две коллекции, пишет в дебаг о всех объектах, которые не удалось найти в коллекции A из B
        /// </summary>
        /// <param name="listIdsA">Коллекция, в которой надо искать</param>
        /// <param name="listIdsB">Колеекция с элементами для поиска</param>
        /// <param name="methodName">Метод, где запущен поиск, нужен для инфы в дебаге</param>
        /// <returns>Коллекция не найденных элементов в коллекции A</returns>
        public List<int> CheckAndGetBadIds(List<int> listIdsA, List<int> listIdsB, string methodName = "CheckAndGetBadIds") {
            var notFoundIds = listIdsA.Except(listIdsB).ToList();
            string badId = "Bad localize ids count: " + notFoundIds.Count;
            notFoundIds.ForEach(x => badId += "\nId:" + x);
            if (notFoundIds.Count > 0)
                Debug.LogWarning($"{methodName} - {badId}");
            return notFoundIds;
        }

        public bool ReloadObjectsLanguage(GameLocalization gameLocalization) {
            try {
                allGameItems = ConvertXmlToGameItemList(gameLocalization);
                allGameItemsSave = ConvertSaveXMLToGameItemList();
                return true;
            }
            catch (Exception ex) {

                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: ReloadItemsLanguage()");
                return false;
            }
        }

        /// <summary>
        /// Проверяет полученные объекты для дальнейшего соханения в базу данных
        /// </summary>
        /// <param name="saveChangeItems">Объекты, которые будут сохранены в БД</param>
        /// <returns></returns>
        public bool SaveChangesInSaveFile(List<GameItem> saveChangeItems) {
            try {
                var allSaveItems = saveGameInformation.Save_WorldObjects.Save_Items.SaveItemList;

                var newObjects = saveChangeItems.Where(x => !allSaveItems.Any(y => y.Id == x.Id));

                var saveChangeItemsWithNotNew = saveChangeItems.Except(newObjects);
                var changeObjects = GetChangeSaveItems(saveChangeItemsWithNotNew.ToList(), allSaveItems);
                allSaveItems = allSaveItems.Except(changeObjects).ToList();
                allSaveItems = allSaveItems.Concat(changeObjects).ToList();

                foreach (var newObject in newObjects) {
                    allSaveItems.Add(new SaveItem() { Id = newObject.Id, isLock = newObject.IsLock });
                }

                var deleteObjects = allSaveItems.Where(x => !saveChangeItems.Any(y => y.Id == x.Id));
                allSaveItems = allSaveItems.Except(deleteObjects).ToList();

                saveGameInformation.Save_WorldObjects.Save_Items.SaveItemList = allSaveItems;
                DataBaseManager.dataBaseManager.SaveChanges(saveGameInformation);
            }
            catch (Exception ex) {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: SaveChangesInSaveFile()");
            }
            return true;
        }

        /// <summary>
        /// Добавляет запись в базу данных сохранения
        /// </summary>
        /// <param name="Item">Объект, который будут сохранен в БД</param>
        /// <returns></returns>
        public bool AddItemInSaveFile(GameItem Item) {
            try {
                var allSaveItems = saveGameInformation.Save_WorldObjects.Save_Items.SaveItemList;
                allSaveItems.Add(Item);
                
                saveGameInformation.Save_WorldObjects.Save_Items.SaveItemList = allSaveItems;
                DataBaseManager.dataBaseManager.SaveChanges(saveGameInformation);
            }
            catch (Exception ex) {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
            }
            return true;
        }

        /// <summary>
        /// Проверяет на измение имеющихся в БД объектов сохранения и пришедших
        /// </summary>
        /// <param name="saveChangeItems">Объекты, нуждающиеся в проверке</param>
        /// <param name="allSaveItems">Объекты из базы данных</param>
        /// <returns></returns>
        private List<SaveItem> GetChangeSaveItems(List<GameItem> saveChangeItems, List<SaveItem> allSaveItems) {
            List<SaveItem> changeItems = new List<SaveItem>();
            foreach (var saveChangeItem in saveChangeItems) {
                try {
                    var itemInSaveFile = allSaveItems.FirstOrDefault(x => x.Id == saveChangeItem.Id);
                    if (itemInSaveFile.isLock != saveChangeItem.IsLock) {
                        itemInSaveFile.isLock = saveChangeItem.IsLock;
                        changeItems.Add(itemInSaveFile);
                        continue;
                    }
                }
                catch (Exception ex) {
                    Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: GetChangeSaveItems()");
                }
            }
            return changeItems;
        }

        public void UpdateSaveObjects(SaveGameInformation saveGameInformation) {
            this.saveGameInformation = saveGameInformation;
            allGameItemsSave = ConvertSaveXMLToGameItemList();
        }
    }
}
