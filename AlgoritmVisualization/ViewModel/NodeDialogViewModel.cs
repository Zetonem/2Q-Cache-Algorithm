using System;

namespace AlgorithmVisualization
{
    internal class NodeDialogViewModel : ViewModelBase
    {
        private int _key;

        public int Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        private String _data;

        public String Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        private String _text;
        public String Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public NodeDialogViewModel(Tuple<int, String> data)
        {
            Key = data.Item1;
            Data = data.Item2;
            Text = "Key: " + Key + "\n" + Data;
        }
    }
}