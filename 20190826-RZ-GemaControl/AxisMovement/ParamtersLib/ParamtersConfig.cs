using Interface.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParamtersLib
{
    public class ParamtersConfig : IStaticParamters
    {
        private JObject Object = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="datas"></param>
        public ParamtersConfig(byte[] datas)
        {
            string msg = System.Text.Encoding.UTF8.GetString(datas);
            Object = JsonConvert.DeserializeObject(msg) as JObject;
        }

        public string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void SetValue<T>(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}
