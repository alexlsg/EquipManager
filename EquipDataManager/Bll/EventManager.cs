using EquipDataManager.Dal;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipDataManager.Bll
{
    public partial class DataPicker
    {      /// <summary>
           /// 计算事件级别
           /// </summary>
           /// <param name="data"></param>
        void CalcEventLevel(EquipData data)
        {
            double _value;
            string _key = GetKey(data);
            //值为数字，才进行匹配
            if (double.TryParse(data.Data, out _value))
            {
                //匹配阈值规则
                var _temp = eventYzSets.Find(n => n.EquipID == data.EquipID && n.DataType == data.DataType && n.SpotNO == data.SpotNO);
                if (_temp != null)
                {
                    Pub.AddTestLog($"匹配到阈值规则ID{_temp.ID}，设备ID:{data.EquipID},测点:{data.DataType}{data.SpotNO},数据:{data.Data}");
                    //已保存有事件
                    if (eventdatas.ContainsKey(_key))
                    {
                        Pub.AddTestLog($"上次事件类型:{eventdatas[_key].eventtype}");
                        if (eventdatas[_key].eventtype == 0)
                        {
                            if (_value < _temp.LowerValue)
                            {
                                data.eventyset = _temp;
                                data.eventtype = 1;
                                data.eventmessage = _temp.MsLower;
                                Pub.AddTestLog($"低于下限{_temp.LowerValue}");
                            }
                            else if (_value > _temp.UperValue)
                            {
                                data.eventyset = _temp;
                                data.eventtype = 2;
                                data.eventmessage = _temp.MsUper;
                                Pub.AddTestLog($"高于上限{_temp.UperValue}");
                            }
                            else
                            {
                                Pub.AddTestLog($"值正常无变化");
                            }
                        }
                        else if (eventdatas[_key].eventtype == 1)
                        {
                            if (_value > _temp.LowerHF && _value < _temp.UperValue)
                            {
                                data.eventyset = _temp;
                                data.eventtype = 0;
                                data.eventmessage = _temp.MsHf;
                                Pub.AddTestLog($"恢复正常");
                            }
                            else if (_value > _temp.UperValue)
                            {
                                data.eventyset = _temp;
                                data.eventtype = 2;
                                data.eventmessage = _temp.MsUper;
                                Pub.AddTestLog($"高于上限{_temp.UperValue}");
                            }
                            else
                            {
                                Pub.AddTestLog($"值低于下限无变化");
                            }
                        }
                        else
                        {
                            if (_value < _temp.UperHF && _value > _temp.LowerValue)
                            {
                                data.eventyset = _temp;
                                data.eventtype = 0;
                                data.eventmessage = _temp.MsHf;
                                Pub.AddTestLog($"恢复正常");
                            }
                            else if (_value < _temp.LowerValue)
                            {
                                data.eventyset = _temp;
                                data.eventtype = 1;
                                data.eventmessage = _temp.MsLower;
                                Pub.AddTestLog($"低于下限{_temp.LowerValue}");
                            }
                            else
                            {
                                Pub.AddTestLog($"值高于上限无变化");
                            }
                        }
                    }
                    else if (_value < _temp.LowerValue)
                    {
                        data.eventyset = _temp;
                        data.eventtype = 1;
                        data.eventmessage = _temp.MsLower;
                        Pub.AddTestLog($"低于下限{_temp.LowerValue}");
                    }
                    else if (_value > _temp.UperValue)
                    {
                        data.eventyset = _temp;
                        data.eventtype = 2;
                        data.eventmessage = _temp.MsUper;
                        Pub.AddTestLog($"高于上限{_temp.UperValue}");
                    }
                    else
                    {
                        data.eventyset = _temp;
                        data.eventtype = 0;
                        data.eventmessage = _temp.MsHf;
                        Pub.AddTestLog($"值正常无事件");
                    }
                }
                else
                {
                    //匹配测点
                    var _temp1 = eventsets.Find(n => equips.Exists(m => m.EquipType == n.EquipType.ToString() && m.ID == data.EquipID) && n.DataType == data.DataType && n.SpotNO == data.SpotNO && n.Value == _value.ToString());
                    if (_temp1 != null)
                    {
                        Pub.AddTestLog($"匹配到状态规则ID{_temp1.ID}，Level{_temp1.EventLevel}，设备ID:{data.EquipID},测点:{data.DataType}{data.SpotNO},数据:{data.Data}");
                        if (eventdatas.ContainsKey(_key))
                        {
                            Pub.AddTestLog($"上次事件,类型:{eventdatas[_key].eventtype},ID:{eventdatas[_key].eventset.ID}");
                            //与上次不是同一事件,并且事件等级不同为0
                            if (_temp1.ID != eventdatas[_key].eventset.ID && (_temp1.EventLevel != 0 || eventdatas[_key].eventset.EventLevel != 0))
                            {
                                Pub.AddTestLog($"上次事件类型:{eventdatas[_key].eventtype}");
                                data.eventset = _temp1;
                                data.eventtype = 3;
                                data.eventmessage = _temp1.Desc;
                            }
                            else
                            {
                                Pub.AddTestLog($"与上次事件同ID或者事件级别都是0");
                            }
                        }
                        else if (_temp1.EventLevel > 0)
                        {
                            Pub.AddTestLog($"初次事件，级别大于0");
                            data.eventset = _temp1;
                            data.eventtype = 3;
                            data.eventmessage = _temp1.Desc;
                        }
                        else
                        {
                            Pub.AddTestLog($"事件等级小于等于0,不形成事件.");
                        }
                    }
                    else
                    {
                        Pub.AddTestLog($"未匹配到任何规则");
                    }
                }
            }
            else
            {
                Pub.AddTestLog($"值为非数字，设备ID:{data.EquipID},测点:{data.DataType}{data.SpotNO},数据:{data.Data}");
            }
        }
        void SeveEvent1(EquipData data, Equip item)
        {
            if (data.eventset?.CMDID != null || data.eventyset?.CMDID != null)
            {
                CMDSet _cmdset = cmdsets.Find(n => n.ID == data.eventset?.CMDID || n.ID == data.eventyset?.CMDID);
                if (_cmdset != null)
                {
                    Command _cmd = new Command()
                    {
                        CMD = _cmdset.CMD,
                        CMDNotes = _cmdset.Notes,
                        EquipID = item.ID,
                        IsCurve = _cmdset.IsCurve,
                        Level = _cmdset.Level,
                        Param = _cmdset.Param,
                        ParamValue = _cmdset.ParamValue,
                        TimeOut = _cmdset.TimeOut,
                        User = "admin"
                    };
                    ExecCommand(new List<Command>() { _cmd }, false);
                }
            }
            EquipDataDal.SaveEvent(data, item);
            eventdatas[GetKey(data)] = data;
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SaveEvent(Equip item, EquipData data)
        {
            Pub.AddTestLog($"开始进行事件匹配,设备ID:{item.ID},测点:{data.DataType}{data.SpotNO},数据:{data.Data}");
            //匹配事件
            CalcEventLevel(data);
            Pub.AddTestLog($"事件匹配结果,设备ID{item.ID},事件类型:{data.eventtype},事件类型说明:-1(无事件),0(值正常),1(值低于下限),2(值高于上限),3(匹配到状态事件)");
            //匹配有事件
            if (data.eventtype > -1)
            {
                string _key = GetKey(data);
                //初次获取
                if (!eventdatas.ContainsKey(_key))
                {
                    //非正常事件
                    if (data.eventtype > 0)
                    {
                        SeveEvent1(data, item);
                    }
                }
                else
                {
                    SeveEvent1(data, item);
                }
            }
        }

    }
}
