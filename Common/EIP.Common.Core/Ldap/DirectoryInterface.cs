using System.Collections.Generic;
using System.DirectoryServices;

namespace EIP.Common.Core.Ldap
{

    public interface IPropertyUpdate
    {
        void PropertyUpdate(string propertyName, object value);
    }

    public interface INotifyEntityAdded
    {
        event EntityAddedEventHandler EntityAddedEvent;
    }

    public interface INotifyEntityDeleted
    {
        event EntityDeletedEventHandler EntityDeletedEvent;
    }

    public interface IEntityCollection<T> : INotifyEntityAdded, INotifyEntityDeleted
    {

        #region Properties

        /// <summary>
        /// 父实体
        /// </summary>
        DirectoryEntity Parent { get; }

        /// <summary>
        /// 目录上下文
        /// </summary>
        DirectoryContext DirectoryContext { get; }

        #endregion

        #region Methods

        List<T> ToList();

        string[] ToStringArray();

        #endregion
    }

    public delegate void EntityAddedEventHandler(DirectoryEntry parent, DirectoryEntity sender);
    public delegate void EntityDeletedEventHandler(DirectoryEntity sender);

}
