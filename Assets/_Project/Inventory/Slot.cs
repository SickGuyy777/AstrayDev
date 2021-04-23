using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Inventory
{
    public class Slot
    {
        public Item Item { get; private set; }

        public int Amount => Item.Amount;
        public int MaxStack => Item.MaxStack;
        public bool IsEmpty => Item == null || Item.Info == null;

        /// <summary>
        /// Add <paramref name="itemToAdd"/> to <see langword="this"/> <see cref="Slot"/>.
        /// </summary>
        /// <returns>The remaining amount.</returns>
        public int AddItem(Item itemToAdd, int amountToAdd = 0)
        {
            if (!IsEmpty || !Item.IsSameItemType(itemToAdd.Info))
                return itemToAdd.Amount;

            if (amountToAdd <= 0)
                amountToAdd = itemToAdd.Amount;

            int remaining = amountToAdd + Item.Amount - Item.MaxStack;

            if (amountToAdd > itemToAdd.Amount)
                amountToAdd = itemToAdd.Amount;

            if (amountToAdd + Item.Amount > Item.MaxStack)
                amountToAdd = Item.MaxStack - Item.Amount;

            itemToAdd.Amount -= amountToAdd;
            Item.Amount += amountToAdd;

            return remaining;
        }

        public void SetItem(Item item) => Item = item;
    }
}
