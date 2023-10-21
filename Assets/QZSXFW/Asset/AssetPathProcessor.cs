using UnityEngine;

namespace QZSXFrameWork.Asset
{
    /// <summary>
    /// 此类提供资源路径处理逻辑,
    /// 分出该类的原因是可能提供多种打包方式(幻想),
    /// 单纯感觉可以把相关功能分出来做一个工具类,
    /// 暂定为静态类,
    /// </summary>
    public static class AssetPathProcessor
    {
        /// <summary>
        /// AssetBundle文件夹名称
        /// </summary>
        public static string DirBundleName = "data";
        /// <summary>
        /// 储存所有资源信息的文件
        /// </summary>
        public static string assetInfoFiles = "files.json";

        /// <summary>
        /// 沙盒目录  数据持久化目录
        /// </summary>
        public static string persistentDataPath { get { return Application.persistentDataPath + "/"; } }

        /// <summary>
        /// 本地目录
        /// </summary> 
        public static string streamingAssetsPath { get { return Application.streamingAssetsPath + "/"; } }

        /// <summary>
        /// 获取AB资源目录 需要设置 Unity宏 AB_MODE
        /// </summary>
        public static string assetBundleBasePath
        {
            get
            {
#if AB_MODE
                return $"{Application.persistentDataPath}/{DirBundleName}/";
#else           
                return $"{Application.streamingAssetsPath}/{DirBundleName}/";
#endif
            }
        }

        public static string editorAssetPath = "Resource";

        /// <summary>
        /// 编辑器环境下
        /// </summary>
        public static string editorModeAssetPath =  $"{Application.dataPath}/{editorAssetPath}/";
        
        /// <summary>
        /// 资源信息文件路径
        /// </summary>
        public static string assetInfoFilesPath = $"{Application.persistentDataPath}/{DirBundleName}/{assetInfoFiles}";

        // 根据后缀获取文件夹名称
        public static string GetAssetRelativePath(string fileName_)
        {
            string suffix = fileName_.Substring(fileName_.LastIndexOf(".") + 1);
            return string.Format("{0}/", suffix);
        }

        //资源名称
        public static string GetAssetShortName(string fileName_)
        {
            string shortname = fileName_.Substring(0, fileName_.LastIndexOf("."));
            string suffix = fileName_.Substring(fileName_.LastIndexOf(".") + 1);
            if (suffix == AssetExt.UI || suffix == AssetExt.MODEL || suffix == AssetExt.EFFECT ||
                suffix == AssetExt.SMODEL || suffix == AssetExt.PREFAB)
                return shortname + ".prefab";
            else if (suffix == AssetExt.CONFIG)
                return shortname + ".json";
            else if (suffix == AssetExt.TEXTURE)
                return shortname + ".png";
            else if (suffix == AssetExt.FONT)
                return shortname + ".ttf";
            else if (suffix == AssetExt.AUDIO)
                return shortname + ".mp3";
            else if (suffix == AssetExt.ANIM)
                return shortname + ".anim";
            else if (suffix == AssetExt.ASSET)
                return shortname + ".asset";
            else if (suffix == AssetExt.scene)
                return shortname + ".unity";
            else if (suffix == AssetExt.MAT)
                return shortname + ".mat";
            return string.Empty;
        }

        public static string GetAssetPath(string fileName_)
        {
            // 想要支持大小写命名格式
            // return GetAssetRelativePath(fileName_) + fileName_.ToLower();
            return GetAssetRelativePath(fileName_) + fileName_;
        }
    }
}
