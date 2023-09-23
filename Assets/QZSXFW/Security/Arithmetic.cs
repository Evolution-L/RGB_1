using System;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 安全工具类
    /// </summary>
    public static class Arithmetic
    {
        /// <summary>
        /// 计算MD5值
        /// </summary>
        /// <returns>原始字符串</returns>
        /// <param name="source">MD5计算后的字符串</param>
        public static string MD5(string source)
        {
            byte[] input = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] hash = System.Security.Cryptography.MD5.Create().ComputeHash(input);

            string str = "";
            int length = hash.Length;
            for(int i = 0; i < length; ++i)
            {
                str += hash[i].ToString("X2");
            }

            return str.ToLower();
        }

        /// <summary>
        /// 计算文件的MD5值，文件不存在返回"null"
        /// </summary>
        /// <returns>文件的MD5值</returns>
        /// <param name="path">文件存储路径</param>
        public static string FileMD5(string path)
        {
            if(!File.Exists(path))
            {
                Debug.LogError("File '" + path + "' not exist!");
                return null;
            }

            int bufferSize = 1024 * 16;
            byte[] buffer = new byte[bufferSize];

            Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();

            int readLength = 0;
            byte[] output = new byte[bufferSize];

            while((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
            }
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);

            string md5 = BitConverter.ToString(hashAlgorithm.Hash);

            hashAlgorithm.Clear();
            inputStream.Close();
            inputStream.Dispose();

            return md5.Replace("-", "").ToLower();
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns>随机字符串</returns>
        /// <param name="length">生成字符串的长度</param>
        /// <param name="numeric">如果为<c>true</c>生成带有数字的随机字符串</param>
        public static string RandString(uint length, bool numeric = true)
        {
            string pool1 = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
            string pool2 = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

            string chars = numeric ? pool1 : pool2;
            int max = chars.Length - 1;

            string result = "";
            for(int i = 0; i < length; ++i)
            {
                result += chars[UnityEngine.Random.Range(0, max)];
            }

            return result;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <returns>Base64编码后的结果</returns>
        /// <param name="source">原始字符串</param>
        public static string Base64Encode(string source)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <returns>Base64解码后的结果</returns>
        /// <param name="source">base64字符串</param>
        public static string Base64Decode(string source)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(source));
        }


        /// <summary>
        /// 随机取出一个区间内不重复的一组数
        /// </summary>
        /// <param name="total">随机区间最大值</param>
        /// <param name="n">你要随机产出的数字个数</param>
        /// <returns>举例</returns>
        public static int[] GetRandomSequence(int total, int n)
        {
            //随机总数组
            int[] sequence = new int[total];
            //取到的不重复数字的数组长度
            int[] output = new int[n];
            for(int i = 0; i < total; i++)
            {
                sequence[i] = i;
            }
            int end = total - 1;
            for(int i = 0; i < n; i++)
            {
                //随机一个数，每随机一次，随机区间-1
                int num = UnityEngine.Random.Range(0, end + 1);
                output[i] = sequence[num];
                //将区间最后一个数赋值到取到数上
                sequence[num] = sequence[end];
                end--;
                //执行一次效果如：1，2，3，4，5 取到2
                //则下次随机区间变为1,5,3,4;
            }
            return output;
        }

        /// <summary>
        /// 根据次数随机出一个数组，不超过总数
        /// </summary>
        /// <param name="sum">最大总数</param>
        /// <param name="n">随机次数</param>
        /// <returns></returns>
        public static int[] GetRandomBySum(int sum, int n)
        {
            if(sum < 0)
            {
                sum = Mathf.Abs(sum);
            }

            if(sum < n)
            {
                Debug.Log("数据非法");
                return null;
            }
            int[] output = new int[n];

            int end = sum;
            for(int i = 0; i < n - 1; i++)
            {
                int temp = end - n;
                if(temp < 0)
                    temp = 0;
                int num = UnityEngine.Random.Range(1, temp);

                output[i] = num;
                end -= num;
            }
            output[n - 1] = end;
            return output;
        }

    }
}
