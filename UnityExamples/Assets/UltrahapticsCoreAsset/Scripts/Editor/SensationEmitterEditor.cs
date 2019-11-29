using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace UltrahapticsCoreAsset.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SensationEmitter))]
    public class SensationEmitterEditor : UnityEditor.Editor
    {
        private ReorderableList listOfActiveSensationSources_;

        private SerializedProperty allowMockEmitter_;
        private SerializedProperty logEmissionToFile_;
        private SerializedProperty arrayTransform_;
        private SerializedProperty sphereRadius_;
        private SerializedProperty sphereColor_;
        private SerializedProperty historySize_;
        private SerializedProperty currentSensation_;
        private SerializedProperty registeredSources_;

        private SerializedProperty AllowMockEmitter { get { return allowMockEmitter_ ?? (allowMockEmitter_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.AllowMockEmitter));}}
        private SerializedProperty LogEmissionToFile { get { return logEmissionToFile_ ?? (logEmissionToFile_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.LogEmissionToFile));}}
        private SerializedProperty ArrayTransform { get { return arrayTransform_ ?? (arrayTransform_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.ArrayTransform));}}
        private SerializedProperty SphereRadius { get { return sphereRadius_ ?? (sphereRadius_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.SphereRadius));}}
        private SerializedProperty SphereColor { get { return sphereColor_ ?? (sphereColor_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.SphereColor));}}
        private SerializedProperty HistorySize { get { return historySize_ ?? (historySize_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.HistorySize));}}
        private SerializedProperty CurrentSensation { get { return currentSensation_ ?? (currentSensation_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.CurrentSensation));}}
        private SerializedProperty RegisteredSources { get { return registeredSources_ ?? (registeredSources_ = serializedObject.FindProperty(ReflectionProperties.SensationEmitter.registeredSources_));}}

        private static void AssignHeaderCallback(ReorderableList list)
        {
            list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Active Sensation Sources"); };
        }
        private static void AssignElementCallback(ReorderableList list)
        {
            list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = (SensationSource)(list.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue);
                if (element == null)
                {
                    return;
                }

                rect.y += 2;
                element.Running = EditorGUI.Toggle(
                    new Rect(rect.x, rect.y, rect.width * .05f, EditorGUIUtility.singleLineHeight),
                    element.Running);
                EditorGUI.TextField(
                    new Rect(rect.x + (rect.width * .05f), rect.y, rect.width * .85f, EditorGUIUtility.singleLineHeight),
                    element.name);
                element.Priority = Convert.ToUInt32(EditorGUI.IntField(
                    new Rect(rect.x + (rect.width * .9f), rect.y, rect.width * .1f, EditorGUIUtility.singleLineHeight),
                    Convert.ToInt32(element.Priority)));
            };
        }

        public void OnEnable()
        {
            listOfActiveSensationSources_ = new ReorderableList(serializedObject, RegisteredSources, draggable: false, displayHeader: true, displayAddButton: false, displayRemoveButton: false);
            AssignHeaderCallback(listOfActiveSensationSources_);
            AssignElementCallback(listOfActiveSensationSources_);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Hardware Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(AllowMockEmitter, new GUIContent("Allow Mock Emitter", Tooltips.SensationEmitter.AllowMockEmitter));
            EditorGUILayout.PropertyField(LogEmissionToFile, new GUIContent("Log Emissions to file", Tooltips.SensationEmitter.LogEmissionToFile));

            EditorGUILayout.LabelField("Visualization Setting", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(ArrayTransform, new GUIContent("Array Transform", Tooltips.SensationEmitter.ArrayTransform));
            EditorGUILayout.PropertyField(SphereRadius, new GUIContent("Sphere Radius", Tooltips.SensationEmitter.SphereRadius));
            EditorGUILayout.PropertyField(SphereColor, new GUIContent("Sphere Color", Tooltips.SensationEmitter.SphereColor));
            EditorGUILayout.PropertyField(HistorySize, new GUIContent("History Size", Tooltips.SensationEmitter.HistorySize));
            if (CurrentSensation != null)
            {
                EditorGUILayout.PropertyField(CurrentSensation, new GUIContent("Current Sensation", Tooltips.SensationEmitter.CurrentSensation));
            }

            if (EditorApplication.isPlaying)
            {
                listOfActiveSensationSources_.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
