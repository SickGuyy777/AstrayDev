
public abstract class ItemComponent
{
    public Item Item { get; private set; }


    public void SetItem(Item aItem) => this.Item = aItem;

    public abstract ItemComponent Copy();
}
