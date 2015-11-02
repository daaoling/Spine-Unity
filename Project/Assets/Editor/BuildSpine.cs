using UnityEngine;
using UnityEditor;

public class BuildSpine
{
    [MenuItem("Tools/BuildSpine")]
    public static void DoIt()
    {
        string dirName = "Assets/Animation";
        string spineFileName = "hounv";
        string textureName = dirName + "/" + spineFileName + "/" + spineFileName + ".png";
        string atlasFileName = dirName + "/" + spineFileName + "/" + spineFileName + ".atlas.txt";
        string jsonFileName = dirName + "/" + spineFileName + "/" + spineFileName + ".json.txt";
       

        string atlasAssetName = dirName + "/" + spineFileName + "/" + spineFileName + ".asset";
        string skeletonDataAssetName = dirName + "/" + spineFileName + "/" + spineFileName + "skeltonData" + ".asset";

        ///1、 创建材质，并指贴图和shader
        Shader shader = Shader.Find("Spine/SkeletonGhost");
        Material mat = new Material(shader);
        Texture tex = Resources.LoadAssetAtPath(textureName, typeof(Texture)) as Texture;
        mat.SetTexture("_MainTex", tex);
        AssetDatabase.CreateAsset(mat, dirName + "/" + spineFileName + "/" + spineFileName + ".mat");
        AssetDatabase.SaveAssets();

        ///2、 创建atlas，并指xx
        AtlasAsset m_AtlasAsset = AtlasAsset.CreateInstance<AtlasAsset>();
        AssetDatabase.CreateAsset(m_AtlasAsset, atlasAssetName);
        Selection.activeObject = m_AtlasAsset;

        TextAsset textAsset = Resources.LoadAssetAtPath(atlasFileName, typeof(TextAsset)) as TextAsset;
        m_AtlasAsset.atlasFile = textAsset;
        m_AtlasAsset.materials = new Material[1];
        m_AtlasAsset.materials[0] = mat;
        AssetDatabase.SaveAssets();

        ///3、 创建SkeletonDataAsset，并指相关
        SkeletonDataAsset m_skeltonDataAsset = SkeletonDataAsset.CreateInstance<SkeletonDataAsset>();
        AssetDatabase.CreateAsset(m_skeltonDataAsset, skeletonDataAssetName);
        Selection.activeObject = m_skeltonDataAsset;

        m_skeltonDataAsset.atlasAssets = new AtlasAsset[1];
        m_skeltonDataAsset.atlasAssets[0] = m_AtlasAsset;
        TextAsset m_jsonAsset = Resources.LoadAssetAtPath(jsonFileName, typeof(TextAsset)) as TextAsset;
        m_skeltonDataAsset.skeletonJSON = m_jsonAsset;
        AssetDatabase.SaveAssets();


        /// 创建场景物件
        GameObject gameObject = new GameObject(spineFileName, typeof(SkeletonAnimation));
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = gameObject;

        SkeletonAnimation m_skelAnim = gameObject.GetComponent<SkeletonAnimation>();
        m_skelAnim.skeletonDataAsset = m_skeltonDataAsset;
        //m_skelAnim.initialSkinName = "normal";
    }
}