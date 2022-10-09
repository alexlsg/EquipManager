using EquipDataManager.Dal;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManagement;

namespace EquipDataManager.Bll
{
    public partial class DataPicker
    {
        private bool GetSjdb(EquipData a, EquipTjSet b)
        {
            if (b.SpotNO.Split(',').Contains(a.DataType + "-" + a.SpotNO))
            {
                if (b.Tjtj == "State")
                {
                    if (b.Data.StartsWith("<>"))
                    {
                        string _v = b.Data.Replace("<>", "");
                        return a.State != _v;
                    }
                    else
                        return a.State == b.Data;
                }
                else if (b.Tjtj == "NoState")
                {
                    if (b.Data.StartsWith("<>"))
                    {
                        string _v = b.Data.Replace("<>", "");
                        return a.NoState != _v;
                    }
                    else
                        return a.NoState == b.Data;
                }
                else if (b.Tjtj == "Data")
                {
                    double _d1;
                    double _d2;
                    if (double.TryParse(a.Data, out _d1) && double.TryParse(b.Data, out _d2))
                        return _d1 == _d2;
                    return false;
                }
            }
            return false;
        }
        public object GetDataByGroup(string groupid)
        {
            lock (dataCachelockobj)
            {
                var _jdsh = from a in dataCache
                            from b in equips
                            where
                            a.EquipID == b.ID
                            && b.EquipType == Config.Jdsblx
                            && b.GroupID == groupid
                            && a.DataType == "A"
                            && Config.Jdsbspots.Contains(a.SpotNO)
                            && Config.Jdsbjcz.Contains(a.Data)
                            group a by a.EquipName into g
                            select new
                            {
                                Name = g.Key,
                                Images = g.Select(n => GetSbzt(n.Data))
                            };
                var _wsd = from a in dataCache
                           from b in equips
                           where
                            a.EquipID == b.ID
                           && b.EquipType == Config.Wsdlx
                           && b.GroupID == groupid
                           && a.DataType == "A"
                           && Config.Jdsbspots.Contains(a.SpotNO)
                           group a by a.EquipName into g
                           select new
                           {
                               Name = g.Key,
                               Wd = g.Where(n => n.SpotNO == "51").First().Data,
                               Sd = g.Where(n => n.SpotNO == "52").First().Data,
                           };

                var _jdsbhz = from a in dataCache
                              from b in equips
                              from c in equipTjSets
                              where
                            a.EquipID == b.ID
                         && b.EquipType == Config.Jdsblx
                         && b.GroupID == groupid
                         && a.DataType == "A"
                         && Config.Jdsbspots.Contains(a.SpotNO)
                         && c.EquipTypeName == Config.Jdsblx
                         && GetSjdb(a, c)
                              group new { a, c } by c.Tjlx into g
                              select new
                              {
                                  Name = g.Key,
                                  Zs = g.Count(),
                                  Bjs = g.Where(n => Config.Jdsbbjz.Contains(n.a.Data)).Count()
                              };
                var _lzfjtj = from a in dataCache
                              from b in equipTjSets
                              from c in equips
                              where
                              a.EquipID == c.ID
                              && c.EquipType == Config.Fjlx
                              && c.EquipTypeName == b.EquipTypeName
                              && GetSjdb(a, b)
                              group a by new { b.Tjzt } into g
                              select new
                              {
                                  Name = g.Key,
                                  Count = g.Count()
                              };
                var _result = new
                {
                    Jdsb = _jdsh.ToList(),
                    Wsd = _wsd.ToList(),
                    Jdsbhz = _jdsbhz.ToList(),
                    Lzfjhz = _lzfjtj.ToList()
                };
                return _result;
            }
        } /// <summary>
          /// 获取设备实时统计数据,按类型
          /// </summary>
          /// <returns></returns>
        public List<EquipSstjData> GetEquipSstjDataByType(string user,string typeid, string groupid)
        {
            lock (dataCachelockobj)
            {
                //权限配置
                var _temp = (from a in dataCache
                             from b in equipTjSets
                             from c in equips
                             where a.EquipID == c.ID
                             && (string.IsNullOrWhiteSpace(typeid) || c.EquipType == typeid)
                             && (string.IsNullOrWhiteSpace(groupid) || c.GroupID == groupid)
                             && c.EquipTypeName == b.EquipTypeName
                             && GetSjdb(a, b)
                             && Pub.CheckQx(user, a.EquipID.ToString(), c.GroupID)
                             group a by new { b.EquipTypeName, b.Tjlx, b.Tjzt, b.Cyhj, b.Cyzshj } into g
                             select new EquipSstjData
                             {
                                 TypeName = g.Key.EquipTypeName,
                                 Tjlx = g.Key.Tjlx,
                                 Tjzt = g.Key.Tjzt,
                                 Count = g.Count(),
                                 Cyhj = g.Key.Cyhj,
                                 Cyzshj = g.Key.Cyzshj,
                                 TjType = "明细",
                                 Px = 0
                             }).ToList();

                var _temp2 = from a in equipTjSets
                             where (string.IsNullOrWhiteSpace(typeid) || a.EquipType == typeid) &&
                             !_temp.Exists(n => n.TypeName == a.EquipTypeName && n.Tjlx == a.Tjlx && n.Tjzt == a.Tjzt)
                             select new EquipSstjData
                             {
                                 TypeName = a.EquipTypeName,
                                 Tjlx = a.Tjlx,
                                 Tjzt = a.Tjzt,
                                 Count = 0,
                                 Cyhj = a.Cyhj,
                                 Cyzshj = a.Cyzshj,
                                 TjType = "明细",
                                 Px = 0
                             };
                List<EquipSstjData> _temp3 = new List<EquipSstjData>(_temp);
                _temp3.AddRange(_temp2);
                var _temp1 = from a in _temp3
                             where a.Cyhj
                             group a by new { a.TypeName, a.Tjlx, a.Cyzshj } into g
                             select new EquipSstjData
                             {
                                 TjType = g.Key.Cyzshj ? "总计" : "合计",
                                 TypeName = g.Key.TypeName,
                                 Tjlx = g.Key.Tjlx,
                                 Tjzt = "总数",
                                 Count = g.Sum(n => n.Count),
                                 Cyzshj = g.Key.Cyzshj,
                                 Px = 99
                             };
                List<EquipSstjData> _result = new List<EquipSstjData>();
                _result.AddRange(_temp3);
                _result.AddRange(_temp1);
                return _result.OrderBy(n => n.TypeName + n.Cyzshj.ToString() + n.Tjlx + n.Px.ToString() + n.Tjzt).ToList();
            }
        }

