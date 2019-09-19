using System;
using System.Collections.Generic;
using UnityEngine;

namespace DBEditor
{
	// [CreateAssetMenu()]
	public class DBEditorConfig : ScriptableObject
	{
		[Serializable]
		public class Config
		{
			[Tooltip("The parent category structure for this files. Use '/' to nest.")]
			public string TreeViewName;
			[Tooltip("The path to the folder to contain newly created objects of this category. 'Assets/...'")]
			public string FileStoragePath = "Assets/...";
			[Tooltip("The ScriptableObject script class name.")]
			[HideInInspector] public string ClassName;
			[Tooltip("This option will auto load all assets of the ClassName to the files list below.")]
			[HideInInspector] public bool AutoScan;
			[Space]
			[Tooltip("The ScriptableObject instances to include manually.")]
			public List<UnityEngine.Object> Files;
		}

		[HideInInspector] public int MaxCategoryId = 10000;
		[Tooltip("The width in pixels reserved for labels in the DB Editor.")]
		[HideInInspector] public float LabelWidth = 220f;

		public List<Config> Configs;
	}
}