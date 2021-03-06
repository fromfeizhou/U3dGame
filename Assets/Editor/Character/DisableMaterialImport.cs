﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/************************************************************************/
/*               fbx自动导入的时候，设置    importMaterials = false                                                    */
/************************************************************************/
class DisableMaterialImport : AssetPostprocessor
{
    //void OnPreprocessModel()
    //{
    //    ModelImporter modelImporter = assetImporter as ModelImporter;
    //    modelImporter.importMaterials = false;
    //}

    //void OnPostprocessModel(GameObject go)
    //{
    //    if (!assetPath.Contains("/Characters/")) return;

    //    // Assume an animation FBX has an @ in its name,
    //    // to determine if an fbx is a character or an animation.
    //    if (assetPath.Contains("@"))
    //    {
    //        // For animation FBX's all unnecessary Objects are removed.
    //        // This is not required but improves clarity when browsing assets.

    //        // Remove SkinnedMeshRenderers and their meshes.
    //        foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
    //        {
    //            Object.DestroyImmediate(smr.sharedMesh, true);
    //            Object.DestroyImmediate(smr.gameObject);
    //        }

    //        // Remove the bones.
    //        foreach (Transform o in go.transform)
    //            Object.DestroyImmediate(o.gameObject);
    //    }
    //}

    public void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        modelImporter.animationType = ModelImporterAnimationType.Legacy;
        try
        {
            string fileAnim;
            if (DragAndDrop.paths.Length <= 0)
            {
                return;
            }
            fileAnim = DragAndDrop.paths[0];
            string ClipText = Path.ChangeExtension(fileAnim, ".txt");
            StreamReader file = new StreamReader(ClipText);
            string sAnimList = file.ReadToEnd();
            file.Close();
            //  
            if (EditorUtility.DisplayDialog("FBX Animation Import from file",
                fileAnim, "Import", "Cancel"))
            {
                System.Collections.ArrayList List = new ArrayList();
                ParseAnimFile(sAnimList, ref List);

                //modelImporter.clipAnimations. = true;  
                modelImporter.clipAnimations = (ModelImporterClipAnimation[])
                    List.ToArray(typeof(ModelImporterClipAnimation));

                EditorUtility.DisplayDialog("Imported animations",
                    "Number of imported clips: "
                    + modelImporter.clipAnimations.GetLength(0).ToString(), "OK");
            }
        }
        catch { }
        // (Exception e) { EditorUtility.DisplayDialog("Imported animations", e.Message, "OK"); }  
    }
    void ParseAnimFile(string sAnimList, ref System.Collections.ArrayList List)
    {
        string[] lines = sAnimList.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++)
        {
            string strLine = lines[i];
            if (strLine != "" && strLine.Contains("import"))
            {
                string[] keyValue = strLine.Replace("_import", "").Split(' ');
                if (keyValue.Length >= 3)
                {
                    ModelImporterClipAnimation clip = new ModelImporterClipAnimation();
                    clip.name = keyValue[0];
                    clip.firstFrame = int.Parse(keyValue[1]);
                    clip.lastFrame = int.Parse(keyValue[2]);
                    clip.loop = true;
                    clip.wrapMode = WrapMode.Loop;
                    List.Add(clip);
                }
            }
        }
    }

}