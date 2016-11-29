using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace ExchangeRates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XmlOperetions xmlOp = new XmlOperetions();
        List<CurrRatePair> list = new List<CurrRatePair>();
        Task<XmlDocument> task;
        public MainWindow()
        {
            InitializeComponent();

            var col = (xmlOp.GetHistoryDates());
            foreach (var item in col)
            {
                comboBox.Items.Add(item.ToString());
            }
        }

        private async void update_Click(object sender, RoutedEventArgs e)
        {
            task = xmlOp.GetXml();
            var xml = await task;

            var dict = xml.SelectNodes("/Currencies/*")
               .Cast<XmlNode>()
               .ToDictionary(n => n.Name, n => n.InnerText);
            foreach (var kv in dict)
            {
                list.Add(new CurrRatePair { Currency = kv.Key, Rate = kv.Value });
            }

            listView.ItemsSource = list;
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            var a = await Task.Run(() => xmlOp.SaveXmlInHistory());
            comboBox.Items.Add(a);
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => xmlOp.ClearHistory());
            comboBox.Items.Clear();
            listView.ItemsSource = null;
            listView.Items.Clear();
        }
    }
    
}
