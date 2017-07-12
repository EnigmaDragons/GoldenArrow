using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.Core.Entities
{
    public class Items : IList<GameObject>
    {
        private readonly List<GameObject> _objList = new List<GameObject>();

        public int Count => _objList.Count;
        public bool IsReadOnly => false;

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _objList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(GameObject item)
        {
            if (Contains(item))
                Remove(item);
            _objList.Add(item);
        }

        public void Clear()
        {
            _objList.Clear();
        }

        public bool Contains(GameObject item)
        {
            return _objList.Any(x => x.Id == item.Id);
        }

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            _objList.CopyTo(array, arrayIndex);
        }

        public bool Remove(GameObject item)
        {
            return _objList.Remove(_objList.First(x => x.Id == item.Id));
        }

        public int IndexOf(GameObject item)
        {
            return _objList.FindIndex(x => x.Id == item.Id);
        }

        public void Insert(int index, GameObject item)
        {
            if (Contains(item))
            {
                index = IndexOf(item) >= index ? index : index - 1;
                Remove(item);
            }
            if (index == Count)
                Add(item);
            else
                _objList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _objList.RemoveAt(index);
        }

        public GameObject this[int index]
        {
            get { return _objList[index]; }
            set { Insert(index, value); }
        }
    }
}
