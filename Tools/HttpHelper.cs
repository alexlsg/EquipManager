using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Tools
{
    public class HttpHelper
    {

        public string GetHtml(string url, List<KeyValue> headers = null)
        {
            HttpWebResponse _httpWebResponse;
            HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            _httpWebRequest.Timeout = 30000;
            _httpWebRequest.Method = "GET";
            _httpWebRequest.ContentType = "text/xml; charset=utf-8";
            if (headers != null)
            {
                foreach (KeyValue kv in headers)
                    _httpWebRequest.Headers[kv.Key] = kv.Value;
            }
            string _result = "";
            using (WebResponse wr = _httpWebResponse = (HttpWebResponse)_httpWebRequest.GetResponse())
            {
                Stream stream = wr.GetResponseStream();
                byte[] buf = new byte[1024 * 1024];
                while (true)
                {
                    int len = stream.Read(buf, 0, buf.Length);
                    if (len <= 0)
                        break;
                    _result += Encoding.GetEncoding("utf-8").GetString(buf, 0, len);
                }
                _httpWebResponse.Close();
                _httpWebResponse.Dispose();
            }
            return _result;
        }
    }
}
