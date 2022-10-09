using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备业务类
    /// </summary>
    public class EquipService
    {
        /// <summary>
        /// 添加设备功能
        /// </summary>
        /// <param name="Equip">设备功能对象</param>
        /// <returns></returns>
        public HttpResult AddEquip(Equip Equip)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO Equip(EquipNO,EquipName,GatewayId,TypeBinding,ProductionLineGroupBinding,TxgzDesc,TxzcDesc,EventLevel,Pb,Txcs)
VALUES(@EquipNO,@EquipName,@GatewayId,@TypeBinding,@ProductionLineGroupBinding,@TxgzDesc,@TxzcDesc,@EventLevel,@Pb,@Txcs)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[10];
                mySqlParameters[0] = new MySqlParameter("EquipNO", Equip.EquipNO);
                mySqlParameters[1] = new MySqlParameter("EquipName", Equip.EquipName);
                mySqlParameters[2] = new MySqlParameter("GatewayId", Equip.GatewayId);
                mySqlParameters[3] = new MySqlParameter("TypeBinding", Equip.TypeBinding);
                mySqlParameters[4] = new MySqlParameter("ProductionLineGroupBinding", Equip.ProductionLineGroupBinding);
                mySqlParameters[5] = new MySqlParameter("TxgzDesc", Equip.TxgzDesc);
                mySqlParameters[6] = new MySqlParameter("TxzcDesc", Equip.TxzcDesc);
                mySqlParameters[7] = new MySqlParameter("EventLevel", Equip.EventLevel);
                mySqlParameters[8] = new MySqlParameter("Pb", Equip.Pb);
                mySqlParameters[9] = new MySqlParameter("Txcs", Equip.Txcs);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加设备功能成功", "添加设备功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加设备功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改设备功能
        /// </summary>

        /// <param name="Equip">设备功能对象</param>
        /// <returns></returns>
        public HttpResult ModEquip(Equip Equip)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE Equip SET EquipName=@EquipName, EquipNO=@EquipNO, GatewayId=@GatewayId,TypeBinding=@TypeBinding,ProductionLineGroupBinding=@ProductionLineGroupBinding,
TxgzDesc = @TxgzDesc,TxzcDesc = @TxzcDesc,EventLevel = @EventLevel,Pb = @Pb,Txcs = @Txcs
WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[11];
                mySqlParameters[0] = new MySqlParameter("EquipName", Equip.EquipName);
                mySqlParameters[1] = new MySqlParameter("EquipNO", Equip.EquipNO);
                mySqlParameters[2] = new MySqlParameter("GatewayId", Equip.GatewayId);
                mySqlParameters[3] = new MySqlParameter("Id", Equip.Id);
                mySqlParameters[4] = new MySqlParameter("TypeBinding", Equip.TypeBinding);
                mySqlParameters[5] = new MySqlParameter("ProductionLineGroupBinding", Equip.ProductionLineGroupBinding);
                mySqlParameters[6] = new MySqlParameter("TxgzDesc", Equip.TxgzDesc);
                mySqlParameters[7] = new MySqlParameter("TxzcDesc", Equip.TxzcDesc);
                mySqlParameters[8] = new MySqlParameter("EventLevel", Equip.EventLevel);
                mySqlParameters[9] = new MySqlParameter("Pb", Equip.Pb);
                mySqlParameters[10] = new MySqlParameter("Txcs", Equip.Txcs);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改设备功能成功", "修改设备功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改设备功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除设备功能
        /// </summary>

        /// <param name="Equip">设备功能对象</param>
        /// <returns></returns>
        public HttpResult DelEquip(Equip Equip)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From Equip WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("Id", Equip.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除设备功能成功", "删除设备功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除设备功能异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询设备功能列表
        /// </summary>

        /// <returns></returns>
        public HttpResult GetEquipList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Equip> Equips = new List<Equip>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM Equip");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Equip Equip = new Equip();
                    Equip.Id = (int)_ds.Rows[i]["Id"];
                    Equip.EquipName = _ds.Rows[i]["EquipName"].ToString();
                    Equip.EquipNO = _ds.Rows[i]["EquipNO"].ToString();
                    Equip.GatewayId = (int)_ds.Rows[i]["GatewayId"];
                    Equip.TypeBinding = (int)_ds.Rows[i]["TypeBinding"];
                    Equip.ProductionLineGroupBinding = _ds.Rows[i]["ProductionLineGroupBinding"].ToString();
                    Equip.TxgzDesc = _ds.Rows[i]["TxgzDesc"].ToString();
                    Equip.TxzcDesc = _ds.Rows[i]["TxzcDesc"].ToString();
                    Equip.EventLevel = _ds.Rows[i].Field<int?>("EventLevel");
                    Equip.Pb = _ds.Rows[i].Field<UInt64>("Pb") == 1;
                    Equip.Txcs = _ds.Rows[i].Field<int?>("Txcs");
                    Equip.DataStatus = DataStatus.NONE;
                    Equips.Add(Equip);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备功能列表成功", string.Empty, Equips);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询设备功能列表异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 根据网关ID查询设备功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipListByGatewayId(int gatewayId)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Equip> Equips = new List<Equip>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM Equip WHERE GatewayId=@GatewayId");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("GatewayId", gatewayId);
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Equip Equip = new Equip();
                    Equip.Id = (int)_ds.Rows[i]["Id"];
                    Equip.EquipName = _ds.Rows[i]["EquipName"].ToString();
                    Equip.EquipNO = _ds.Rows[i]["EquipNO"].ToString();
                    Equip.GatewayId = (int)_ds.Rows[i]["GatewayId"];
                    Equip.TypeBinding = (int)_ds.Rows[i]["TypeBinding"];
                    Equip.ProductionLineGroupBinding = _ds.Rows[i]["ProductionLineGroupBinding"].ToString();
                    Equip.TxgzDesc = _ds.Rows[i]["TxgzDesc"].ToString();
                    Equip.TxzcDesc = _ds.Rows[i]["TxzcDesc"].ToString();
                    Equip.EventLevel = _ds.Rows[i].Field<int?>("EventLevel");
                    Equip.Pb = _ds.Rows[i].Field<UInt64>("Pb") == 1;
                    Equip.Txcs = _ds.Rows[i].Field<int?>("Txcs");
                    Equip.DataStatus = DataStatus.NONE;
                    Equips.Add(Equip);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备功能列表成功", string.Empty, Equips);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询设备功能列表异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 根据产线ID和设备类型ID查询设备功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipListByProductionLineIdANDEquipTypeId(string productionLineId, string equipTypeId)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Equip> Equips = new List<Equip>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
SELECT B.* FROM ProductionLineGroup A 
JOIN Equip B ON A.EquipNO=B.EquipNO
WHERE ProductionLineId=@ProductionLineId AND B.TypeBinding=@TypeBinding
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("ProductionLineId", productionLineId);
                mySqlParameters[1] = new MySqlParameter("TypeBinding", equipTypeId);
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Equip Equip = new Equip();
                    Equip.Id = (int)_ds.Rows[i]["Id"];
                    Equip.EquipName = _ds.Rows[i]["EquipName"].ToString();
                    Equip.EquipNO = _ds.Rows[i]["EquipNO"].ToString();
                    Equip.GatewayId = (int)_ds.Rows[i]["GatewayId"];
                    Equip.TypeBinding = (int)_ds.Rows[i]["TypeBinding"];
                    Equip.ProductionLineGroupBinding = _ds.Rows[i]["ProductionLineGroupBinding"].ToString();
                    Equip.TxgzDesc = _ds.Rows[i]["TxgzDesc"].ToString();
                    Equip.TxzcDesc = _ds.Rows[i]["TxzcDesc"].ToString();
                    Equip.EventLevel = _ds.Rows[i].Field<int?>("EventLevel");
                    Equip.Pb = _ds.Rows[i].Field<UInt64>("Pb") == 1;
                    Equip.Txcs = _ds.Rows[i].Field<int?>("Txcs");
                    Equip.DataStatus = DataStatus.NONE;
                    Equips.Add(Equip);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备功能列表成功", string.Empty, Equips);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询设备功能列表异常：" + ex.Message);
            }
            return httpResult;
        }
    }
}
