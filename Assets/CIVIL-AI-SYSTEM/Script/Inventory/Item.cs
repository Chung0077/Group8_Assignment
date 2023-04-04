using UnityEngine;

namespace AISystem.ItemSystem
{
    public class Item : MonoBehaviour
    {
        public ITEMS itemName = ITEMS.NULL;
        public ITEMS_TYPE type = ITEMS_TYPE.NULL;
        public int quanity;
        [SerializeField] bool inUse = false;
        [SerializeField] Interactor interaction = null;

        public void Awake()
        {
            interaction = GetComponent<Interactor>();
        }

        public bool IsInUse()
        {
            return inUse;
        }

        public void SetInUse(bool update)
        {
            if(interaction != null)
            {
                interaction.Apply(update);
            }

            inUse = update;
        }
    }
}
