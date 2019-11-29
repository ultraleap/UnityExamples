using UnityEditor;
using UnityEngine;

namespace UltrahapticsCoreAsset.Editor
{
    [CustomPropertyDrawer(typeof(HapticTriggerRegion.EditorHelpBoxAttribute))]
    public class HapticTriggerRegionEditor : PropertyDrawer
    {
        public HapticTriggerRegionEditor()
        {
            editor_ = EditorImpl.Instance;
        }
        public HapticTriggerRegionEditor(IEditor editor)
        {
            editor_ = editor;
        }

        IEditor editor_;
        const float Margin = 2f;
        float size_ = EditorGUIUtility.singleLineHeight * 2;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (size_ * 2) + Margin;
        }

        public override void OnGUI(Rect p, SerializedProperty property, GUIContent label)
        {
            var target = (Component)property.serializedObject.targetObject;

            var r1 = new Rect(p.x, p.y, p.width, size_);
            var r2 = new Rect(p.x, r1.yMax + Margin, p.width, size_);

            var hasCollider = target.GetComponent<Collider>();
            if (hasCollider)
            {
                editor_.HelpBox(r1, "Found collider: " + hasCollider, MessageType.Info);
            }
            else
            {
                editor_.HelpBox(r1, "No collider", MessageType.Error);
            }

            var mapperExists = Object.FindObjectOfType<AutoMapper>();
            if (mapperExists)
            {
                editor_.HelpBox(r2, "Mapper exists", MessageType.Info);
            }
            else
            {
                editor_.HelpBox(r2, "No AutoMapper found", MessageType.Warning);
            }
        }
    }
}
