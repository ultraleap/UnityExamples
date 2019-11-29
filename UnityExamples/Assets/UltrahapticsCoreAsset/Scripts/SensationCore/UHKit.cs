using System.Collections;
using System.Collections.Generic;

namespace UltrahapticsCoreAsset
{
    public static class UHKit
    {
        public enum Model
        {
            STRATOSExplore,
            STRATOSInspire
        };

        public enum Zone
        {
            Poor,
            Ok,
            Good,
            Best
        }

        public static Dictionary<Model, UnityEngine.Vector3> TrackingOrigins =
            new Dictionary<Model, UnityEngine.Vector3>
            {
                {Model.STRATOSExplore, new UnityEngine.Vector3(0, 0, 0.121f)},
                {Model.STRATOSInspire, new UnityEngine.Vector3(0, -0.00006f, -0.089f)}
            };

        public static Dictionary<Model, UnityEngine.Vector3> ModelScales =
            new Dictionary<Model, UnityEngine.Vector3>
            {
                {Model.STRATOSExplore, new UnityEngine.Vector3(0.168f, 0.01f, 0.168f)},
                {Model.STRATOSInspire, new UnityEngine.Vector3(0.311f, 0.01f, 0.11f)}
            };

        public static Dictionary<Model, Dictionary<Zone, UnityEngine.Vector3>> InteractionZones = 
        new Dictionary<Model, Dictionary<Zone, UnityEngine.Vector3>>
        {
            {Model.STRATOSExplore, new Dictionary<Zone, UnityEngine.Vector3> 
                {
                    {Zone.Poor, new UnityEngine.Vector3(0.7f, 0.85f, 0.7f)},
                    {Zone.Ok, new UnityEngine.Vector3(0.6f, 0.7f, 0.6f)},
                    {Zone.Good, new UnityEngine.Vector3(0.5f, 0.6f, 0.5f)},
                    {Zone.Best, new UnityEngine.Vector3(0.4f, 0.45f, 0.4f)}
                }},
            {Model.STRATOSInspire, new Dictionary<Zone, UnityEngine.Vector3> 
                {
                    {Zone.Poor, new UnityEngine.Vector3(0.7f, 0.85f, 0.7f)},
                    {Zone.Ok, new UnityEngine.Vector3(0.6f, 0.7f, 0.55f)},
                    {Zone.Good, new UnityEngine.Vector3(0.5f, 0.6f, 0.45f)},
                    {Zone.Best, new UnityEngine.Vector3(0.4f, 0.45f, 0.35f)}
                }},   
        };
        
        public static Dictionary<Zone, UnityEngine.Color> ZoneColours = 
        new Dictionary<Zone, UnityEngine.Color>
        {
            {Zone.Poor, new UnityEngine.Color(1.0f, 0, 0)},
            {Zone.Ok, new UnityEngine.Color(1.0f, 0.5f, 0)},
            {Zone.Good, new UnityEngine.Color(1.0f, 1.0f, 0)},
            {Zone.Best, new UnityEngine.Color(0.0f, 1.0f, 0)}  
        };
    }
}
