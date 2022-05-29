using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Util.Collections
{
    public struct MapKey<V>
    {
        public int hash;
        public int idealposition;
        public int psl;
        public V value;

        public static MapKey<V> Empty { 
            get {
                return new MapKey<V>() {
                    hash = 0,
                    psl = -1,
                    idealposition = -1,
                    value = default(V)
                }; 
            } 
        }
    }

    public enum HashMapDuplicateMode
    {
        ReplaceOnDuplicate,
        ExceptOnDuplicate
    }

    public class DynamicHashMap<K, V>
    {
        private readonly int DEFAULT_SIZE = 32;
        public MapKey<V>[] map;

        private int items;
        public int Items { get { return items; } }
        public HashMapDuplicateMode DuplicateMode { get; private set; }



        public DynamicHashMap(uint reserve)
        {
            DuplicateMode = HashMapDuplicateMode.ReplaceOnDuplicate;
            Init((int)reserve);
            
        }

        public DynamicHashMap(uint reserve, HashMapDuplicateMode mode)
        {
            DuplicateMode = mode;
            Init((int)reserve);

        }

        public DynamicHashMap()
        {
            Init(DEFAULT_SIZE);
        }

        private void Init(int reserve)
        {
            this.items = 0;
            this.map = new MapKey<V>[reserve];
            for(int i =0; i< map.Length; i++)
            {
                map[i] = MapKey<V>.Empty;
            }
        }


        private int GetIdealPosition(int hashcode)
        {
            return (int)(((uint)hashcode) % map.Count());
        }

        private void Expand()
        {
            MapKey<V>[] oldmap = new MapKey<V>[this.map.Length];
            Array.Copy(map, oldmap, map.Length);

            Init(oldmap.Length * 2);

            for(int i = 0; i < oldmap.Length; i++)
            {
                MapKey<V> item = oldmap[i];
                if(item.psl != -1)
                {
                    this.NativeInsert(item.hash, item.value);
                }
            }
        }

        private void NativeInsert(int hashkey, V _value)
        {
            MapKey<V> item = new MapKey<V>
            {
                hash = hashkey,
                psl = 0,
                value = _value,
                idealposition = GetIdealPosition(hashkey)
            };
            int idealposition = item.idealposition;
            int currentposition = idealposition;
            bool duplicatedetected = false;



            while (map[currentposition].psl != -1)
            {
                MapKey<V> lookatitem = map[currentposition];

                if (lookatitem.hash != item.hash)
                {
                    if (lookatitem.psl < item.psl)
                    {
                        map[currentposition] = item;
                        item = lookatitem;
                    }
                    item.psl += 1;
                    currentposition += 1;

                    if (currentposition == map.Length)
                    {
                        this.Expand();
                        item.idealposition = GetIdealPosition(item.hash);
                        idealposition = item.idealposition;
                        currentposition = idealposition;
                    }
                } else
                {
                    duplicatedetected = true;
                    if(DuplicateMode == HashMapDuplicateMode.ExceptOnDuplicate)
                    {
                        throw new Exception("DynamicHashMap has encountered a duplicate");
                    } else
                    {
                        break;
                    }
                }
            }


            map[currentposition] = item;

            //prevent affecting the item count on insertion of duplicate items
            if(!duplicatedetected)
            {
                this.items += 1;
            }

        }

        public V this[K key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Insert(K _key, V _value)
        {
            if (this.items == map.Length)
            {
                //if the item count equals the map size then expand first before we insert
                this.Expand();
            }
            NativeInsert(_key.GetHashCode(), _value);
        }

        

        public void Delete(K key)
        {
            throw new NotImplementedException("Deletion operator");
        }

        public bool Contains(K key)
        {
            throw new NotImplementedException("Containment operator");
        }

        public bool Replace(K key, V newvalue)
        {
            throw new NotImplementedException("Replacement operator");
        }
    }
}
