namespace SFB
{
    public struct ExtensionFilter
    {
        public string name;
        public string[] extensions;


        public ExtensionFilter(string _filterName, params string[] _filterExtensions)
        {
            name = _filterName;
            extensions = _filterExtensions;
        }
    }
}
