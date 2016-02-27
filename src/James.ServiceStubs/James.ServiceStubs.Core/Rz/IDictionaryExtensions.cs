using System.Collections.Generic;

namespace James.ServiceStubs.Core.Rz
{
    public static class IDictionaryExtensions
    {
        public static MassiveExpando ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new MassiveExpando();
            var expandoDictionary = (IDictionary<string, object>) expando;

            foreach (var kvp in dictionary)
            {
                var value = kvp.Value as IDictionary<string, object>;

                if (value == null)
                {
                    expandoDictionary.Add(kvp);
                }
                else
                {   
                    var expandoValue = value.ToExpando();
                    expandoDictionary.Add(kvp.Key, expandoValue);
                }
            }

            return expando;
        }
    }
}