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

namespace WpfGenerationSqlCommand
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VueModele _VM;
        public MainWindow()
        {
            InitializeComponent();
            _VM = new VueModele();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            bool bItemExist = false;
            if (e.Key == Key.Return)
            {
                // TEST DE PRESENCE DE LA TETIERE DANS LA LISTE ACTUELLE
                foreach(string Row in ListeTetieres.Items)
                {
                    if(Row.Trim()==txtTETIERE.Text.Trim())
                    {
                        bItemExist = true;
                    }
                }

                if (!bItemExist)
                {
                    // TEST DE PRESENCE DANS LA BASE DE DONNEES
                    if (_VM.presenceTetiere(txtTETIERE.Text.Trim()))
                        LoadTetiere(txtTETIERE.Text.Trim());
                }
                txtTETIERE.Text = "";
            }
        }

        private void LoadTetiere(string sTetiere)
        {
            ListeTetieres.Items.Add(sTetiere);
        }

        private void btnCLEAR_Click(object sender, RoutedEventArgs e)
        {
            ListeTetieres.Items.Clear();
        }

        private void btnGENERATE_Click(object sender, RoutedEventArgs e)
        {
            List<string> sLignes = new List<string>();
            foreach(string Row in ListeTetieres.Items)
            {
                sLignes.Add(Row);
            }
            SQL_SCRIPT.Text=_VM.generateSQL(sLignes);
        }
    }
}
