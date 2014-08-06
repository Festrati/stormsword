using UnityEngine;
using UnityEditor;
using System.Collections;

/* spritePostProcessor - automagically post-process our sprites to save time and headaches! */
public class spritePostProcessor : AssetPostprocessor {

	private int textureSize = 32;	// Size (in pixels) of tiles we're working with (default: 32x32px)

	/* OnPreprocessTexture - Used to force settings on sprites when imported
	 * 	Sprites are created from a base 'texture' (.png or .jpg) */
	void OnPreprocessTexture() {
		TextureImporter texture = assetImporter as TextureImporter;

		// Apply our default settings
		texture.filterMode =  FilterMode.Point;	// Reduces blurriness and lines between sprites
		texture.spritePixelsToUnits = 32;		// Currently working with 32x32px textures
	}
}
