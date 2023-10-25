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
        public static string assetBundleVariant = ".one";
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
        public static string aBPersistentPath
        {
            get
            {
                return $"{Application.persistentDataPath}/";
            }
        }
        public static string aBStreamingPath
        {
            get
            {
                return $"{Application.streamingAssetsPath}/";
            }
        }

        public static string editorAssetPath = "Resource";

        /// <summary>
        /// 编辑器环境下
        /// </summary>
        public static string editorModeAssetPath = $"{Application.dataPath}/{editorAssetPath}/";

        /// <summary>
        /// 资源信息文件路径
        /// </summary>
        public static string assetInfoFilesPath = $"{Application.persistentDataPath}/{DirBundleName}/{assetInfoFiles}";

        // 根据后缀获取文件夹名称
        public static string GetAssetRelativePath(string fileName_)
        {
            string suffix = fileName_[(fileName_.LastIndexOf(".") + 1)..];
            return string.Format("{0}/", suffix);
        }
    }
}
