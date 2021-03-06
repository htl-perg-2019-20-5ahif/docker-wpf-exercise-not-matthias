﻿using CarBooking.API.Controllers.Requests;
using CarBooking.API.Model;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace CarBooking
{
    /// <summary>
    /// Interaction logic for BookCar.xaml
    /// </summary>
    public partial class BookCar : Window
    {
        public DateTime BookingDate { get; set; } = DateTime.Now.Date;

        private readonly Car _car;

        public BookCar(Car car)
        {
            InitializeComponent();

            DataContext = this;

            _car = car;
        }

        /// <summary>
        /// Simply exits the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Books the specified car for the selected date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            BookingRequest request = new BookingRequest
            {
                BookingDate = BookingDate,
                CarId = _car.CarId
            };

            var client = HttpClientSingleton.GetClient();
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5000/api/bookings", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to book car.");
            }

            Close();
        }
    }
}
