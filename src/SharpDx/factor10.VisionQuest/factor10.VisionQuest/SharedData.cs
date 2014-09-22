using System.Drawing;

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
        }

        public Size Size { get; set; }

    }
}