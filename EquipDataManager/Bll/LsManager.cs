using EquipDataManager.Dal;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipDataManager.Bll
{
    public partial class DataPicker
    {
        public bool CheckSave_Bhl(EquipData data, EquipSpotSet set)
        {
            string _key = GetKey(data);
            bool _re = false;
            //初次直接保存,否则进行计算
            if (lsdatas.ContainsKey(_key))
            {
                EquipData _temp = lsdatas[_key];
                double _value, _valueold;
                //都是数字进行比较,否则保存
                if (double.TryParse(data.Data, out _value) && double.TryParse(_temp.Data, out _valueold))
                {
                    //变动大于变换率
                    if (_valueold == 0 && _value != 0 || (Math.Abs(_value - _valueold) * 100 / _valueold > set.Bhl))
                    {
                        _re = true;
                    }
                    else
                    {
                        if (dataSaveTime.ContainsKey(_key))
                        {
                            if ((DateTime.Now - dataSaveTime[_key]).TotalSeconds > set.Mrjg)
                            {
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                            _re = true;
                    }
                }
                else
                    _re = true;
            }
            else
                _re = true;
            return _re;
        }       /// <summary>
                /// 保存明细
                /// </summary>
                /// <param name="data"></param>
                /// <exception cref="NotImplementedException"></exception>
        private void SaveMx(EquipData data, EquipSpotSet set)
        {
            double _value;
            if (double.TryParse(data.Data, out _value))
            {
                string _key = GetKey(data);
                bool _bc = false;
                switch (set.SaveType)
                {
                    case 3: _bc = CheckSave_Bhl(data, set); break;
                    case 4: _bc = CheckSave_Bxs(data, set); break;
                    case 5: _bc = CheckSave_Yxs(data, set); break;
                    default:
                        break;
                }
                if (_bc)
                {
                    EquipDataDal.SaveMx(data);
                    lsdatas[_key] = data;
                    dataSaveTime[_key] = DateTime.Now;
                }
            }
        }

        private bool CheckSave_Yxs(EquipData data, EquipSpotSet set)
        {
            string _key = GetKey(data);
            if (dataSaveTime.ContainsKey(_key))
            {
                return (DateTime.Now - dataSaveTime[_key]).TotalHours > 1;
            }
            else
                return true;
        }

        private bool CheckSave_Bxs(EquipData data, EquipSpotSet set)
        {
            string _key = GetKey(data);
            if (dataSaveTime.ContainsKey(_key))
            {
                return (DateTime.Now - dataSaveTime[_key]).TotalHours > 0.5;
            }
            else
                return true;
        }


    }
}
