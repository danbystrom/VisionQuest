using System.Xml;
using System.Collections;
using System.IO;

namespace factor10.VisionaryHeads
{

    public class SimpleXmlReader
    {
        protected XmlDocument Document;
        public XmlNode CurrentNode { get; protected set; }
        protected Stack _stack;

        protected XmlNodeList _collection;
        protected int _nCollectionCurrent;

        public SimpleXmlReader()
        {
            Document = new XmlDocument();
            CurrentNode = Document;
            _stack = new Stack();
        }

        public void LoadFile(string filename)
        {
            Document.Load(filename);
        }

        public void LoadFile(Stream stream)
        {
            Document.Load(stream);
        }

        public void LoadXml(string xml)
        {
            Document.LoadXml(xml);
        }

        public string this[string tag]
        {
            get
            {
                var attr = CurrentNode.Attributes[tag];
                return attr != null ? attr.Value : null;
            }
        }

        public int getValueAsInt(string tag)
        {
            return getValueAsInt(tag, 0);
        }

        public int getValueAsInt(string tag, int defaultValue)
        {
            var attr = CurrentNode.Attributes[tag];
            if (attr != null)
                int.TryParse(attr.Value, out defaultValue);
            return defaultValue;
        }

        public double getValueAsDouble(string tag, double defaultValue)
        {
            var attr = CurrentNode.Attributes[tag];
            if (attr != null)
                double.TryParse(
                    attr.Value,
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out defaultValue);
            return defaultValue;
        }

        public bool getValueAsBool(string tag)
        {
            return string.CompareOrdinal("Y", this[tag]) == 0;
        }

        private void push()
        {
            _stack.Push(new object[] { CurrentNode, _collection, _nCollectionCurrent });
        }

        private void pop()
        {
            var aobj = (object[])_stack.Pop();
            CurrentNode = (XmlNode)aobj[0];
            _collection = (XmlNodeList)aobj[1];
            _nCollectionCurrent = (int)aobj[2];
        }

        public bool descend(string tag)
        {
            push();
            CurrentNode = CurrentNode.SelectSingleNode(tag);
            if (CurrentNode == null)
            {
                ascend();
                return false;
            }
            return true;
        }

        public void ascend()
        {
            pop();
        }

        public void descendCollection(string tag)
        {
            push();
            _collection = CurrentNode.SelectNodes(tag);
            _nCollectionCurrent = -1;
        }

        public bool nextInCollection()
        {
            if (_collection == null || ++_nCollectionCurrent >= _collection.Count)
            {
                ascend();
                return false;
            }

            CurrentNode = _collection[_nCollectionCurrent];
            return true;
        }

    }

}
