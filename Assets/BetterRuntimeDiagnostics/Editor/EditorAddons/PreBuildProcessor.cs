using Better.Diagnostics.Runtime.DrawingModule.Framing.Models;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Better.Diagnostics.EditorAddons
{
    public class PreBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; } = -999;

        private readonly string[] _shadersToInclude = new[]
        {
            RenderStrategy.ShaderName
        };
        
        public void OnPreprocessBuild(BuildReport report)
        {
            foreach (var shader in _shadersToInclude)
            {
                AddAlwaysIncludedShader(shader);
            }
        }

        private static void AddAlwaysIncludedShader(string shaderName)
        {
            var shader = Shader.Find(shaderName);
            if (shader == null)
                return;
 
            var graphicsSettingsObj = AssetDatabase.LoadAssetAtPath<GraphicsSettings>("ProjectSettings/GraphicsSettings.asset");
            var serializedObject = new SerializedObject(graphicsSettingsObj);
            var arrayProp = serializedObject.FindProperty("m_AlwaysIncludedShaders");
            var hasShader = false;
            for (var i = 0; i < arrayProp.arraySize; ++i)
            {
                var arrayElem = arrayProp.GetArrayElementAtIndex(i);
                if (shader == arrayElem.objectReferenceValue)
                {
                    hasShader = true;
                    break;
                }
            }

            if (hasShader) return;
            var arrayIndex = arrayProp.arraySize;
            arrayProp.InsertArrayElementAtIndex(arrayIndex);
            var serializedProperty = arrayProp.GetArrayElementAtIndex(arrayIndex);
            serializedProperty.objectReferenceValue = shader;

            serializedObject.ApplyModifiedProperties();

            AssetDatabase.SaveAssets();
        }
    }
}
