using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;

//合并skinnedMesh步骤  
//1.从Prefab创一个无子mesh模型  
//2.创建 CombineInstances集合（融合实例） | Materials集合（材质） | 创建一个Bone_Transforms结合（骨骼矩阵）用于融合准备  
//3.从Prefab创建各个SkinnedMesh模型  
//4.给对应的SkinnedMesh赋予Material，并将Material加入Materials集合  
//5.根据SkinnedMesh创建CombineInstance，并将CombineInstance加入CombineInstances集合  
//6.从无子mesh的模型获取整体Transform，再根据各个SkinnedMesh和骨骼信息，将对应的Transform加入Bone_Transforms集合  
//7.在无子mesh下调用CombineMeshes接口，将三个集合信息设置好开始合并。  

public class TestChara : MonoBehaviour
{

    public Button Btn_Change;
    GameObject Root;
    string[] change_string = new string[6];

    // Use this for initialization  
    void Start()
    {
        Generate();

        Btn_Change.onClick.AddListener(OnClick_Change);
    }

    // Update is called once per frame  
    void Update()
    {

    }

    //切换模型组件并创建.  
    public void OnClick_Change()
    {
        change_string[0] = change_string[0] == "male_eyes" ? "male_eyes" : "male_eyes";
        change_string[1] = change_string[1] == "male_face-1" ? "male_face-2" : "male_face-1";
        change_string[2] = change_string[2] == "male_hair-1" ? "male_hair-2" : "male_hair-1";
        change_string[3] = change_string[3] == "male_pants-1" ? "male_pants-2" : "male_pants-1";
        change_string[4] = change_string[4] == "male_shoes-1" ? "male_shoes-2" : "male_shoes-1";
        change_string[5] = change_string[5] == "male_top-1" ? "male_top-2" : "male_top-1";

        List<string> constant = new List<string>();
        constant.AddRange(change_string);
        Generate(Root, constant);
    }

    public void Generate()
    {
        //合并skinnedMesh步骤1  
        Root = Resources.Load<GameObject>("Prefabs/male");
        Root = Instantiate(Root) as GameObject;

        OnClick_Change();
    }

    public GameObject Generate(GameObject root, List<string> constant)
    {
        float startTime = Time.realtimeSinceStartup;

        // The SkinnedMeshRenderers that will make up a character will be  
        // combined into one SkinnedMeshRenderers using multiple materials.  
        // This will speed up rendering the resulting character.  
        //合并skinnedMesh步骤2  
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Material> materials = new List<Material>();
        List<Transform> bones = new List<Transform>();
        Transform[] transforms = root.GetComponentsInChildren<Transform>();

        foreach (var res in constant)
        {
            //合并skinnedMesh步骤3  
            GameObject element = Resources.Load<GameObject>("Prefabs/" + res);
            element = Instantiate(element) as GameObject;
            SkinnedMeshRenderer smr = element.GetComponent<Renderer>() as SkinnedMeshRenderer;

            //合并skinnedMesh步骤4  
            string mat_res = "Materials/";
            string[] existingMaterials = Directory.GetFiles("FBX/Characters/MAN/Materials/");
            foreach (string matfile in existingMaterials)
            {
                if (matfile.EndsWith(".mat") && matfile.Contains(res))
                {
                    mat_res += matfile.Substring(matfile.LastIndexOf('/') + 1);
                    break;
                }
            }
            mat_res = mat_res.Substring(0, mat_res.LastIndexOf('.'));
            smr.material = Resources.Load<Material>(mat_res);
            materials.AddRange(smr.materials);

            //合并skinnedMesh步骤5  
            for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                combineInstances.Add(ci);
            }

            //合并skinnedMesh步骤6  
            XmlDocument xml = new XmlDocument();
            xml.Load("Assets/Resources/" + res + ".xml");
            var Root = xml.FirstChild;
            var bonelist = Root.ChildNodes;

            foreach (XmlNode bone in bonelist)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform.name == bone.Name)
                    {
                        bones.Add(transform);
                        break;
                    }
                }
            }

            Object.Destroy(smr.gameObject);
        }

        // Obtain and configure the SkinnedMeshRenderer attached to  
        // the character base.  
        //合并skinnedMesh步骤7  
        SkinnedMeshRenderer r = root.GetComponent<SkinnedMeshRenderer>();
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
        r.bones = bones.ToArray();
        r.materials = materials.ToArray();

        Debug.Log("Generating character took: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
        return root;
    }
}