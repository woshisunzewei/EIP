using System;

namespace EIP.Common.Entities.Dtos
{
    /// <summary>
    /// 以传递一个实体Id值给应用服务方法。
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class IdInput<TId> : IInputDto
    {
        public TId Id { get; set; }

        public IdInput()
        {
        }

        public IdInput(TId id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Id类型为Guid的一个快捷实现
    /// </summary>
    public class IdInput : IdInput<Guid>
    {
        public IdInput()
        {

        }

        public IdInput(Guid id)
            : base(id)
        {

        }
    }
}