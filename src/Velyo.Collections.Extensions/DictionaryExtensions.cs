using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for generic dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Synchronizes the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Synchronize<TKey, TValue>(this IDictionary<TKey, TValue> origin)
        {
            return new SafeDictionary<TKey, TValue>(origin);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    class SafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        public SafeDictionary(IDictionary<TKey, TValue> origin)
        {
            _origin = origin;
        }

        IDictionary<TKey, TValue> _origin;
        [NonSerialized]
        ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get
            {
                bool lockAcquired = false;
                while (!lockAcquired)
                    lockAcquired = _lock.TryEnterReadLock(1);
                try
                {
                    return _origin.Count;
                }
                finally
                {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                bool lockAcquired = false;
                while (!lockAcquired)
                    lockAcquired = _lock.TryEnterReadLock(1);
                try
                {
                    return _origin.IsReadOnly;
                }
                finally
                {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get
            {
                bool lockAcquired = false;
                while (!lockAcquired)
                    lockAcquired = _lock.TryEnterReadLock(1);
                try
                {
                    return _origin.Keys;
                }
                finally
                {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TValue> Values
        {
            get
            {
                bool lockAcquired = false;
                while (!lockAcquired)
                    lockAcquired = _lock.TryEnterReadLock(1);
                try
                {
                    return _origin.Values;
                }
                finally
                {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <value></value>
        public TValue this[TKey key]
        {
            get
            {
                bool lockAcquired = false;
                while (!lockAcquired)
                    lockAcquired = _lock.TryEnterReadLock(1);
                try
                {
                    return _origin[key];
                }
                finally
                {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
            set
            {
                bool lockAcquired = false;
                while (!lockAcquired)
                    lockAcquired = _lock.TryEnterWriteLock(1);
                try
                {
                    _origin[key] = value;
                }
                finally
                {
                    if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
        /// </exception>
        public void Add(TKey key, TValue value)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterWriteLock(1);
            try
            {
                _origin.Add(key, value);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterWriteLock(1);
            try
            {
                _origin.Add(item);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Clear()
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterWriteLock(1);
            try
            {
                _origin.Clear();
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterReadLock(1);
            try
            {
                return _origin.Contains(item);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool ContainsKey(TKey key)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterReadLock(1);
            try
            {
                return _origin.ContainsKey(key);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterReadLock(1);
            try
            {
                _origin.CopyTo(array, arrayIndex);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotSupportedException("Cannot enumerate a threadsafe dictionary.  Instead, enumerate the keys or values collection");
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException("Cannot enumerate a threadsafe dictionary.  Instead, enumerate the keys or values collection");
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
        /// </exception>
        public bool Remove(TKey key)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterWriteLock(1);
            try
            {
                return _origin.Remove(key);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterWriteLock(1);
            try
            {
                return _origin.Remove(item);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = _lock.TryEnterReadLock(1);
            try
            {
                return _origin.TryGetValue(key, out value);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }
    }
}