using EquipDataManager.Bll;
using EquipDataManager.Relealise;
using EquipModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThirdParty.Json.LitJson;
using Tools;

namespace UITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log.LogEvent = AddLog;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataPicker.Instance.Start(new DataPickerRealise(), AddLog);
        }

        private void AddLog(string message)
        {
            rtb.Dispatcher.Invoke(() =>
            {
                rtb.AppendText(message + "\r\n");
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Stopwatch _sw = new Stopwatch();
                _sw.Start();
                dg.ItemsSource = null;
                List<EquipSstjData> data = DataPicker.Instance.GetEquipSstjDataByType(tb_lxid.Text);
                rtb_lx.Document.Blocks.Clear();
                rtb_lx.AppendText(JsonMapper.ToJson(data));
                dg.ItemsSource = data;
                _sw.Stop();
                MessageBox.Show("获取按类型实时统计用时:"+_sw.ElapsedMilliseconds+"毫秒");
                _sw.Restart();

                dg1.ItemsSource = null;
                List<EquipSstjData> data1 = DataPicker.Instance.GetEquipSstjDataByGroup(tb_cxid.Text);
                rtb_g.Document.Blocks.Clear();
                rtb_g.AppendText(JsonMapper.ToJson(data1));
                dg1.ItemsSource = data1;
                _sw.Stop();
                MessageBox.Show("获取按产线实时统计用时:" + _sw.ElapsedMilliseconds + "毫秒");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dg_hc.ItemsSource = null;
            dg_hc.ItemsSource = DataPicker.Instance.dataCache;
            rtb_hc.Document.Blocks.Clear();
            rtb_hc.AppendText(JsonMapper.ToJson(DataPicker.Instance.dataCache));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DataPicker.Instance.LoadFile();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SocketListen.Instance.Start(AddLog);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                if (db_cdlb.SelectedItem != null)
                {
                    List<EquipData_Ls> _data = DataPicker.Instance.GetLssj(db_cdlb.SelectedValue.ToString(), ksrq.SelectedDate.Value, jsrq.SelectedDate.Value);
                    dg_ls.ItemsSource = _data;
                    rtb_qx.Document.Blocks.Clear();
                    rtb_qx.AppendText(JsonMapper.ToJson(_data));
                }
                else
                {
                    MessageBox.Show("请选择测点");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ExecBD(string bd)
        {
            try
            {
                DBHelper.ExecuteCommand(bd);
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("表补丁,将删除表进行重建!", "提醒", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }
            ExecBD(@"DROP TABLE IF EXISTS `Equip`;
CREATE TABLE `Equip`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EquipNO` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '设备编号',
  `EquipName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `TypeBinding` int(11) NULL DEFAULT NULL COMMENT '类型绑定',
  `ProductionLineGroupBinding` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产线组绑定',
  `GatewayId` int(11) NULL DEFAULT NULL COMMENT '网关ID',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 34 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");

            ExecBD(@"DROP TABLE IF EXISTS `EquipGroup`;
CREATE TABLE `EquipGroup`  (
  `EquipGroupId` int(11) NOT NULL AUTO_INCREMENT,
  `EquipGroupName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`EquipGroupId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"DROP TABLE IF EXISTS `EquipSpot`;
CREATE TABLE `EquipSpot`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `EquipNo` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '设备编号',
  `Type` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '测点类型',
  `SpotNo` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '测点编号',
  `Unit` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '单位',
  `Value` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '当前实时值',
  `SpotNm` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '名称',
  `State` int(11) NULL DEFAULT NULL COMMENT '处理后的当前状态',
  `NoState` int(11) NULL DEFAULT NULL COMMENT '当前状态',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"DROP TABLE IF EXISTS `EquipSpotSet`;
CREATE TABLE `EquipSpotSet`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Id',
  `EquipType` int(11) NULL DEFAULT NULL COMMENT '设备类型',
  `DataType` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '数据类型',
  `SpotNO` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '测点编号',
  `SaveType` int(11) NULL DEFAULT NULL COMMENT '保存类型',
  `EquipId` int(11) NULL DEFAULT NULL,
  `SpotNm` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"DROP TABLE IF EXISTS `EquipType`;
CREATE TABLE `EquipType`  (
  `EquipTypeId` int(11) NOT NULL AUTO_INCREMENT COMMENT '设备类型Id',
  `EquipTypeName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '设备类型名称',
  PRIMARY KEY (`EquipTypeId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"DROP TABLE IF EXISTS `Gateway`;
CREATE TABLE `Gateway`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Id',
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '网关名称',
  `IP` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'IP',
  `PORT` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '端口',
  `EquipGroupId` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"DROP TABLE IF EXISTS `ProductionLine`;
CREATE TABLE `ProductionLine`  (
  `ProductionLineId` int(11) NOT NULL AUTO_INCREMENT COMMENT '产线Id',
  `ProductionLineName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产线名称',
  PRIMARY KEY (`ProductionLineId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"DROP TABLE IF EXISTS `User`;
CREATE TABLE `User`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Id',
  `UserName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '用户名',
  `RoleGroup` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色组',
  `PassWord` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '密码',
  `ThemeColor` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '主题颜色',
  `LastLoginTime` datetime NULL DEFAULT NULL COMMENT '最后一次登录时间',
  `Remarks` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ZoneBinding` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '区域绑定',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");

            ExecBD(@"DROP TABLE IF EXISTS `equipevent`;
CREATE TABLE `equipevent`  (
  `ID` int NOT NULL AUTO_INCREMENT,
  `GroupID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `TypeID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `EquipName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `devID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `SpotName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `SpotID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `event` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Proc` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `StartTime` datetime NULL DEFAULT NULL,
  `endTime` datetime NULL DEFAULT NULL,
  `DValue` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Level` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `isconfirm` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `confirmor` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `confirm_time` datetime NULL DEFAULT NULL,
  `confirm_opinion` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `ID`(`ID` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;");

            ExecBD(@"DROP TABLE IF EXISTS `EquipTjSet`;
CREATE TABLE `EquipTjSet`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Id',
  `EquipType` int(11) NULL DEFAULT NULL COMMENT '设备类型',
  `DataType` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '数据类型',
  `SpotNO` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '测点编号',
  `Tjlx` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '统计类型',
  `Tjzt` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '统计状态',
  `Data` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '对应值',
  `Tjtj` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '统计条件',
  `Cyhj` bit(1) NULL DEFAULT NULL COMMENT '参与合计',
  `Cyzshj` bit(1) NULL DEFAULT NULL COMMENT '参与总数合计',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;");
            ExecBD(@"insert into User(UserName,PassWord) values('Admin','6u1n1q7jQ5A=')");
            MessageBox.Show("补丁执行完成!");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<object> _lb = DataPicker.Instance.GetLssjcdlb(tb_group.Text);
                db_cdlb.ItemsSource = _lb;
                rtb_ls.Document.Blocks.Clear();
                rtb_ls.AppendText(JsonMapper.ToJson(_lb.ToList()));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            object _obj = DataPicker.Instance.GetDataByGroup(tb_cx.Text);
            string _json = JsonMapper.ToJson(_obj);
            rtb_1.Document.Blocks.Clear();
            rtb_1.AppendText(_json);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            SaveFileDialog _sfd = new SaveFileDialog();
            if (_sfd.ShowDialog() == true)
            {
                Tools.XmlHelper.SaveListToXml(_sfd.FileName, DataPicker.Instance.dataCache);
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            try
            {
                dg_sj.DataContext = EquipDataManager.Dal.EquipDataDal.GetEvent(sj_cx.Text, sj_lx.Text, sj_ks.SelectedDate.Value, sj_js.SelectedDate.Value, sj_sjlx.Text, sj_gjz.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
