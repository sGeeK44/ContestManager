using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace Contest.Core.Filebase
{
    public class FileStrategy
    {
        private string _filePath;
        private readonly IList<object> _insertList = new List<object>();
        private readonly IList<object> _updateList = new List<object>();
        private readonly IList<object> _removeList = new List<object>();

        public FileStrategy(string filePath)
        {
            SetNewFile(filePath);
        }

        public void SetNewFile(string filePath)
        {
            _filePath = filePath;
        }

        public void Insert<T>(T itemToInsert)
        {
            AddItem(_insertList, itemToInsert);
        }

        public void Update<T>(T itemToUpdate)
        {
            AddItem(_updateList, itemToUpdate);
        }

        public void Delete<T>(T itemToDelete)
        {
            AddItem(_removeList, itemToDelete);
        }

        public IList<T> Find<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var formatter = new BinaryFormatter();
            if (!File.Exists(_filePath)) return new List<T>();
            using (var stream = new MemoryStream(File.ReadAllBytes(_filePath)))
            {
                stream.Position = 0;
                IList<object> list = formatter.Deserialize(stream) as List<object>;
                return list == null ? new List<T>() : list.Where(_ => _.GetType() == typeof(T)).Cast<T>().Where(predicate.Compile()).ToList();
            }
        }

        public void Commit()
        {
            var formatter = new BinaryFormatter();
            IList<object> listToCommit = null;
            if (File.Exists(_filePath))
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(_filePath)))
                {
                    stream.Position = 0;
                    listToCommit = formatter.Deserialize(stream) as List<object>;
                }
            }
            if (listToCommit == null) listToCommit = new List<object>();

            foreach (var itemToInsert in _insertList)
            {
                AddItem(listToCommit, itemToInsert);
            }

            foreach (var itemToUpdate in _updateList)
            {
                AddItem(listToCommit, itemToUpdate);
            }

            foreach (var itemToRemove in _removeList)
            {
                var item = listToCommit.FirstOrDefault(itemToRemove.Equals);
                if (item != null) listToCommit.Remove(item);
            }

            using (var stream = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(stream, listToCommit);
                    stream.Position = 0;
                    File.WriteAllBytes(_filePath, stream.GetBuffer());

                }
                catch (Exception)
                {
                    RollBack();
                    throw;
                }
            }
            _insertList.Clear();
            _removeList.Clear();
            _updateList.Clear();
        }

        public void RollBack()
        {
            _insertList.Clear();
            _removeList.Clear();
            _updateList.Clear();
        }

        private static void AddItem<T>(IList<object> list, T itemSearch)
        {
            if (list == null) return;
            var item = list.FirstOrDefault(itemSearch.Equals);
            if (item != null) list.Remove(item);
            list.Add(itemSearch);
        }

        private static void AddItem(IList<object> list, object itemSearch)
        {
            if (list == null) return;
            var item = list.FirstOrDefault(itemSearch.Equals);
            if (item != null) list.Remove(item);
            list.Add(itemSearch);
        }
    }
}
