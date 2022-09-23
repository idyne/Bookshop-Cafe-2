
namespace FateGames
{
    public interface IItemStack
    {
        public Stackable Pop();
        public void Push(Stackable newItem, bool audio = false, bool overrideWave = false);

        public bool Transfer(IItemStack otherStack, bool audio = false, bool overrideWave = false);

        public bool Transfer(IItemStack otherStack, out Stackable item, bool audio = false, bool overrideWave = false);
        public bool IsTagValid(string stackableTag);

        public bool CanPush();

        public void Clear();

    }

}
