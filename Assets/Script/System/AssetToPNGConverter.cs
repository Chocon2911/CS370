using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteToPNGConverter : MonoBehaviour
{
    public Sprite spriteAsset;              
    public string savePath = "Assets/ExportedSprite.png";
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteToPNGConverter))]
public class SpriteToPNGConverterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpriteToPNGConverter script = (SpriteToPNGConverter)target;

        if (GUILayout.Button("Convert Sprite to PNG"))
        {
            if (script.spriteAsset == null)
            {
                Debug.LogError("Sprite is null");
                return;
            }

            Texture2D texture = script.spriteAsset.texture;
            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
                Debug.Log("Texture importer set to readable and reimported.");
            }

            Rect spriteRect = script.spriteAsset.textureRect;

            Texture2D newTexture = new Texture2D((int)spriteRect.width, (int)spriteRect.height, texture.format, false);

            Color[] pixels = texture.GetPixels(
                (int)spriteRect.x,
                (int)spriteRect.y,
                (int)spriteRect.width,
                (int)spriteRect.height);

            newTexture.SetPixels(pixels);
            newTexture.Apply();

            byte[] pngData = newTexture.EncodeToPNG();
            if (pngData == null)
            {
                Debug.LogError("Encode failed");
                return;
            }

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), script.savePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllBytes(fullPath, pngData);

            Debug.Log("Saved sprite PNG to: " + fullPath);
            AssetDatabase.Refresh();
        }
    }
}
#endif
