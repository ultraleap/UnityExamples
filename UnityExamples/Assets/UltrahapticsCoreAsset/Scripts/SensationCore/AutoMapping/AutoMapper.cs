using System;
using System.Collections.Generic;
using System.Linq;

namespace UltrahapticsCoreAsset
{
    public class AutoMapper : IAutoMapper
    {
        private readonly List<IDataSource> sources_ = new List<IDataSource>();
        private readonly List<SensationBlockInput> inputs_ = new List<SensationBlockInput>();

        public override void RegisterDataSource(IDataSource dataSource)
        {
            sources_.Add(dataSource);
        }

        public override void RegisterBlockInput(SensationBlockInput blockInput)
        {
            inputs_.Add(blockInput);
        }

        public override void DeregisterDataSource(IDataSource dataSource)
        {
            sources_.Remove(dataSource);
        }

        public override void DeregisterBlockInput(SensationBlockInput blockInput)
        {
            inputs_.Remove(blockInput);
        }

        public void Update()
        {
            foreach (var source in sources_)
            {
                var availableDataItems = source.GetAvailableDataItemsForType<UnityEngine.Vector3>();
                foreach (var input in inputs_)
                {
                    if (availableDataItems.Contains(input.Name))
                    {
                        input.Value = source.GetDataItemByName<UnityEngine.Vector3>(input.Name);
                    }
                }
            }
        }

        public override bool HasValueForInputName(string input)
        {
            foreach (var source in sources_)
            {
                var availableDataItems = source.GetAvailableDataItemsForType<UnityEngine.Vector3>();
                if (availableDataItems.Contains(input))
                {
                    return true;
                }
            }
            return false;
        }

        public override UnityEngine.Vector3 GetValueForInputName(string input)
        {
            foreach (var source in sources_)
            {
                var availableDataItems = source.GetAvailableDataItemsForType<UnityEngine.Vector3>();
                if (availableDataItems.Contains(input))
                {
                    return source.GetDataItemByName<UnityEngine.Vector3>(input);
                }
            }
            UCA.Logger.LogError("AutoMapper could not find value for input : " + input);
            return new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }
    }
}
