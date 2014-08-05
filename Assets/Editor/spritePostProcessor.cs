using UnityEngine;
using UnityEditor;
using System.Collections;

/* spritePostProcessor - automagically post-process our sprites to save time and headaches! */
public class spritePostProcessor : AssetPostprocessor {

	// Verify deleting sprites
	static private string deleteTitle = "Warning: Sprites were deleted";
	static private string deleteMessage = "Do you want to delete their Prefabs as well?";
	static private string deleteOk = "Delete";

	/* OnPostprocessAllAssets - triggered any time an asset is updated/added */
	static void OnPostprocessAllAssets (string[] importedAssets,
		string[] deletedAssets,
		string[] movedAssets,
		string[] movedFromAssetPaths) {

		if(importedAssets.Length > 0) {
			// A new asset has been imported
			foreach(string asset in importedAssets) {
				Debug.Log ("Imported Asset: " + asset);
			}
		}

		if(movedAssets.Length > 0) {
			// An asset has been renamed/moved
			foreach(string asset in movedAssets) {
				Debug.Log ("Moved Asset: " + asset);
			}
		}

		if(deletedAssets.Length > 0) {
			
			// Prompt artist to confirm deletion of prefab
			if(EditorUtility.DisplayDialog(deleteTitle, deleteMessage, deleteOk)) {
				// An asset has been deleted
				foreach(string asset in deletedAssets) {
					Debug.Log ("Deleting prefab for: " + asset);		
				}
			}
		}
	}
}
