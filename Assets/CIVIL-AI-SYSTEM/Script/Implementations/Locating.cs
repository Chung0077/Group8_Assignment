using System.Collections.Generic;
using AISystem.Civil.Ownership;
using AISystem.ItemSystem;
using UnityEngine;

namespace AISystem
{
    public static class Locating
    {
        public static List<Item> GetLocaLItems(Vector3 fromSection, float size, bool careInUse, ITEMS_TYPE itemTypeNeeded = ITEMS_TYPE.NULL, ITEMS itemNeeded = ITEMS.NULL)
        {
            List<Item> items = new List<Item>();

            Collider[] resourceZone = Physics.OverlapSphere(fromSection, size, LayerMask.GetMask("Resource"));

            foreach (Collider candidate in resourceZone)
            {
                if (candidate.gameObject.activeInHierarchy)
                {
                    Item item = candidate.GetComponent<Item>();

                    if (item != null)
                    {
                        if (item.type == itemTypeNeeded || itemTypeNeeded == ITEMS_TYPE.NULL)
                        {
                            if (item.itemName == itemNeeded || itemNeeded == ITEMS.NULL)
                            {
                                if (!item.IsInUse() || careInUse == false)
                                {
                                    items.Add(item);
                                }
                            }
                        }
                    }
                }
            }

            return items;
        }

        public static List<Item> GetItems(AIDataBoard databoard, bool careInUse, OWNERSHIP ownership, ITEMS_TYPE itemTypeNeeded = ITEMS_TYPE.NULL, ITEMS itemNeeded = ITEMS.NULL)
        {
            List<Item> items = new List<Item>();

            if(ownership == OWNERSHIP.LOCAL)
            {
                return GetLocaLItems(databoard.transform.position, 150f, careInUse, itemTypeNeeded, itemNeeded);
            }

            OwnershipManager ownershipManager = databoard.GetOwnershipManager();

            List<Item> candidateList = ownershipManager.GetItems(ownership, itemTypeNeeded, itemNeeded);

            foreach(var item in candidateList)
            {
                if(item.gameObject.activeInHierarchy)
                {
                    if (!item.IsInUse() || careInUse == false)
                    {
                        items.Add(item);
                    }
                }
            }

            if (ownership == OWNERSHIP.ALL)
            {
                items.AddRange(GetLocaLItems(databoard.transform.position, 150f, careInUse, itemTypeNeeded, itemNeeded));
            }

            return items;
        }
    }
}
