//using System.Collections.Generic;
//using System.Reflection;

//namespace EIP.Common.Core.Ldap
//{

//    public class EntityCollection<T> : IList<T>, IEntityCollection<T>
//    {

//        #region Constructors

//        internal EntityCollection()
//        {
//            list = new List<T>();
//        }

//        #endregion

//        #region Members

//        private List<T> list;

//        #endregion

//        internal protected PropertyInfo PropertyInfo
//        {
//            get;
//            set;
//        }

//        #region Interface Members

//        #region IList<T> Members

//        public int IndexOf(T item)
//        {
//            return list.IndexOf(item);
//        }

//        public void Insert(int index, T item)
//        {
//            list.Insert(index, item);
//            if (this.Parent != null)
//            {
//                (item as DirectoryEntity).Parent = this.Parent.Entry;
//                if (PropertyInfo != null)
//                    PropertyInfo.SetValue(this.Parent, ToStringArray(), null);
//                else if (EntityAddedEvent != null)
//                    EntityAddedEvent(this.Parent.Entry, item as DirectoryEntity);
//            }
//        }

//        public void RemoveAt(int index)
//        {
//            T item = list[index];
//            list.RemoveAt(index);
//            if (this.Parent != null)
//            {
//                (item as DirectoryEntity).Parent = this.Parent.Entry;
//                if (PropertyInfo != null)
//                    PropertyInfo.SetValue(this.Parent, ToStringArray(), null);
//                else if (EntityDeletedEvent != null)
//                    EntityDeletedEvent(item as DirectoryEntity);
//            }
//        }

//        public T this[int index]
//        {
//            get
//            {
//                return list[index];
//            }
//            set
//            {
//                list[index] = value;
//            }
//        }

//        #endregion

//        #region ICollection<T> Members

//        public void Add(T item)
//        {
//            this.list.Add(item);
//            if (this.Parent != null)
//            {
//                (item as DirectoryEntity).Parent = this.Parent.Entry;
//                if (PropertyInfo != null)
//                    PropertyInfo.SetValue(this.Parent, ToStringArray(), null);
//                else if (EntityAddedEvent != null)
//                    EntityAddedEvent(this.Parent.Entry, item as DirectoryEntity);
//            }
//        }

//        public void Clear()
//        {
//            for (int i = list.Count - 1; i >= 0; i--)
//            {
//                this.RemoveAt(i);
//            }
//        }

//        public bool Contains(T item)
//        {
//            return this.list.Contains(item);
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            this.list.CopyTo(array, arrayIndex);
//        }

//        public int Count
//        {
//            get { return list.Count; }
//        }

//        public bool IsReadOnly
//        {
//            get { return false; }
//        }

//        public bool Remove(T item)
//        {
//            bool result = this.list.Remove(item);
//            (item as DirectoryEntity).Parent = this.Parent.Entry;
//            PropertyInfo.SetValue(this.Parent, ToStringArray(), null);
//            if (EntityDeletedEvent != null)
//                EntityDeletedEvent(item as DirectoryEntity);
//            return result;
//        }

//        #endregion

//        #region IEnumerable<T> Members

//        public IEnumerator<T> GetEnumerator()
//        {
//            return list.GetEnumerator();
//        }

//        #endregion

//        #region IEnumerable Members

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return list.GetEnumerator();
//        }

//        #endregion

//        #region IDisposable Members

//        public void Dispose()
//        {

//        }

//        #endregion

//        #region IEntityCollection<T> Members

//        public DirectoryEntity Parent
//        {
//            get;
//            internal set;
//        }

//        public DirectoryContext DirectoryContext
//        {
//            get;
//            internal set;
//        }

//        public List<T> ToList()
//        {
//            return list;
//        }

//        public string[] ToStringArray()
//        {
//            string[] dNs = new string[this.list.Count];
//            for (int i = 0; i < list.Count; i++)
//            {
//                dNs[i] = (this.list[i] as DirectoryEntity).DistinguishedName;
//            }
//            return dNs;
//        }

//        #endregion

//        #region INotifyEntityAdded Members

//        public event EntityAddedEventHandler EntityAddedEvent;

//        #endregion

//        #region INotifyEntityDeleted Members

//        public event EntityDeletedEventHandler EntityDeletedEvent;

//        #endregion

//        #endregion
//    }
//}
