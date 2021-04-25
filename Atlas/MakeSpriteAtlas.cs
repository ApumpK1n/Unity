using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.U2D;
using TextureCompressionQuality = UnityEditor.TextureCompressionQuality;

public class MakeSpriteAtlas
{
    private static string[] imgTypes = {".png", ".psd"};
    private static string atlasPath = "Assets/Resources/Atlas";
    private static string dynamicLoadCfgPath = "Assets/Scripts/DynamicLoadCfg";
    private static Dictionary<string, List<string>> atlasMap = new Dictionary<string, List<string>>();
    
    [MenuItem("GameTools/Sprite Atlas/Make Atals")]
    public static void GenerateAtlas()
    {
        atlasMap.Clear();
        string[] paths =
        {
            "Art/Sprites",
            "Texture",
            "ResourcesImg",
        };
        
        if(Directory.Exists(atlasPath)){
            DirectoryInfo dirInfo = new DirectoryInfo(atlasPath);
            dirInfo.Delete(true);
        }
        Directory.CreateDirectory(atlasPath);
        
        for (int i = 0; i < paths.Length; i++)
        {
            string path = Path.Combine(Application.dataPath, paths[i]);
            MakeAtlasByDir(path);
        }
        //GenerateDynamicLoadCfg();
        AssetDatabase.Refresh();
    }

    private static void MakeAtlasByDir(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            return;
        }

        string tmpDir = dirPath.Replace('\\', '/');
        string[] dirs = tmpDir.Split('/');
        string atlasName = dirs[dirs.Length - 1];
        atlasName = GenerateAtlasName(atlasName);
        if (string.IsNullOrEmpty(atlasName))
        {
            return;
        }
        atlasMap.Add(atlasName, new List<string>());
        SpriteAtlas spriteAtlas = new SpriteAtlas();

        DirectoryInfo rootDirInfo = new DirectoryInfo(dirPath);
        FileSystemInfo[] fsInfos = rootDirInfo.GetFileSystemInfos();

        bool isNeedCreateAtlas = false;

        for (int i = 0; i < fsInfos.Length; i++)
        {
            if (File.Exists(fsInfos[i].FullName))
            {
                if (imgTypes.Contains(fsInfos[i].Extension))
                {
                    (int w, int h, Sprite sp) = GetImgSize(fsInfos[i].FullName);
                    if (w * h < 1024 * 512)
                    {
                        isNeedCreateAtlas = true;
                        spriteAtlas.Add(new Object[]{sp});
                        atlasMap[atlasName].Add(fsInfos[i].FullName);
                    }
                    else
                    {
                        Debug.LogWarning($"{fsInfos[i].FullName} size is more than 1024 * 1024");
                    }
                }
            }
            else
            {
                MakeAtlasByDir(fsInfos[i].FullName);
            }
        }

