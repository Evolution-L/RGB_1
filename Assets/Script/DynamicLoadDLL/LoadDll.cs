using System;
using System.IO;
using System.Reflection;
namespace DynamicLoadDLL
{
    /// <summary>
    /// 动态加载dll
    /// </summary>
    public class LoadDLL
    {
        private Assembly ass = null;
        /// <summary>
        /// 加载dll
        /// </summary>
        /// <param name="dllPath">dll文件路径</param>
        public LoadDLL(string dllPath)
        {
            this.ass = Assembly.LoadFrom(dllPath);
            //利用dll的路径加载(fullname)
        }
        /// <summary>
        /// 返回反射的dll
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly()
        {
            return this.ass;
        }
        /// <summary>
        /// 获取所有类名
        /// </summary>
        /// <returns></returns>
        public Type[] GetClass()
        {
            return ass.GetTypes();
        }
        /// <summary>
        /// 获取程序集下的所有文件名
        /// </summary>
        /// <returns></returns>
        public Module[] GetModules()
        {
            return ass.GetModules();
        }
        /// <summary>
        /// 获取程序集清单文件表中的文件
        /// </summary>
        /// <returns></returns>
        public FileStream[] GetFiles()
        {
            return ass.GetFiles();
        }
    }
}