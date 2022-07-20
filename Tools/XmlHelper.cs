using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tools
{
    public static class XmlHelper
    {
        /// <summary>
        /// 当前路径
        /// </summary>
        public static string CurrentDir = System.AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 获取当前路径文件夹下的文件完整路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FullName(string fileName, string dir = "")
        {
            if (string.IsNullOrEmpty(dir))
            {
                dir = CurrentDir;
            }
            return Path.Combine(dir, fileName);
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="fullName">保存路径</param>
        /// <returns>泛型类对象</returns>
        public static T LoadModelFromXml<T>(string fullName)
        {
            if (File.Exists(fullName))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                Stream stream = new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                T _result = (T)xs.Deserialize(stream);
                stream.Close();
                return _result;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="path">保存路径</param>
        /// <returns>泛型类对象</returns>
        public static T LoadModelFromXml<T>(Stream stream)
        {
            if (stream != null)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                T _result = (T)xs.Deserialize(stream);
                stream.Close();
                return _result;
            }
            else
            {
                return default(T);
            }
        }

        public static List<T> LoadListFromXml<T>(string path)
        {
            if (File.Exists(path))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<T>));
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                List<T> _result = (List<T>)xs.Deserialize(stream);
                stream.Close();
                return _result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">完整路径</param>
        /// <param name="data"></param>
        public static void SaveModelToXml<T>(string path, T data)
        {
            string _dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
            XmlSerializer xs = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            xs.Serialize(stream, data);
            stream.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">完整路径</param>
        /// <param name="data"></param>
        public static void SaveListToXml<T>(string path, List<T> data)
        {
            string _dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            xs.Serialize(stream, data);
            stream.Close();
        }
    }
}
