using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AlgorithmLRU;

namespace AlgorithmVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int? _lastAddedKey = null;
        private int? _lastReceivedKey = null;
        public static bool IsDataAdded = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Output();
        }

        private Cache2Q<String> GetCache2Q()
        {
            return ((MainViewModel)DataContext).Cache;
        }

        private void Output()
        {
            keyComboBox.Items.Clear();
            OutputHotQ();
            OutputInQ();
            OutputOutQ();
            OutputLastRemovedNode();
        }

        private void OutputHotQ()
        {
            hotQPanel.Children.Clear();
            var hotQ = GetCache2Q().GetLRUCache();
            foreach (var key in hotQ)
            {
                Button dataButton = new Button
                {
                    Content = key.ToString(),
                    Width = 128,
                    CommandParameter = key
                };
                if (_lastAddedKey.HasValue && _lastAddedKey.Value == key)
                    dataButton.Background = new SolidColorBrush(Color.FromRgb(29, 185, 84));
                else if (_lastReceivedKey.HasValue && _lastReceivedKey.Value == key)
                    dataButton.Background = new SolidColorBrush(Color.FromRgb(35, 170, 242));
                dataButton.SetBinding(Button.CommandProperty, new Binding("RunDialogCommand"));
                hotQPanel.Children.Add(dataButton);
                keyComboBox.Items.Add(new ComboBoxItem() { Content = key.ToString() });
            }
        }

        private void OutputInQ()
        {
            inQPanel.Children.Clear();
            var inQ = GetCache2Q()._inQ;
            foreach (var key in inQ)
            {
                Button dataButton = new Button
                {
                    Content = key.ToString(),
                    Width = 128,
                    CommandParameter = key
                };
                if (_lastAddedKey.HasValue && _lastAddedKey.Value == key)
                    dataButton.Background = new SolidColorBrush(Color.FromRgb(29, 185, 84));
                else if (_lastReceivedKey.HasValue && _lastReceivedKey.Value == key)
                    dataButton.Background = new SolidColorBrush(Color.FromRgb(35, 170, 242));
                dataButton.SetBinding(Button.CommandProperty, new Binding("RunDialogCommand"));
                inQPanel.Children.Add(dataButton);
                keyComboBox.Items.Add(new ComboBoxItem() { Content = key.ToString() });
            }
        }

        private void OutputOutQ()
        {
            outQPanel.Children.Clear();
            var outQ = GetCache2Q()._outQ;
            foreach (var key in outQ)
            {
                Button dataButton = new Button
                {
                    Content = key.ToString(),
                    Width = 128,
                    CommandParameter = key
                };
                if (_lastAddedKey.HasValue && _lastAddedKey.Value == key)
                    dataButton.Background = new SolidColorBrush(Color.FromRgb(29, 185, 84));
                else if (_lastReceivedKey.HasValue && _lastReceivedKey.Value == key)
                    dataButton.Background = new SolidColorBrush(Color.FromRgb(35, 170, 242));
                dataButton.SetBinding(Button.CommandProperty, new Binding("RunDialogCommand"));
                outQPanel.Children.Add(dataButton);
                keyComboBox.Items.Add(new ComboBoxItem() { Content = key.ToString() });
            }
        }

        private void OutputLastRemovedNode()
        {
            var lastRemovedNode = GetCache2Q().LastRemovedNode;
            if (lastRemovedNode != null)
            {
                ((MainViewModel)DataContext).LastRemovedKey = lastRemovedNode.Item1;
                ((MainViewModel)DataContext).LastRemovedData = lastRemovedNode.Item2;
                int key = lastRemovedNode.Item1;
                Button dataButton = new Button
                {
                    Content = key.ToString(),
                    Width = 128,
                    Background = new SolidColorBrush(Color.FromRgb(224, 1, 46)),
                };
                lastRemovedButton.Content = key.ToString();
                lastRemovedButton.IsEnabled = true;
                lastRemovedButton.Background = new SolidColorBrush(Color.FromRgb(224, 1, 46));
            }
            else
            {
                lastRemovedButton.Content = "NULL";
                lastRemovedButton.IsEnabled = false;
                lastRemovedButton.Background = new SolidColorBrush(Color.FromRgb(103, 58, 183));
            }
        }

        private void addDataButton_Click(object sender, RoutedEventArgs e)
        {
            String keyS = keyTextBox.Text, data = dataTextBox.Text;
            if (keyS == String.Empty && data == String.Empty)
                return;
            if (keyS != String.Empty && data != String.Empty)
            {
                int key;
                if (Int32.TryParse(keyS, out key))
                {
                    GetCache2Q().Add(key, data);
                    _lastAddedKey = key;
                    Validation.ClearInvalid(keyTextBox.GetBindingExpression(TextBox.TextProperty));
                    Validation.ClearInvalid(dataTextBox.GetBindingExpression(TextBox.TextProperty));
                    IsDataAdded = true;
                    keyTextBox.Text = String.Empty;
                    IsDataAdded = true;
                    dataTextBox.Text = String.Empty;
                    Output();
                }
            }
        }

        private async void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            String keyS = keyComboBox.Text;
            if (keyS == String.Empty)
                return;
            if (keyS != String.Empty)
            {
                int key;
                if (Int32.TryParse(keyS, out key))
                {
                    await ((MainViewModel)DataContext).RunGetDataComboBox(keyS);
                    _lastReceivedKey = key;
                    Output();
                }
            }
        }
    }
}
