using System;
using System.Collections.Generic;
using UnityEngine;

namespace aisilol
{
	public static class GUILayoutUtils
	{
		public static bool SizeButton(GUIContent _content)
		{
			var size = GUI.skin.button.CalcSize(_content);
			return GUILayout.Button(_content, GUILayout.Width(size.x), GUILayout.Height(size.y));
		}
		public static int Grid(int _selected, List<GUIContent> _columns, List<List<GUIContent>> _rows, params GUILayoutOption[] _options)
		{
			for (var i = 0; i < _rows.Count; i++)
			{
				var row = _rows[i];
				if (row.Count == _columns.Count)
					continue;

				Debug.LogWarningFormat("Invalid column count. [Row : {0}]", i);
			}

			var datas = new GUIContent[_columns.Count * (_rows.Count + 1)];
			Array.Copy(_columns.ToArray(), datas, _columns.Count);

			var index = 1;
			foreach (var row in _rows)
			{
				Array.Copy(row.ToArray(), 0, datas, _columns.Count * index++, _columns.Count);
			}

			return GUILayout.SelectionGrid(_selected, datas, _columns.Count, _options);
		}
	}
}