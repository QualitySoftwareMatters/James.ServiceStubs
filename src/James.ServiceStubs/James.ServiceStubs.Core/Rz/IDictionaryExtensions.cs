using System.Collections.Generic;

namespace James.ServiceStubs.Core.Rz
{
    public static class IDictionaryExtensions
    {
        public static MassiveExpando ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new MassiveExpando();
            var expandoDic = (IDictionary<string, object>) expando;

            foreach (var kvp in dictionary)
            {
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>) kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                //else if (kvp.Value is ICollection)
                //{
                //    var itemList = new List<object>();
                //    foreach (var item in (ICollection) kvp.Value)
                //    {
                //        if (item is IDictionary<string, object>)
                //        {
                //            var expandoItem = ((IDictionary<string, object>) item).ToExpando();
                //            itemList.Add(expandoItem);
                //        }
                //        else
                //        {
                //            itemList.Add(item);
                //        }
                //    }

                //    expandoDic.Add(kvp.Key, itemList);
                //}
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }
    }
}