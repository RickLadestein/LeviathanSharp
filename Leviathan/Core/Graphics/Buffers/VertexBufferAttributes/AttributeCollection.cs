using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public class AttributeCollection : IEnumerable<KeyValuePair<AttributeType, Attribute>>
    {
        Dictionary<AttributeType, Attribute> dict;

        public int Count
        {
            get
            {
                return dict.Keys.Count;
            }
        }

        public AttributeCollection()
        {
            dict = new Dictionary<AttributeType, Attribute>();
        }

        public void ClearAttributes()
        {
            this.dict.Clear();
        }

        public void AddAttribute(Attribute attrib, AttributeType type, bool force_replace = true)
        {
            if (HasAttribute(type) && !force_replace)
            {
                throw new Exception($"AttributeCollection already contains entry of type {type}");
            }
            dict[type] = attrib;
        }

        public void RemoveAttribute(AttributeType type)
        {
            Attribute found;
            bool success = dict.TryGetValue(type, out found);
            if (success)
            {
                dict.Remove(type);
                found.Dispose();
            }
        }

        public bool HasAttribute(AttributeType type)
        {
            return dict.ContainsKey(type);
        }

        public IEnumerator<KeyValuePair<AttributeType, Attribute>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }
    }
}
