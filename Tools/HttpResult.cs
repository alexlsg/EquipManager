
namespace Tools
{
    /// <summary>
    /// 消息公用返回类
    /// </summary>
    public class HttpResult
    {
        public HttpResult() { }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 组装返回Json结果
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResult GetJsonResult(bool status, string succmessage, string failmessage, object data = null)
        {
            HttpResult httpResult = new HttpResult();
            httpResult.Status = status;
            httpResult.Message = status ? succmessage : failmessage;
            httpResult.Data = data;
            return httpResult;
        }
    }
}
