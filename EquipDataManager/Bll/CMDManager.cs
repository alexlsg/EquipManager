using EquipDataManager.Dal;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tools;

namespace EquipDataManager.Bll
{
    public partial class DataPicker
    {    /// <summary>
         /// 执行命令
         /// </summary>
         /// <param name="cmds"></param>
         /// <param name="needreturn">需要返回</param>
         /// <returns></returns>
        public string ExecCommand(List<Command> cmds, bool needreturn)
        {
            try
            {
                if (needreturn)
                {
                    lock (NeedreturnCommands)
                    {
                        NeedreturnCommands.AddRange(cmds);
                    }
                    //等待执行结束
                    while (cmds.Exists(n => string.IsNullOrWhiteSpace(n.Result)))
                    {
                        Thread.Sleep(100);
                    }
                    return String.Join("\r\n", cmds.Select(n => n.Result));
                }
                else
                {
                    lock (Commands)
                    {
                        Commands.AddRange(cmds);
                    }
                    return "命令已加入队列";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private void CmdZF()
        {
            //cmd时间间隔,毫秒
            int _cmdtimespan = 100;
            try
            {
                string _s = ConfigHelper.GetConfigString("cmd:timespan");
                int.TryParse(_s, out _cmdtimespan);
            }
            catch (Exception)
            {

            }
            while (running)
            {
                Command _cmd = null;
                //先处理需要回复的命令
                lock (NeedreturnCommands)
                {
                    if (NeedreturnCommands.Count > 0)
                    {
                        _cmd = NeedreturnCommands[0];
                        NeedreturnCommands.Remove(_cmd);
                        lock (ReturnCommands)
                        {
                            ReturnCommands.Add(_cmd);
                        }
                    }
                }
                if (_cmd == null)
                {
                    lock (Commands)
                    {
                        if (Commands.Count > 0)
                        {
                            _cmd = Commands[0];
                            Commands.Remove(_cmd);
                        }
                    }
                }
                //处理命令
                if (_cmd != null)
                {
                    Equip _eq = equips.Find(n => n.ID == _cmd.EquipID);
                    if (_eq != null)
                    {
                        string _cmdurl = null;
                        try
                        {
                            string _url = _eq.IP;
                            if (!string.IsNullOrWhiteSpace(_eq.PORT))
                                _url += ":" + _eq.PORT;
                            _cmdurl = $"http://{_url}/cgi-bin/webctrldev.cgi?oper=set&device_id={_eq.NO}&cmd={_cmd.CMD}&cmdparam={_cmd.Param}&param_value={_cmd.ParamValue}&timeout={_cmd.TimeOut}&level={_cmd.Level}&IsCurve={_cmd.IsCurve}&cmd_notes={_cmd.CMDNotes}";
                            HttpHelper _httphelper = new HttpHelper();
                            List<KeyValue> _kv = new List<KeyValue>();
                            _kv.Add(new KeyValue() { Key = "token", Value = datapicker.GetToken(_eq) }); ;
                            string _result = _httphelper.GetHtml(_cmdurl, _kv);
                            if (_result.Contains("ok"))
                            {
                                _cmd.Result = $"{_eq.Name}执行成功";
                                LogEventDal.AddLog(_eq, _cmd);
                            }
                            else
                                _cmd.Result = $"{_eq.Name}执行失败命令:{_cmd.CMD},命令参数:{_cmd.Param},命令参数值:{_cmd.ParamValue},命令说明:{_cmd.CMDNotes},错误信息:{_result}";
                        }
                        catch (Exception ex)
                        {
                            _cmd.Result = $"{_eq.Name}执行失败命令{_cmd.CMD},命令参数:{_cmd.Param},命令参数值:{_cmd.ParamValue},命令说明:{_cmd.CMDNotes},url:{_cmdurl},错误信息:{ex.Message}";
                            Log.Add(ex);
                            Log.Add(_cmdurl);
                        }
                    }
                    else
                    {
                        _cmd.Result = "设备ID不存在。";
                    }
                }
                Thread.Sleep(_cmdtimespan);
            }
        }


    }
}
