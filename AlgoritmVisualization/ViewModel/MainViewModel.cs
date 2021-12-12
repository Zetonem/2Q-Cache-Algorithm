using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AlgorithmLRU;
using MaterialDesignThemes.Wpf;

namespace AlgorithmVisualization
{
    public class MainViewModel : ViewModelBase
    {
        public Cache2Q<String> Cache;

        private String _key;
        private String _data;
        private int _lastRemovedKey;
        private String _lastRemovedData;

        public String Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }
        public String Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }
        public int LastRemovedKey
        {
            get => _lastRemovedKey;
            set => SetProperty(ref _lastRemovedKey, value);
        }
        public String LastRemovedData
        {
            get => _lastRemovedData;
            set => SetProperty(ref _lastRemovedData, value);
        }

        public ICommand RunDialogCommand => new AnotherCommandImplementation(ExecuteRunDialog);
        public ICommand RunDialogLastRemovedCommand => new AnotherCommandImplementation(ExecuteRunDialogLastRemoved);

        public MainViewModel()
        {
            Cache = new Cache2Q<String>(10);
            Cache.Add(0, "adagsodgpoglodlgdpof");
            Cache.Add(1, "b");
            Cache.Add(2, "c");
            Cache.Add(3, "d");
            Cache.Add(4, "e");
            Cache.Add(5, "f");
            Cache.Add(6, "g");
            Cache.Add(7, "h");
            Cache.Add(8, "i");
            Cache.Add(9, "j");
        }

        public async Task RunGetDataComboBox(object o)
        {
            int key = Int32.Parse((String)(o));
            var view = new NodeDialog()
            {
                DataContext = new NodeDialogViewModel(new Tuple<int, String>(key, Cache._hashTable[key])),
            };

            // show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            Cache.Get(key);

            // check the result...
            Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private async void ExecuteRunDialog(object o)
        {
            int key = (int)o;
            var view = new NodeDialog()
            {
                DataContext = new NodeDialogViewModel(new Tuple<int, String>(key, Cache._hashTable[key])),
            };

            // show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            // check the result...
            Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private async void ExecuteRunDialogLastRemoved(object o)
        {
            var view = new NodeDialog()
            {
                DataContext = new NodeDialogViewModel(new Tuple<int, String>(_lastRemovedKey, _lastRemovedData)),
            };

            // show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            // check the result...
            Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");
    }
}