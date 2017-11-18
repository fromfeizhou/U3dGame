using UnityEngine;
using System.Collections;
using UnityEditor;

/************************************************************************/
/*               fbx自动导入的时候，设置    importMaterials = false                                                    */
/************************************************************************/
class DisableMaterialImport : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        modelImporter.importMaterials = false;
    }

    void OnPostprocessModel(GameObject go)
    {
        if (!assetPath.Contains("/Characters/")) return;

        // Assume an animation FBX has an @ in its name,
        // to determine if an fbx is a character or an animation.
        if (assetPath.Contains("@"))
        {
            // For animation FBX's all unnecessary Objects are removed.
            // This is not required but improves clarity when browsing assets.

            // Remove SkinnedMeshRenderers and their meshes.
            foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                Object.DestroyImmediate(smr.sharedMesh, true);
                Object.DestroyImmediate(smr.gameObject);
            }

            // Remove the bones.
            foreach (Transform o in go.transform)
                Object.DestroyImmediate(o.gameObject);
        }
    }
}