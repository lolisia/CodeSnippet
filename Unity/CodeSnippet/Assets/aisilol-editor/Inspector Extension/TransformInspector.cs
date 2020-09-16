using UnityEditor;
using UnityEngine;

namespace aisilol
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
			rotationPropertyField(ROTATION_GUI_CONTENTS);
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

			using (new GUILayout.VerticalScope(GUI.skin.box))
			{
				using (new GUI_.IndentScope())
				using (new GUILayout.HorizontalScope())
				{
					mFoldOutWorldTransform = EditorGUILayout.Foldout(mFoldOutWorldTransform, "World Transform");
				}

				if (mFoldOutWorldTransform)
				{
					var transform = target as Transform;
					if (transform != null)
					{
						using (new EditorGUI.DisabledScope(true))
						{
							EditorGUILayout.Vector3Field("Position", transform.position);
							EditorGUILayout.Vector3Field("Rotation", transform.rotation.eulerAngles);
							EditorGUILayout.Vector3Field("Scale", transform.lossyScale);
						}
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void rotationPropertyField(GUIContent _contents)
		{
			var transform = targets[0] as Transform;
			if (transform == null)
				return;

			var localRotation = transform.localRotation;
			foreach (var o in targets)
			{
				if (o is Transform t)
				{
					if (t.localRotation == localRotation)
						continue;
				}

				EditorGUI.showMixedValue = true;
				break;
			}

			EditorGUI.BeginChangeCheck();
			var eulerAngles = EditorGUILayout.Vector3Field(_contents, localRotation.eulerAngles);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObjects(targets, "Rotation Changed");
				foreach (var o in targets)
				{
					if (o is Transform t)
						t.localEulerAngles = eulerAngles;
				}

				mRotation.serializedObject.SetIsDifferentCacheDirty();
			}

			EditorGUI.showMixedValue = false;
		}

		private static readonly GUIContent POSITION_GUI_CONTENTS = new GUIContent("Position");
		private static readonly GUIContent ROTATION_GUI_CONTENTS = new GUIContent("Rotation");
		private static readonly GUIContent SCALE_GUI_CONTENTS = new GUIContent("Scale");

		private SerializedProperty mPosition;
		private SerializedProperty mRotation;
		private SerializedProperty mScale;

		private static bool mFoldOutWorldTransform;
    }
}
