using CarBooking.API.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CarBooking
{
    enum FetchType
    {
        All,
        Available
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();
        public Car SelectedCar { get; set; }
        public int SelectedFetchType { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Fetches the cars from the api and returns them.
        /// </summary>
        /// <param name="fetchType"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Car>> FetchList(FetchType fetchType)
        {
            var requestUri = "https://localhost:5001/api/cars/" + fetchType switch
            {
                FetchType.All => "all",
                FetchType.Available => "available",
                _ => "all"
            };

            // Send request
            var client = HttpClientSingleton.GetClient();
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            // Parse content
            var responseBody = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<Car[]>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return cars;
        }

        /// <summary>
        /// Clears and sets the specified elements.
        /// </summary>
        /// <param name="cars">The elements of the list</param>
        private void ReloadList(IEnumerable<Car> cars)
        {
            // Clear list
            Cars.Clear();

            // Add to list
            cars.ToList().ForEach(car => Cars.Add(car));
        }

        /// <summary>
        /// Update car list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fetch the list
            var cars = await FetchList(SelectedFetchType switch
            {
                0 => FetchType.All,
                1 => FetchType.Available,
                _ => FetchType.All,
            });

            // Reload list
            ReloadList(cars);
        }

        /// <summary>
        /// Book the specified car (opening the BookCar window for that).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar is null)
            {
                MessageBox.Show("Please select a car.");
                return;
            }

            // Show the dialog
            var window = new BookCar(SelectedCar);
            window.ShowDialog();

            // Update the list
            ComboBox_SelectionChanged(null, null);
        }
    }
}
