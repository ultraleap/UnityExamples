using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UltrahapticsCoreAsset
{
    public class UnityToEmitterSpace
    {
        public static Matrix4x4 Transform
        {
            get
            {
                var transform = Matrix4x4.identity;
                var y = transform.GetColumn(1);
                var z = transform.GetColumn(2);
                transform.SetColumn(1, z);
                transform.SetColumn(2, y);
                return transform;
            }
        }
    }

}
