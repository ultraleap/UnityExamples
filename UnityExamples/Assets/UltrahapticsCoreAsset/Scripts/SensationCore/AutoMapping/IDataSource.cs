using UnityEngine;

namespace UltrahapticsCoreAsset
{
    public interface IDataSource
    {
        string[] GetAvailableDataItemsForType<T>();
        T GetDataItemByName<T>(string name);
    }

    public static class IDataSourceExtensions
    {
        public static void RegisterToAutoMapper(this IDataSource dataSource)
        {
            var autoMapper = GameObject.FindObjectOfType<AutoMapper>();
            if (autoMapper)
            {
                autoMapper.RegisterDataSource(dataSource);
            }
            else
            {
                UCA.Logger.LogWarning("No AutoMapper found to add data source to, is there an AutoMapper in the scene?");
            }
        }

        public static void LogWarningForDuplicateDataSources(this IDataSource dataSource, string dataSourceName)
        {
            UCA.Logger.LogWarning("There is more than one " + dataSourceName + " in the scene. Please ensure there is only one " + dataSourceName + " in the scene.");
        }
    }
}