        /// <summary>
        /// 获取设备实时统计数据,按产线
        /// </summary>
        /// <returns></returns>
        public List<EquipSstjData> GetEquipSstjDataByGroup(string groupid)
        {
            lock (dataCachelockobj)
            {
                var _temp = (from a in dataCache
                             from b in equipTjSets
                             from c in equips
                             where a.EquipID == c.ID && c.GroupID == groupid && c.EquipTypeName == b.EquipTypeName && GetSjdb(a, b)
                             group a by new { c.GroupName, b.Tjlx, b.Tjzt, b.Cyhj, b.Cyzshj } into g
                             select new EquipSstjData
                             {
                                 TjType = "明细",
                                 TypeName = g.Key.GroupName,
                                 Tjlx = g.Key.Tjlx,
                                 Tjzt = g.Key.Tjzt,
                                 Count = g.Count(),
                                 Cyhj = g.Key.Cyhj,
                                 Cyzshj = g.Key.Cyzshj,
                                 Px = 0
                             }).ToList();
                var _temp2 = from a in equipTjSets
                             join b in equips on a.EquipTypeName equals b.EquipTypeName
                             where b.GroupID == groupid &&
                             !_temp.Exists(n => n.TypeName == b.GroupName && n.Tjlx == a.Tjlx && n.Tjzt == a.Tjzt)
                             group new { a, b } by new { b.GroupName, a.Tjlx, a.Tjzt, a.Cyhj, a.Cyzshj } into g
                             select new EquipSstjData
                             {
                                 TjType = "明细",
                                 TypeName = g.Key.GroupName,
                                 Tjlx = g.Key.Tjlx,
                                 Tjzt = g.Key.Tjzt,
                                 Count = 0,
                                 Cyhj = g.Key.Cyhj,
                                 Cyzshj = g.Key.Cyzshj,
                                 Px = 0
                             };
                List<EquipSstjData> _temp3 = new List<EquipSstjData>(_temp);
                _temp3.AddRange(_temp2);
                var _temp1 = from a in _temp3
                             where a.Cyhj
                             group a by new { a.TypeName, a.Tjlx, a.Cyzshj } into g
                             select new EquipSstjData
                             {
                                 TjType = g.Key.Cyzshj ? "总计" : "合计",
                                 TypeName = g.Key.TypeName,
                                 Tjlx = g.Key.Tjlx,
                                 Tjzt = "总数",
                                 Count = g.Sum(n => n.Count),
                                 Cyzshj = g.Key.Cyzshj,
                                 Px = 99
                             };
                List<EquipSstjData> _result = new List<EquipSstjData>();
                _result.AddRange(_temp3);
                _result.AddRange(_temp1);
                //_result.AddRange(_temp2);
                return _result.OrderBy(n => n.TypeName + n.Cyzshj.ToString() + n.Tjlx + n.Px.ToString() + n.Tjzt).ToList();
            }
        }

        /// <summary>
        /// 获取历史数据左边列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetLssjcdlb(string user,string groupid)
        {
            var _temp = from a in equips
                        from b in equipsSpotSets
                        where
                        a.EquipTypeName == b.EquipTypeName
                        && b.SaveType == 3
                        && a.GroupID == groupid
                        && Pub.CheckQx(user,a.ID.ToString(),a.GroupID)
                        group new { a, b } by a.Name into g
                        select new
                        {
                            Name = g.Key,
                            Spots = (from c in g
                                     select new
                                     {
                                         Name = g.Key + c.b.SpotName,
                                         Code = c.a.ID + c.b.DataType + c.b.SpotNO
                                     }).ToList()
                        };
            return _temp.ToList();
        }
        /// <summary>
        /// 查询历史数据
        /// </summary>
        /// <param name="spotno"></param>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public List<EquipData_Ls> GetLssj(string spotno, DateTime ksrq, DateTime jsrq)
        {
            return EquipDataDal.GetLssj(spotno, ksrq, jsrq);
        }

    }
}