        if (isNeedCreateAtlas)
        {
            Debug.Log($"Atlas {atlasName}----{dirPath} is Making");
            SetAtlasSettings(spriteAtlas);
            AssetDatabase.CreateAsset(spriteAtlas, $"{atlasPath}/{atlasName}.spriteatlas");
        }
    }

    private static void SetAtlasSettings(SpriteAtlas atlas)
    {
        SpriteAtlasPackingSettings packingSettings = atlas.GetPackingSettings();
        packingSettings.enableRotation = false;
        packingSettings.enableTightPacking = false;
        atlas.SetPackingSettings(packingSettings);
            
        SpriteAtlasTextureSettings textureSetting = atlas.GetTextureSettings();
        atlas.SetTextureSettings(textureSetting);
        
        SetActivePlatformAttr(atlas);
        SetAndroidPlatformAttr(atlas);
        SetIOSPlatformAttr(atlas);
    }
    
    private static void SetActivePlatformAttr(SpriteAtlas atlas)
    {
        (string target, TextureImporterFormat format) = GetPlatformAttr(EditorUserBuildSettings.activeBuildTarget);
        TextureImporterPlatformSettings platformSetting = atlas.GetPlatformSettings(target);
        platformSetting.overridden = true;
        platformSetting.maxTextureSize = 2048;
        platformSetting.compressionQuality = (int)TextureCompressionQuality.Normal;
        //platformSetting.textureCompression = TextureImporterCompression.CompressedHQ;
        platformSetting.format = format;
        atlas.SetPlatformSettings(platformSetting);
    }

    private static void SetAndroidPlatformAttr(SpriteAtlas atlas)
    {
        (string target, TextureImporterFormat format) = GetPlatformAttr(BuildTarget.Android);
        TextureImporterPlatformSettings platformSetting = atlas.GetPlatformSettings(target);
        platformSetting.overridden = true;
        platformSetting.maxTextureSize = 2048;
        platformSetting.compressionQuality = (int)TextureCompressionQuality.Normal;
        //platformSetting.textureCompression = TextureImporterCompression.CompressedHQ;
        platformSetting.format = format;
        atlas.SetPlatformSettings(platformSetting);
    }
    
    private static void SetIOSPlatformAttr(SpriteAtlas atlas)
    {
        (string target, TextureImporterFormat format) = GetPlatformAttr(BuildTarget.iOS);
        TextureImporterPlatformSettings platformSetting = atlas.GetPlatformSettings(target);
        platformSetting.overridden = true;
        platformSetting.maxTextureSize = 2048;
        platformSetting.compressionQuality = (int)TextureCompressionQuality.Normal;
        //platformSetting.textureCompression = TextureImporterCompression.CompressedHQ;
        platformSetting.format = format;
        atlas.SetPlatformSettings(platformSetting);
    }
    
    private static (string, TextureImporterFormat) GetPlatformAttr(BuildTarget target)
    {
        string platformName = "";
        TextureImporterFormat format = TextureImporterFormat.ASTC_4x4;
        switch (target)
        {
            case BuildTarget.Android:
                platformName = "Android";
                format = TextureImporterFormat.ETC2_RGBA8;
                break;
            case BuildTarget.iOS:
                platformName = "iPhone";
                //format = TextureImporterFormat.PVRTC_RGBA4;
                format = TextureImporterFormat.ASTC_4x4;
                break;
            case BuildTarget.PS4:
                platformName = "PS4";
                break;
            case BuildTarget.XboxOne:
                platformName = "XboxOne";
                break;
            case BuildTarget.NoTarget:
                platformName = "DefaultTexturePlatform";
                break;
            default:
                platformName = "Standalone";
                break;
        }
        return (platformName, format);
    }
    
    private static (int, int, Sprite) GetImgSize(string path)
    {
        path = path.Substring(path.IndexOf("Assets"));
        Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (null == sp)
        {
            Debug.LogWarning($"{path} textureType is not Sprite(2D and UI)");
            return (1024, 512, null);
        }
        return (sp.texture.width, sp.texture.height, sp);
    }
    
    private static string GenerateAtlasName(string atlasName)
    {
        atlasName = atlasName.Replace(" ", "");
        atlasName = atlasName.ToLower();
        if (atlasMap.ContainsKey(atlasName))
        {
            atlasName = atlasName + "1";
            return GenerateAtlasName(atlasName);
        }
        else
        {
            return atlasName;
        }
    }

    private static void GenerateDynamicLoadCfg()
    {
        if(Directory.Exists(dynamicLoadCfgPath)){
            DirectoryInfo dirInfo = new DirectoryInfo(dynamicLoadCfgPath);
            dirInfo.Delete(true);
        }
        Directory.CreateDirectory(dynamicLoadCfgPath);
        string filePath = $"{dynamicLoadCfgPath}/DynamicLoadCfg.cs";
        File.Create(filePath).Close();
        
        StreamWriter sw = new StreamWriter(filePath, true);
        
        StringBuilder sb = new StringBuilder();
        sb.Append("using System.Collections.Generic;\n");
        sb.Append("public class DynamicLoadCfg\n{\n");
        sb.Append("\tpublic static Dictionary<string, string> cfg = new Dictionary<string, string>\n\t{\n");
        sw.Write(sb.ToString());

        sb.Clear();
        int count = 0;
        int len = 0;
        foreach (var kv in atlasMap)
        {
            for (int i = 0; i < kv.Value.Count; i++)
            {
                if (kv.Value[i].IndexOf("ResourcesImg/") != -1)
                {
                    string str = kv.Value[i].Split('.')[0];
                    if (string.IsNullOrEmpty(str))
                    {
                        continue;
                    }
                    str = str.Substring(kv.Value[i].IndexOf("ResourcesImg/"));
                    str = str.Replace("ResourcesImg/", "");
                    sb.Append("\t\t{\"" + str + "\", \"" + kv.Key + "\"},\n");
                    sw.Write(sb.ToString());
                    sb.Clear();
                    count++;
                }
            }

            len += kv.Value.Count;
        }
        
        sb.Append("\t};\n}");
        sw.Write(sb.ToString());
        sw.Close();
    }
}
