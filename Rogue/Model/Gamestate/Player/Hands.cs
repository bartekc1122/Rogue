using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Rogue;

public class Hands
{
    [JsonInclude]
    public IItem? Right;
    [JsonInclude]
    public IItem? Left;
    public Hands()
    {
        Right = null;
        Left = null;
    }
    public bool RightEquip(IItem item)
    {
        if (Right != null)
        {
            return false;
        }
        if (item.IsTwoHanded)
        {
            return TwoHandEquip(item);
        }
        Right = item;
        return true;
    }
    public bool LeftEquip(IItem item)
    {
        if (Left != null)
        {
            return false;
        }
        if (item.IsTwoHanded)
        {
            return TwoHandEquip(item);
        }
        Left = item;
        return true;
    }
    private bool TwoHandEquip(IItem item)
    {
        if (Right == null && Left == null)
        {
            Right = item;
            Left = item;
            return true;
        }
        return false;
    }
    public IItem? RightUnequip()
    {
        if (Right == null)
        {
            return null;
        }
        if (Right.IsTwoHanded)
        {

            return TwoHandUnequip();
        }
        var item = Right;
        Right = null;
        return item;
    }
    public IItem? LeftUnequip()
    {
        if (Left == null)
        {
            return null;
        }
        if (Left.IsTwoHanded)
        {
            return TwoHandUnequip();
        }
        var item = Left;
        Left = null;
        return item;
    }
    private IItem? TwoHandUnequip()
    {
        if (Right == Left && Right != null)
        {
            var item = Right;
            Right = null;
            Left = null;
            return item;
        }
        return null;
    }

}