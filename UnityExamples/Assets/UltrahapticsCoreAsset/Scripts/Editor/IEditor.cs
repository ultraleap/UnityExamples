using UnityEditor;
using UnityEngine;

namespace UltrahapticsCoreAsset.Editor
{
    public interface IEditor
    {
        void HelpBox(Rect position, string message, MessageType type);
        void PropertyField(Rect position, SerializedProperty property, GUIContent label);
    }

    public class EditorImpl : IEditor
    {
        public static EditorImpl Instance { get; private set; }

        static EditorImpl()
        {
            Instance = new EditorImpl();
        }

        private EditorImpl() { }

        public void HelpBox(Rect position, string message, MessageType type)
        {
            EditorGUI.HelpBox(position, message, type);
        }

        public void PropertyField(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
