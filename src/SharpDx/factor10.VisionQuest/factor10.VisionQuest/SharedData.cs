namespace ShaderLinking
{
    internal sealed class SharedData
    {
        private string _headerText;
        private string _sourceText;
        private string _hiddenText;

        private bool _isShaderDirty;

        private bool _enableInvertColor;
        private bool _enableGrayscale;

        public SharedData()
        {
            Reset();
        }

        public string Header
        {
            get { return _headerText; }
            set
            {
                if (_headerText == value) return;
                _headerText = value;
                _isShaderDirty = true;
            }
        }

        public string Hidden { get { return _hiddenText; } }

        public string Source
        {
            get { return _sourceText; }
            set
            {
                if (_sourceText == value) return;
                _sourceText = value;
                _isShaderDirty = true;
            }
        }

        public bool EnableInvertColor
        {
            get { return _enableInvertColor; }
            set
            {
                if (_enableInvertColor == value) return;
                _enableInvertColor = value;
                _isShaderDirty = true;
            }
        }

        public bool EnableGrayscale
        {
            get { return _enableGrayscale; }
            set
            {
                if (_enableGrayscale == value) return;
                _enableGrayscale = value;
                _isShaderDirty = true;
            }
        }

        public bool IsDirty { get { return _isShaderDirty; } set { _isShaderDirty = value; } }

        public void Reset()
        {
        }
    }
}