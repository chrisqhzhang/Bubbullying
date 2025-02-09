using MKColorManager;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

namespace MKColorManagerEditor
{
    [CustomEditor(typeof(Colorable))]
    public class ColorableEditor : Editor
    {
        private int _lastChoiceIndex;
        private int _choiceIndex;
        private SerializedProperty _category;
        
        private static IReadOnlyList<ColorCategory> _categories;

        private void OnEnable()
        {
            _category = serializedObject.FindProperty("_category");;

            ReInit();
        }

        private void ReInit()
        {
            try
            {
                _categories = AssetDatabase
                    .LoadAssetAtPath<ColorManagerSettings>(ColorManagerProvider.ColorManagerPath)
                    .Categories;
            }
            catch (Exception)
            {
                Debug.LogWarning($"Failed to load ColorManagerSettings @{ColorManagerProvider.ColorManagerPath}");
                _categories = new List<ColorCategory>();
            }

            try
            {
                _choiceIndex = _categories
                    .ToList()
                    .IndexOf(_categories.Single(x => x.Category == _category.stringValue));
            }
            catch (Exception)
            {
                _choiceIndex = 0;
            }
        }

        public override void OnInspectorGUI()
        {
            if (_categories == null || _categories.Count == 0)
            {
                ReInit();
                return;
            }

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "_category");

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Category");

            EditorGUILayout.BeginHorizontal();

            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _categories.Select(x => x.Category).ToArray());

            _category.stringValue = _categories[_choiceIndex].Category;

            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.ColorField(_categories[_choiceIndex].Color);

            if(_lastChoiceIndex != _choiceIndex)
            {
                Apply(_categories[_choiceIndex].Color);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            _lastChoiceIndex = _choiceIndex;

            serializedObject.ApplyModifiedProperties();
        }

        private void Apply(Color color)
        {
            Colorable cm = target as Colorable;
            cm.SetColor(color);
        }
    }
}
