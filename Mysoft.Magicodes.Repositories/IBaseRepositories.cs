using System;
using System.Collections.Generic;
using System.Text;

namespace Mysoft.Magicodes.Repositories
{
    /// <summary>
    /// 基类业务接口定义
    /// </summary>
    public interface IBaseRepositories<T> where T : class
    {
        bool Create(T entity);

        T RetriveById(string id);

        bool Update(T entity);

        bool Delete(string id);
    }
}
