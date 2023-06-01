using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Scala.Adovb3.Core.Entities;
using Scala.Adovb3.Core.Services;

namespace Scala.Adovb3.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ToestelService toestelService = new ToestelService();
        bool isNew;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulCombos();
            LinksActief();
            VulListbox();
            ToonStockWaarde();
        }

        private void LstToestellen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstToetstellen.SelectedItem != null)
            {
                Toestel toestel = (Toestel)lstToetstellen.SelectedItem;
                txtMerk.Text = toestel.Merk;
                txtSerie.Text = toestel.Serie;
                txtPrijs.Text = toestel.Prijs.ToString("#,##0.00");
                txtStock.Text = toestel.Stock.ToString() ;
                cmbSoort.SelectedItem = toestel.Soort;
            }
        }

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            VulListbox();
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            cmbFilter.SelectedIndex = -1;
            VulListbox();
        }
        private void BtnNieuw_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            RechtsActief();
            ClearControls();
            txtMerk.Focus();
        }

        private void BtnWijzig_Click(object sender, RoutedEventArgs e)
        {
            if (lstToetstellen.SelectedItem != null)
            {
                isNew = false;
                RechtsActief();
                txtMerk.Focus();
            }
        }

        private void BtnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (lstToetstellen.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker?", "Toestel verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Toestel toestel = (Toestel)lstToetstellen.SelectedItem;
                    if(!toestelService.ToestelVerwijderen(toestel))
                    {
                        MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ClearControls();
                    VulListbox();
                    ToonStockWaarde();
                }
            }
        }

        private void BtnBewaren_Click(object sender, RoutedEventArgs e)
        {
            string merk = txtMerk.Text.Trim();
            string serie = txtSerie.Text.Trim();
            if(merk.Length == 0)
            {
                MessageBox.Show("Merk invoeren", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMerk.Focus();
                return;
            }
            if (serie.Length == 0)
            {
                MessageBox.Show("Serie invoeren", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSerie.Focus();
                return;
            }
            decimal.TryParse(txtPrijs.Text, out decimal prijs);
            if(prijs < 0)
            {
                MessageBox.Show("Negatieve prijzen verboden", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPrijs.Focus();
                return;
            }
            int.TryParse(txtStock.Text, out int stock);
            if(stock < 0)
            {
                MessageBox.Show("Negatieve stock verboden", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStock.Focus();
                return;
            }
            if (cmbSoort.SelectedIndex == -1)
            {
                MessageBox.Show("Soort opgeven", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbSoort.Focus();
                return;
            }
            string soort = cmbSoort.SelectedItem.ToString();

            Toestel toestel;
            if(isNew)
            {
                toestel = new Toestel(merk, serie, prijs, stock, soort);
                if(!toestelService.ToestelToevoegen(toestel))
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                toestel = (Toestel)lstToetstellen.SelectedItem;
                toestel.Merk = merk;
                toestel.Serie = serie;
                toestel.Prijs = prijs;
                toestel.Stock = stock;
                toestel.Soort = soort;
                if(!toestelService.ToestelWijzigen(toestel))
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            VulListbox();
            ToonStockWaarde();
            LinksActief();
            lstToetstellen.SelectedValue = toestel.Id;
            LstToestellen_SelectionChanged(null, null);
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            LinksActief();
            LstToestellen_SelectionChanged(null, null);
        }
        private void LinksActief()
        {
            grpToestellen.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnBewaren.Visibility = Visibility.Hidden;
            btnAnnuleren.Visibility = Visibility.Hidden;
        }
        private void RechtsActief()
        {
            grpToestellen.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnBewaren.Visibility = Visibility.Visible;
            btnAnnuleren.Visibility = Visibility.Visible;
        }
        private void VulListbox()
        {
            if (cmbFilter.SelectedIndex == -1)
                lstToetstellen.ItemsSource = toestelService.GetToestellen();
            else
                lstToetstellen.ItemsSource = toestelService.GetToestellen(cmbFilter.SelectedItem.ToString());
            lstToetstellen.Items.Refresh();
        }
        private void ClearControls()
        {
            txtMerk.Text = "";
            txtSerie.Text = "";
            txtPrijs.Text = "";
            txtStock.Text = "";
            cmbSoort.SelectedIndex = -1;
        }
        private void VulCombos()
        {
            List<string> soorten = new List<string>();
            soorten.Add("Vaatwas");
            soorten.Add("Wasmachine");
            soorten.Add("Droogkast");
            soorten.Add("Ijskast");
            soorten.Add("Diepvries");
            cmbFilter.ItemsSource = soorten;
            cmbSoort.ItemsSource = soorten;
        }
        private void ToonStockWaarde()
        {
            lblTotaleStockwaarde.Content = toestelService.GetStockwaarde().ToString("#,##0.00");
        }


    }



}
