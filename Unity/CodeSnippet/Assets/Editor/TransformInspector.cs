using UnityEngine;
using UnityEditor;

namespace SRTwo
{
	[CustomEditor(typeof(Transform))]
	public class TransformInspector : Editor
	{
		// for Unity
		private void OnEnable()
		{
			mPosition = serializedObject.FindProperty("m_LocalPosition");
			mRotation = serializedObject.FindProperty("m_LocalRotation");
			mScale = serializedObject.FindProperty("m_LocalScale");
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Position
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("P", GUILayout.Width(20)))
				mPosition.vector3Value = Vector3.zero;
			EditorGUILayout.PropertyField(mPosition, POSITION_GUI_CONTENTS);
			EditorGUILayout.EndHorizontal();

			// Rotation
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("R", GUILayout.Width(20)))
				mRotation.quaternionValue = Quaternion.identity;
			rotationPropertyField(mRotation, ROTATION_GUI_CONTENTS);
			EditorGUILayout.EndHorizontal();

			// Scale
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("S", GUILayout.Width(20)))
				mScale.vector3Value = Vector3.one;
			EditorGUILayout.PropertyField(mScale, SCALE_GUI_CONTENTS);
			EditorGUILayout.EndHorizontal();

			// Tools
			if (GUILayout.Button("Reset", GUILayout.Width(50)))
			{
				mPosition.vector3Value = Vector3.zero;
				mRotation.quaternionValue = Quaternion.identity;
				mScale.vector3Value = Vector3.one;
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void rotationPropertyField(SerializedProperty _rotation, GUIContent _contents)
		{
			var transform = targets[0] as Transform;
			var localRotation = transform.localRotation;
			foreach (var target in targets)
			{
				var t = target as Transform;
				if (t.localRotation == localRotation)
					continue;

				EditorGUI.showMixedValue = true;
				break;
			}

			EditorGUI.BeginChangeCheck();
			var eulerAngles = EditorGUILayout.Vector3Field(_contents, localRotation.eulerAngles);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObjects(this.targets, "Rotation Changed");
				foreach (var target in targets)
				{
					var t = target as Transform;
					t.localEulerAngles = eulerAngles;
				}

				mRotation.serializedObject.SetIsDifferentCacheDirty();
			}

			EditorGUI.showMixedValue = false;
		}

		private static GUIContent POSITION_GUI_CONTENTS = new GUIContent("Position");
		private static GUIContent ROTATION_GUI_CONTENTS = new GUIContent("Rotation");
		private static GUIContent SCALE_GUI_CONTENTS = new GUIContent("Scale");

		private SerializedProperty mPosition;
		private SerializedProperty mRotation;
		private SerializedProperty mScale;
	}
}