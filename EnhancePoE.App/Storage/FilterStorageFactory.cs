using EnhancePoE.UI.Model.Storage;

namespace EnhancePoE.App.Storage

{
    public static class FilterStorageFactory
    {
        public static IFilterStorage Create(string lootFilterFilePath)
        {
            return new FileFilterStorage(lootFilterFilePath);
        }
    }
}