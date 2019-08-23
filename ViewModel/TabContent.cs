using System.ComponentModel;

namespace YueDroidBox.ViewModel
{
    public class TabContent
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string _header;
        private readonly object _content;

        public TabContent(string header, object content)
        {
            _header = header;
            _content = content;
        }

        public string Header
        {
            get { return _header; }
        }

        public object Content
        {
            get { return _content; }
        }
    }
}