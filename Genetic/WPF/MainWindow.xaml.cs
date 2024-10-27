using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Genetic;
using WPF;


namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewData data;
        public double[,] matrix;
        public CancellationTokenSource cts;
        public MainWindow()
        {
            InitializeComponent();
            data = new ViewData();
            matrix = new double[1, 1];
            CommandBinding cmb = new CommandBinding();

        }

        private void RandomizeMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            int numCities;
            if (int.TryParse(NumCitiesTextBox.Text, out numCities) && numCities > 0)
            {
                // Инициализируем новую матрицу расстояний
                matrix = new double[numCities, numCities];

                Random random = new Random();
                for (int i = 0; i < numCities; i++)
                {
                    for (int j = i + 1; j < numCities; j++)
                    {
                        // Генерируем случайное значение для расстояния между городами
                        matrix[i, j] = matrix[j, i] = random.Next(1, 100); // Симметричная матрица
                    }
                }

                // Обновляем DataGrid для отображения новой матрицы
                UpdateDistanceMatrixGrid(matrix);
            }
            else
            {
                MessageBox.Show("Please enter a valid number of cities.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateDistanceMatrixGrid(double[,] matrix)
        {
            int numCities = matrix.GetLength(0);

            DistanceMatrixGrid.Columns.Clear();
            DistanceMatrixGrid.ItemsSource = null;

            for (int i = 0; i < numCities; i++)
            {
                DistanceMatrixGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = $"City {i + 1}",
                    Binding = new System.Windows.Data.Binding($"[{i}]"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                });
            }

            var matrixRows = new List<double[]>();
            for (int i = 0; i < numCities; i++)
            {
                var row = new double[numCities];
                for (int j = 0; j < numCities; j++)
                {
                    row[j] = matrix[i, j];
                }
                matrixRows.Add(row);
            }

            DistanceMatrixGrid.ItemsSource = matrixRows;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            StopButton.IsEnabled = true;
            data.procces(cts.Token, matrix);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет код остановки алгоритма
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет код очистки результатов
        }
    }
}
