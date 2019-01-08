using System;

namespace Mysoft.Magicodes.Core
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Domain
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public virtual string UpdateBy { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime DeleteDate { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public virtual string DeleteBy { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public virtual bool EnableFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
