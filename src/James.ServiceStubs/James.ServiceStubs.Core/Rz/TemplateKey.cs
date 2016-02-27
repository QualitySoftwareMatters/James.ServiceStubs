using RazorEngine.Templating;

namespace James.ServiceStubs.Core.Rz
{
    public class TemplateKey : ITemplateKey
    {
        public TemplateKey(string templateKey)
        {
            Name = templateKey;
            TemplateType = ResolveType.Global;
            Context = this;
        }
        public string GetUniqueKeyString()
        {
            return Name;
        }

        public string Name { get; }
        public ResolveType TemplateType { get; }
        public ITemplateKey Context { get; }
    }
}