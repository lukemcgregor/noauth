using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;

namespace NoAuth.Site.OAuth
{
    public interface IUserStore
    {
        Dictionary<string, List<Tuple<string, string>>> Users { get; }
    }

    public class InMemoryStore : INonceStore, ICryptoKeyStore, IUserStore
    {
        private Dictionary<string, Dictionary<string, CryptoKey>> _keys = new Dictionary<string, Dictionary<string, CryptoKey>>();

        public Dictionary<string, List<Tuple<string, string>>> Users { get; set; } = new Dictionary<string, List<Tuple<string, string>>>();

        public CryptoKey GetKey(string bucket, string handle)
        {
            if (_keys.ContainsKey(bucket) && _keys[bucket].ContainsKey(handle))
            {
                return _keys[bucket][handle];
            }
            return null;
        }

        public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket)
        {

            if (_keys.ContainsKey(bucket))
            {
                return _keys[bucket].ToList();
            }
            return new List<KeyValuePair<string, CryptoKey>>();
        }

        public void RemoveKey(string bucket, string handle)
        {
            if (_keys.ContainsKey(bucket))
            {
                _keys[bucket].Remove(handle);
            }
        }

        public void StoreKey(string bucket, string handle, CryptoKey key)
        {
            if (!_keys.ContainsKey(bucket))
            {
                _keys.Add(bucket, new Dictionary<string, CryptoKey>());
            }
            _keys[bucket][handle] = key;
        }

        public bool StoreNonce(string context, string nonce, DateTime timestampUtc)
        {
            return true;
        }
    }
}