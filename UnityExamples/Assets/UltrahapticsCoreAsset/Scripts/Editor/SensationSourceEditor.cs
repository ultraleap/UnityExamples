using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace UltrahapticsCoreAsset.Editor
{
    [CustomEditor (typeof(SensationSource))]
    public class SensationSourceEditor : UnityEditor.Editor
    {
        private int index_ = -1;
        private bool sensationHasChanged_ = false;
        internal List<string> sensations_ = new List<string>{};
        private  Dictionary<string, bool> inputGroupsExpandedState_ = new Dictionary<string, bool>();

        private SerializedProperty sensationBlock_;
        private SerializedProperty running_;
        private SerializedProperty priority_;
        private SerializedProperty transform_;
        private SerializedProperty sensationBlockInputs_;
        private SerializedProperty supportsTransform_;
        private SerializedProperty inputs_;

        private SerializedProperty SensationBlock { get { return sensationBlock_ ?? (sensationBlock_ = serializedObject.FindProperty(ReflectionProperties.SensationSource.sensationBlock_));}}
        private SerializedProperty Running { get { return running_ ?? (running_ = serializedObject.FindProperty(ReflectionProperties.SensationSource.running_));}}
        private SerializedProperty Priority { get { return priority_ ?? (priority_ = serializedObject.FindProperty(ReflectionProperties.SensationSource.priority_));}}
        private SerializedProperty SensationBlockInputs { get { return sensationBlockInputs_ ?? (sensationBlockInputs_ = serializedObject.FindProperty(ReflectionProperties.SensationSource.Inputs));}}

        private SerializedProperty Inputs { get { return inputs_ ?? (inputs_ = SensationBlockInputs.FindPropertyRelative(ReflectionProperties.SensationBlockInputs.inputs_));}}
        private SerializedProperty Transform { get { return transform_ ?? (transform_ = SensationBlockInputs.FindPropertyRelative(ReflectionProperties.SensationBlockInputs.TrackingObject));}}
        private SerializedProperty SupportsTransform { get { return supportsTransform_ ?? (supportsTransform_ = SensationBlockInputs.FindPropertyRelative(ReflectionProperties.SensationBlockInputs.supportsTransform_));}}

        public override void OnInspectorGUI()
        {
            if (SensationCore.Instance == null)
            {
                EditorGUILayout.HelpBox("Sensation Source requires a Sensation Core component to edit its properties! Please add one to your scene or include an UltrahapticsKit Prefab.", MessageType.Error);
                return;
            }
            {
                var sensationSourceTarget = ((SensationSource)target);
                if (sensationSourceTarget.requiresReloadingListOfSensations_ || !SelectedIndexIsInitialised())
                {
                    UpdateListOfSensationProducingBlocks();
                    sensationSourceTarget.requiresReloadingListOfSensations_ = false;
                }
            }
            serializedObject.Update();

            AddDropDownList(SensationBlock);
            EditorGUILayout.PropertyField(Running, new GUIContent("Running", Tooltips.SensationSource.Running));
            EditorGUILayout.PropertyField(Priority, new GUIContent("Priority", Tooltips.SensationSource.Priority));

            if (SupportsTransform.boolValue)
            {
                Transform.objectReferenceValue =
                    EditorGUILayout.ObjectField("Transform", Transform.objectReferenceValue,
                        typeof(UnityEngine.Transform), true) as UnityEngine.Transform;
            }

            if (SensationHasBeenSelected())
            {
                AddInputsSection();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddInputsSection()
        {
            AddInputsHeader();

            InputGroups inputGroups = GetInputGroupsOfCurrentSensation();
            resetInputGroupsExpandedState(inputGroups);

            foreach (var inputGroup in inputGroups)
            {
                AddInputGroup(inputGroup);
            }
        }

        private InputGroups GetInputGroupsOfCurrentSensation()
        {
            serializedObject.ApplyModifiedProperties();
            var sensationSource = SensationBlockInputs.serializedObject.targetObject as SensationSource;
            return sensationSource.Inputs.Groups();
        }

        private void AddInputGroup(InputGroup inputGroup)
        {
            EditorGUILayout.BeginVertical();
            var tooltip = "";
            if (inputGroup.Name == "Hidden Inputs")
            {
                tooltip = Tooltips.SensationSource.HiddenInputs;
            }

            if (inputGroup.Name == InputGroups.UncategorizedGroupId)
            {
                foreach(var input in inputGroup)
                {
                    AddInput(input);
                }
            }
            else
            {
                var initialFoldOutExpandedState = inputGroupsExpandedState_[inputGroup.Name];
                var newFoldOutExpandedState = EditorGUILayout.Foldout(initialFoldOutExpandedState, new GUIContent(inputGroup.Name, tooltip), true);
                inputGroupsExpandedState_[inputGroup.Name] = newFoldOutExpandedState;

                EditorGUI.indentLevel++;
                if (inputGroupsExpandedState_[inputGroup.Name])
                {
                    foreach(var input in inputGroup)
                    {
                        AddInput(input);
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void resetInputGroupsExpandedState(InputGroups groups)
        {
            if (sensationHasChanged_ || inputGroupsExpandedState_.Count == 0)
            {
                inputGroupsExpandedState_.Clear();
                foreach (var group in groups)
                {
                    inputGroupsExpandedState_[group.Name] = false;
                }
            }
        }

        private void AddDropDownList(SerializedProperty currentSelection)
        {
            index_ = sensations_.FindIndex(x => x == currentSelection.stringValue);
            if (!SelectedIndexIsInitialised())
            {
                index_ = 0;
            }

            index_ = EditorGUILayout.Popup(new GUIContent("Sensation Block", Tooltips.SensationSource.SensationBlock), index_, sensations_.ToArray());
            if (currentSelection.stringValue != sensations_[index_])
            {
                currentSelection.stringValue = sensations_[index_];
                sensationHasChanged_ = true;
            }
            else
            {
                sensationHasChanged_ = false;
            }
        }

        private bool SelectedIndexIsInitialised()
        {
            return index_ != -1;
        }

        private static void AddInputsHeader()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Inputs", EditorStyles.boldLabel);
        }

        private void AddInput(SensationBlockInput input)
        {
            switch (input.Type)
            {
                case "Scalar":
                    var newScalarValue = EditorGUILayout.FloatField(input.Name, input.Value.x);
                    if (newScalarValue != input.Value.x)
                    {
                        var newScalarValueAsVector3 = new UnityEngine.Vector3(newScalarValue, 0, 0);
                        updateSerializedInput(input.Name, newScalarValueAsVector3);
                        input.Value = newScalarValueAsVector3;
                    }
                    break;
                case "Point":
                    var newObjectValue = EditorGUILayout.ObjectField(input.Name, input.Object, typeof(UnityEngine.Transform), true) as UnityEngine.Transform;
                    if (newObjectValue != input.Object)
                    {
                        updateSerializedObjectInput(input.Name, newObjectValue);
                        input.Object = newObjectValue;
                    }
                    break;
                default:
                    var newVector3Value = EditorGUILayout.Vector3Field(input.Name, input.Value);
                    if (input.Value != newVector3Value)
                    {
                        updateSerializedInput(input.Name, newVector3Value);
                        input.Value = newVector3Value;
                    }
                    break;
            }
        }

        void updateSerializedInput(string inputName, UnityEngine.Vector3 value)
        {
            var serializedInputs = Enumerable.Range(0, Inputs.arraySize).Select(Inputs.GetArrayElementAtIndex);
            var serializedInput = serializedInputs.First(i => i.FindPropertyRelative("Name").stringValue == inputName);

            serializedInput.FindPropertyRelative("Value").vector3Value = value;
            serializedObject.ApplyModifiedProperties();
        }

        void updateSerializedObjectInput(string inputName, UnityEngine.Object value)
        {
            var serializedInputs = Enumerable.Range(0, Inputs.arraySize).Select(Inputs.GetArrayElementAtIndex);
            var serializedInput = serializedInputs.First(i => i.FindPropertyRelative("Name").stringValue == inputName);

            serializedInput.FindPropertyRelative("Object").objectReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
        }

        private bool SensationHasBeenSelected()
        {
            return index_ > 0;
        }

        public void UpdateListOfSensationProducingBlocks()
        {
            sensations_.Clear();
            sensations_.AddRange(SensationCore.Instance.GetSensationProducingBlockNames());
            sensations_.Sort();
            sensations_.Insert(0, "None");
        }
    }
}
