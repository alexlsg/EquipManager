using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Tools
{
    public class BaseNotifyPropertyChanged : System.ComponentModel.INotifyPropertyChanged
    {
        public DataStatus DataStatus { get; set; } = DataStatus.ADD;
        /// <summary>
        /// 属性值变化时发生
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                if (DataStatus == DataStatus.NONE)
                {
                    DataStatus = DataStatus.MOD;
                }
            }
        }

        /// <summary>
        /// 属性值变化时发生
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = (propertyExpression.Body as MemberExpression).Member.Name;
            this.OnPropertyChanged(propertyName);
        }

        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum DataStatus
    {
        ADD,
        MOD,
        DEL,
        NONE
    }
}
