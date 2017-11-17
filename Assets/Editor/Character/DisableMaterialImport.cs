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
}