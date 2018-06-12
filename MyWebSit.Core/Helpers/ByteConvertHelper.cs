using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Helpers
{
    public class ByteConvertHelper
    {
        /// <summary>
        ///  将对象转换成byte数组
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换成功后的byte数组</returns>
        public static byte[] ObjectConvertBytes(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            byte[] serializeResult = Encoding.UTF8.GetBytes(json);
            return serializeResult;
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="bytes">被转换的byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static object BytesConvertObject(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<object>(json);

        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="bytes">被转换的byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T BytesConvertObject<T>(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
