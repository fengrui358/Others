using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Nelibur.ObjectMapper;

namespace FormulaDemo.Models
{
    public abstract class FormulaItemModel : ObservableObject, ICloneable
    {
        /// <summary>
        /// 显示
        /// </summary>
        public string Content { get; protected set; }

        /// <summary>
        /// 是否已处于编辑面板
        /// </summary>
        public bool IsActive { get; set; }

        public object Clone()
        {
            var newInstance = Activator.CreateInstance(GetType());
            TinyMapper.Map(this, newInstance);

            return newInstance;
        }
    }
}
