using System;
using System.Reflection;

namespace QZSXFrameWork.DynamicLoadDLL
{
    /// <summary>
    /// 加载类
    /// </summary>
    public class LoadClass
    {
        private static LoadClass dlc = null;
        private Type type;
        private object obj = null;
        //实例
        /// <summary>
        /// 加载dll
        /// </summary>
        /// <param name="ass">dll引用</param>
        /// <param name="nameSpace">类的命名空间</param>
        /// <param name="classPath">类名称</param>
        private LoadClass(Assembly ass, string nameSpace, string classPath)
        {
            //加载dll后,需要使用dll中某类.
            type = ass.GetType(nameSpace + "." + classPath);
            //利用类型的命名空间和名称获得类型
            //需要实例化类型,才可以使用,
            //参数可以人为的指定,也可以无参数,静态实例可以省略
            obj = Activator.CreateInstance(type);
            //利用指定的参数实例话类型
        }
        /// <summary>
        /// 加载dll
        /// </summary>
        /// <param name="ass">dll引用</param>
        /// <param name="nameSpace">类的命名空间</param>
        /// <param name="classPath">类名称</param>
        public static LoadClass GetInstance(Assembly ass, string nameSpace, string classPath)
        {
            if (dlc == null)
            {
                dlc = new LoadClass(ass, nameSpace, classPath);
            }
            return dlc;
        }
        /// <summary>
        /// 获取属性集
        /// </summary>
        /// <returns>返回属性值</returns>
        public PropertyInfo[] GetAttrs()
        {
            //调用类型中的某个属性:
            PropertyInfo[] prop = type.GetProperties();
            //通过属性名称获得属性
            //返回属性集
            return prop;
        }
        public MethodInfo[] GetMethods()
        {
            //调用类型中的方法:
            MethodInfo[] method = type.GetMethods(BindingFlags.NonPublic);
            //获得方法集
            //返回方法集
            return method;
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="attrName">属性名称</param>
        /// <returns>返回属性值</returns>
        public object GetAttrValue(string attrName)
        {
            //调用类型中的某个属性:
            PropertyInfo prop = type.GetProperty(attrName);
            //通过属性名称获得属性
            //返回属性值
            return prop.GetValue(obj);
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="attrName">属性名称</param>
        /// <returns>返回属性值</returns>
        public void SetAttrValue(string attrName, string attrValue)
        {
            //调用类型中的某个属性:
            PropertyInfo prop = type.GetProperty(attrName);
            //通过属性名称获得属性
            //返回属性值
            prop.SetValue(obj, attrValue);
        }
        /// <summary>
        /// 执行类方法
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="paras">参数</param>
        /// <param name="types">参数类型</param>
        /// <returns></returns>
        public object GetMethod(string methodName, object[] paras, Type[] types)
        {
            //调用类型中的某个方法:
            MethodInfo method = type.GetMethod(methodName, types);
            //通过方法名称获得方法
            //执行方法
            return method.Invoke(obj, paras);
        }
    }
}
