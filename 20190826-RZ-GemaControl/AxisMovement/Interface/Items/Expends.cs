using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Items
{
   public static class Expends
    {
        /// <summary>
        /// 集合转换到字符串
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static string JoinToString(this IEnumerable enumerable, string center)
        {
            string msg = "";
            foreach (var item in enumerable)
            {
                msg += item.ToString() + center;
            }
            return msg;
        }
        /// <summary>
        /// 列举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        public static void Foreach<T>(this IEnumerable<T> enumerator, Action<T> func)
        {
            foreach (var item in enumerator)
            {
                func?.Invoke(item);
            }
        }
        /// <summary>
        /// 列举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        public static void Foreach(this IEnumerable enumerator, Action<object> func)
        {
            foreach (var item in enumerator)
            {
                func?.Invoke(item);
            }
        }
        /// <summary>
        /// 转到到对应的枚举类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumType<T>(this string name)
        {
            return Enum.GetValues(typeof(T)).Where<T>(c => c.ToString().Equals(name)).First();
        }
        public static IEnumerable<T> Where<T>(this IEnumerable enumerator, Func<object, bool> func)
        {
            List<T> ts = new List<T>();
            enumerator.Foreach(_ => {
                if (func?.Invoke(_) == true)
                {
                    ts.Add((T)_);
                }
            });
            return ts;
        }
        public static void ToFile(this object obj, string path)
        {
            System.IO.File.WriteAllBytes(path, Newtonsoft.Json.JsonConvert.SerializeObject(obj).ToBytes());
        }
        /// <summary>
        /// 从文件读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ReadFromFile<T>(this string path)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
                System.Text.Encoding.UTF8.GetString(
                    System.IO.File.ReadAllBytes(path)));
        }
        /// <summary>
        /// 字符串转换到字节
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string msg)
        {
            return System.Text.Encoding.UTF8.GetBytes(msg);
        }
    }
}
