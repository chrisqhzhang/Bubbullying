using MKColorManager;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace MKColorManagerEditor
{
    [CreateAssetMenu(menuName ="ColorManager/Settings", fileName ="ManagedColors")]
    public class ColorManagerSettings : ScriptableObject
    {
        [SerializeField]
        private List<ColorCategory> _categories = new List<ColorCategory>();
        public IReadOnlyList<ColorCategory> Categories => _categories;

        public static ColorManagerSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ColorManagerSettings>(ColorManagerProvider.ColorManagerPath);
            if (settings == null)
            {
                settings = CreateInstance<ColorManagerSettings>();
                settings._categories = new List<ColorCategory>();
                AssetDatabase.CreateAsset(settings, ColorManagerProvider.ColorManagerPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    public class ColorManagerProvider : SettingsProvider
    {
        private SerializedObject _colorSettings;

        class Styles { public static GUIContent colors = new GUIContent("Colors"); }

        public const string ColorManagerPath = "Assets/ColorManager/Editor/ManagedColors.asset";
        public const string ProjectSettingsPath = "Project/Color Manager";
        public ColorManagerProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

        public static bool IsSettingsAvailable() => File.Exists(ColorManagerPath);

        private const float catWidth = 75f;
        private const float colWidth = 75f;

        private static string _newCategory;
        private static Color _newColor;

        private SerializedProperty _categories;

        private ReorderableList list;
        private static bool _attemptButAlreadyExists;

        public override void OnActivate(string searchContext, VisualElement rootElement) 
        { 
            _colorSettings = ColorManagerSettings.GetSerializedSettings();

            _categories = _colorSettings.FindProperty("_categories");
            list = new ReorderableList(_colorSettings, _categories, false, true, false, true);
            list.drawElementCallback = DrawListItems;
            list.drawHeaderCallback = DrawHeader;
            _newCategory = string.Empty;
            _newColor = Color.white;
        }       

        public override void OnGUI(string searchContext)
        {
            _colorSettings.Update();

            list.DoLayoutList();

            EditorGUILayout.LabelField("New category:");

            EditorGUILayout.BeginHorizontal();

            _newCategory = EditorGUILayout.TextField(_newCategory, GUILayout.MinWidth(catWidth)).Trim();
            _newColor = EditorGUILayout.ColorField(_newColor, GUILayout.Width(colWidth));

            if (_newCategory.Length > 0 && GUILayout.Button("Add new category"))
            {
                _attemptButAlreadyExists = false;
                for (int i = 0; i < _categories.arraySize; i++)
                {
                    if (string.Equals(_categories.GetArrayElementAtIndex(i).FindPropertyRelative("_category").stringValue, _newCategory))
                    {
                        _attemptButAlreadyExists = true;
                    }
                }

                if (!_attemptButAlreadyExists)
                {
                    _categories.InsertArrayElementAtIndex(_categories.arraySize);
                    var newElt = _categories.GetArrayElementAtIndex(_categories.arraySize - 1);
                    newElt.FindPropertyRelative("_category").stringValue = _newCategory;
                    newElt.FindPropertyRelative("_color").colorValue = _newColor;

                    _newCategory = string.Empty;
                    _newColor = Color.white;
                }
            }

            EditorGUILayout.EndHorizontal();

            if (_attemptButAlreadyExists)
            {
                EditorGUILayout.LabelField("Cannot create this category: name already exists.");
            }

            if (_colorSettings.hasModifiedProperties)
            {
                UpdateScene();
            }

            if (GUILayout.Button("Update Scene"))
            {
                UpdateScene();
            }

            _colorSettings.ApplyModifiedProperties();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            UpdateScene();
        }

        private void UpdateScene()
        {
            if (_categories == null)
                return;

            Object.FindObjectsOfType<IColorable>().ToList().ForEach((x) =>
            {
                for (int i = 0; i < _categories.arraySize; i++)
                {
                    SerializedProperty prop = _categories.GetArrayElementAtIndex(i);
                    string category = prop.FindPropertyRelative("_category").stringValue;

                    if (string.Equals(x.Category, category))
                    {
                        Color col = prop.FindPropertyRelative("_color").colorValue;
                        x.SetColor(col);
                        break;
                    }
                }
            });
        }

        private void DrawHeader(Rect rect)
        {
            string name = "Categories";
            EditorGUI.LabelField(rect, name);
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            float categoryProp = 0.50f;
            float colorProp = 0.50f;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.LabelField(
                new Rect(
                    rect.x,
                    rect.y,
                    rect.width * categoryProp,
                    EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_category").stringValue);

            EditorGUI.PropertyField(
                new Rect(
                    rect.x + (rect.width * categoryProp),
                    rect.y,
                    rect.width * colorProp,
                    EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_color"),
                GUIContent.none);
        }

        [SettingsProvider]
        public static SettingsProvider CreateColorManagerProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new ColorManagerProvider(ProjectSettingsPath, SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            else
            {
                Debug.LogWarning($"Color Management asset is not configured. " +
                    $"You can configure it by creating ColorManagerSettings as {ColorManagerPath}");
            }
            return null;
        }
    }
}