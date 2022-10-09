using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UserManagement;

namespace EquipDataManager
{
    public class Pub
    {
        public static object IsDBNull(object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            else
                return obj;
        }
        static string logmodal;
        static string LogModal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(logmodal))
                {
                    logmodal = ConfigHelper.GetConfigString("log:model");
                }
                if (string.IsNullOrWhiteSpace(logmodal))
                {
                    logmodal = "error";
                }
                return logmodal;
            }
        }
        public static void AddTestLog(string msg)
        {
            if (LogModal == "all")
            {
                Log.Add(msg);
            }
        }
        public static bool CheckQx(string user, string equipid, string groupid)
        {
            lock (UserService.UserRoles)
            {
                var _qx = UserService.UserRoles.Find(n => n.User == user);
                return (_qx == null ||
                                                (
                                                    (
                                                           string.IsNullOrWhiteSpace(_qx.GroupQx)
                                                           || _qx.GroupQx.Split(',').Contains(groupid)
                                                    )
                                                    &&
                                                    (
                                                        string.IsNullOrWhiteSpace(_qx.EquipQx)
                                                        || _qx.EquipQx.Split(',').Contains(equipid)
                                                     )
                                                 )
                                                );
            }
        }
    }
}
