using UnityEngine;
using UnityEditor;
using System.Collections;

/* spritePostProcessor - automagically post-process our sprites to save time and headaches! */
public class spritePostProcessor : AssetPostprocessor {

	// Sprite import settings
	private int tileSize = 32;	// Size (in pixels) of tiles we're working with (default: 32x32px)

	// Verify deleting sprites
	static private string deleteTitle = "Warning: Sprites were deleted";
	static private string deleteMessage = "Do you want to delete their Prefabs as well?";
	static private string deleteOk = "Delete";
	
	/* OnPostprocessAllAssets - triggered any time an asset is updated/added */
//	static void OnPostprocessAllAssets (string[] importedAssets,
//	                                    string[] deletedAssets,
//	                                    string[] movedAssets,
//	                                    string[] movedFromAssetPaths) {
//		
//		if(importedAssets.Length > 0) {
//			// A new asset has been imported
//			foreach(string asset in importedAssets) {
//				if(asset.EndsWith(".png")) {
//					// Only process sprites
//					//					Debug.Log ("Imported Sprite: " + asset);
//					// Set proper sprite info
//
////					Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(asset) as Sprite[];
//					Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(asset);
//
//					// Get individual 
//					foreach(Object spriteObject in sprites) {
//						Sprite sprite = spriteObject as Sprite;
////						Debug.Log (sprite);
//					}
//					//					sprite.Texture
//					//						LoadAssetAtPath(asset) as GameObject;
//					//					sprite.G
//					
//					//					Debug.Log (sprite.texture.);
//				}
//			}
//		}
//		
//		if(movedAssets.Length > 0) {
//			// An asset has been renamed/moved
//			for(int i =0; i < movedAssets.Length; i++) {
//				Debug.Log ("Moved Asset: " + movedAssets[i] + " from " + movedFromAssetPaths[i]);
//				
//			}
//		}
//		
//		if(deletedAssets.Length > 0) {
//			bool containsSprite = false;
//			foreach(string asset in deletedAssets) {
//				if(asset.EndsWith(".png")) {
//					containsSprite = true;
//					break;
//				}
//			}
//			
//			// Prompt artist to confirm deletion of prefab
//			if(EditorUtility.DisplayDialog(deleteTitle, deleteMessage, deleteOk)) {
//				// An asset has been deleted
//				foreach(string asset in deletedAssets) {
//					Debug.Log ("Deleting prefab for: " + asset);		
//				}
//			}
//		}
//	}

	/* OnPreprocessTexture - Used to force settings on sprites when imported
	 * 	Sprites are created from a base 'texture' (.png or .jpg) */
	void OnPreprocessTexture() {
		TextureImporter importer = assetImporter as TextureImporter;

		// Apply our default settings
		importer.filterMode =  FilterMode.Point;	// Reduces blurriness and lines between sprites
		importer.spritePixelsToUnits = tileSize;		// Currently working with 32x32px textures - this causes them to be sized properly in the game editor
		importer.spriteImportMode = SpriteImportMode.Multiple;

	}

	void OnPostprocessTexture(Texture2D texture) {
		// Basically have to re-create their grid-slicer :(
		TextureImporter importer = assetImporter as TextureImporter;
		
		int tilesAcross = texture.width / tileSize;
		int tilesDown = texture.height / tileSize;
		
		int numTiles = tilesAcross * tilesDown;
		
		SpriteMetaData[] newMetaData = new SpriteMetaData[numTiles];
		
		int currentTile = 0;	// Current tile we're working with
		
		for(int y = 0; y < tilesDown; y++) {
			for(int x = 0; x < tilesAcross; x++) {
				// Loop through each tile and manually add slice data
				
				// Calculate current 'pivot' (x/y of top-left corner)
				SpriteMetaData meta = new SpriteMetaData();
				meta.alignment = (int)SpriteAlignment.Center;

				string fileName = GetFileName(importer.assetPath);
				meta.name = fileName + "_" + currentTile;	// Follow naming convention: "filename_0"
				
				// Set pivot
				int xPos = x * tileSize;
				int yPos = y * tileSize;
				
				meta.pivot = new Vector2(0, 0);
				
				meta.border = new Vector4(0, 0);
				
				// Create rect around current position
				meta.rect = new Rect(xPos, yPos, tileSize, tileSize);	// Assumes we're working with square tiles :X

				newMetaData[currentTile] = meta;	// Push it onto the array

				currentTile++;
			}
		}
		
		importer.spritesheet = newMetaData;	// Throw the new mapping into the file
	}

	/* GetFileName - gets the filanem (without extension) from a path string */
	private string GetFileName(string assetPath) {
		string[] explodedFileName = assetPath.Split ('/'); // Parse out directory names
		string fileName = explodedFileName[explodedFileName.Length-1];	// filename will be the last item in the array

		fileName = fileName.Remove(fileName.Length - 4);

		return(fileName);
	}
}
