using System;
using System.Collections.Generic;

namespace AlgorithmLRU
{
    public class CacheLRU
    {
        public int? LastRemovedKey { get; private set; }
        public LinkedList<int> _priorityQ { get; }
        private int _capacity;

        public int Count => _priorityQ.Count;

        public CacheLRU(int capacity)
        {
            _priorityQ = new LinkedList<int>();
            _capacity = capacity;
        }

        public bool Contains(int key) => _priorityQ.Contains(key);

        public void Add(int key)
        {
            var node = _priorityQ.Find(key);
            // If key is already in the table, update it
            if (node != null)
            {
                _priorityQ.Remove(node);
                _priorityQ.AddFirst(node);
                return;
            }

            if (_priorityQ.Count >= _capacity)
            {
                if (_priorityQ.Last == null)
                    throw new NullReferenceException("The last element is missing in LRU cache");
                LastRemovedKey = _priorityQ.Last.Value;
                _priorityQ.RemoveLast();
            }

            _priorityQ.AddFirst(key);
        }
    }

    /// <summary>
    /// A Low Overhead High Performance Buffer Management Replacement Algorithm.
    /// </summary>
    /// <seealso cref="http://www.vldb.org/conf/1994/P439.PDF"/>
    /// <typeparam name="V">Type of data in cache</typeparam>
    public class Cache2Q<V>
    {
        /// <summary>
        /// LRU cache for 2Q algorithm
        /// </summary>
        private CacheLRU _hotQ;

        /// <summary>
        /// Hash table with cache data
        /// </summary>
        public Dictionary<int, V> _hashTable { get; }
        public LinkedList<int> _inQ { get; }
        public LinkedList<int> _outQ { get; }
        public int _inQCapacity, _outQCapacity, _hotQCapacity;

        /// <summary>
        /// Information about the last removed node to display to the UI
        /// </summary>
        public Tuple<int, V>? LastRemovedNode { get; private set; }

        /// <summary>
        /// Returns LRU cache list
        /// </summary>
        /// <returns>LRU cache list</returns>
        public LinkedList<int> GetLRUCache() => _hotQ._priorityQ;

        public Cache2Q(int cacheSize)
        {
            _inQCapacity = (int)(0.2 * cacheSize);
            _outQCapacity = (int)(0.6 * cacheSize);
            _hotQCapacity = cacheSize - _inQCapacity - _outQCapacity;
            _inQ = new LinkedList<int>();
            _outQ = new LinkedList<int>();
            _hotQ = new CacheLRU(_hotQCapacity);
            _hashTable = new Dictionary<int, V>(cacheSize);
        }

        /// <summary>
        /// Add data to cache
        /// </summary>
        /// <param name="key">data key</param>
        /// <param name="value">storage data</param>
        public void Add(int key, V value)
        {
            // If the key is already in the cache, update its value
            if (_hashTable.ContainsKey(key))
            {
                _hashTable[key] = value;
                return;
            }
            // If there are free page slots then put value into a free page slot
            bool hasFreeSlot = false;
            if (_inQ.Count < _inQCapacity)
            {
                _inQ.AddFirst(key);
                hasFreeSlot = true;
            }
            else if (_outQ.Count < _outQCapacity)
            {
                _outQ.AddFirst(key);
                hasFreeSlot = true;
            }

            if (hasFreeSlot)
            {
                _hashTable.Add(key, value);
                return;
            }

            // Page out the tail of Alin, call it Y
            if (_inQ.Last == null)
                throw new NullReferenceException("The last element is missing in input queue");
            int yKey = _inQ.Last.Value;
            _inQ.RemoveLast();
            // Add identifier of Y to the head of Alout
            _outQ.AddFirst(yKey);
            if (_outQ.Count > _outQCapacity)
            {
                if (_outQ.Last == null)
                    throw new NullReferenceException("The last element is missing in LRU cache");
                int zKey = _outQ.Last.Value;
                _hashTable.TryGetValue(zKey, out V valueToDelete);
                if (valueToDelete == null)
                    throw new NullReferenceException($"No value in cache for key {zKey}");
                LastRemovedNode = new Tuple<int, V>(zKey, valueToDelete);
                _hashTable.Remove(zKey);
                // remove identifier of Z from the tail of Alout
                _outQ.RemoveLast();
            }
            // Put X into the reclaimed page slot
            _inQ.AddFirst(key);
            _hashTable.Add(key, value);
        }

        /// <summary>
        /// Returns data from the cache by key
        /// </summary>
        /// <param name="key">key to get data</param>
        /// <returns>Data from cache</returns>
        public V Get(int key)
        {
            V v = default(V);
            if (_hashTable.TryGetValue(key, out v))
            {
                if (_hotQ.Contains(key))
                {
                    // Move X to the head of Am
                    _hotQ.Add(key);
                }
                else if (_outQ.Contains(key))
                {
                    _hotQ.Add(key);
                    _outQ.Remove(key);
                    if (_hotQ.LastRemovedKey != null)
                    {
                        int keyToDelete = _hotQ.LastRemovedKey.Value;
                        LastRemovedNode = new Tuple<int, V>(keyToDelete, _hashTable[keyToDelete]);
                        _hashTable.Remove(keyToDelete);
                    }
                }
            }
            return v;
        }
    }
}
