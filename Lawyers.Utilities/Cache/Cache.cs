using System.Collections.Generic;

namespace Lawyers.Utilities
{
    public class Cache
    {
        private SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
        private static volatile Cache instance = null;
        private static object lockHelper = new object();

        private Cache() { }
        public void Add(string key, object value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);
        }
        public void Remove(string key)
        {
            if (dic.ContainsKey(key))
                dic.Remove(key);
        }

        public object this[string index]
        {
            get
            {
                if (dic.ContainsKey(index))
                    return dic[index];
                else
                    return null;
            }
            set { dic[index] = value; }
        }

        public static Cache Instance
        {
            get
            {
                //instance = null;
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new Cache();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
