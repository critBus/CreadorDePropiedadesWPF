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
using ReneUtiles;
using ReneUtiles.Clases;
using ReneUtiles.Clases.WPF;
using ReneCreadorPropiedadesWPF;

using ReneUtiles.Clases.BD;

namespace CreadorDePropiedadesWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        private TemporalStorageBD TS;
        public MainWindow()
        {
            InitializeComponent();
            TS = new TemporalStorageBD();
            cargarEA();


        }

        private void cargarEA() {
            setTextoSalida(TS.getStr("salida", ""));
            setTextoEntrada(TS.getStr("entrada", ""));
            seleccionarCopiar(TS.getBool("copiar",true)??true);
            seleccionarPegar(TS.getBool("pegar", true) ?? true);
            seleccionar_LasEntradasSonProperties(TS.getBool("entradasProperties", false) ?? false);
        }
        private void guardarEA()
        {
            TS.put("salida", getTextoSalida());
            TS.put("entrada", getTextoEntrada());
            TS.put("copiar", estaSeleccionadoCopiar());
            TS.put("pegar", estaSeleccionadoPegar());
            TS.put("entradasProperties", estaSeleccionado_LasEntradasSonProperties());
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }


        private void btnMinimizeLogin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnCloseLogin_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }




        public void setTextoEntrada(string a)
        {
            this.setText(TB_Entrada, a);
        }
        public string getTextoEntrada()
        {
            return UtilesWPF.getText(TB_Entrada);// T_entrada.Text.Replace("\r","");
        }
        public void setTextoSalida(string a)
        {
            //	T_Salida.Text=a;
            this.setText(TB_Salida, a);
        }
        public string getTextoSalida()
        {
            return this.getText(TB_Salida);// T_entrada.Text.Replace("\r","");
        }

        public bool estaSeleccionadoCopiar()
        {
            //return CB_PegarYAplicar.IsChecked==true;
            return this.isSelect(CB_Copiar);
        }
        public bool estaSeleccionadoPegar()
        {
            //return CB_PegarYAplicar.IsChecked==true;
            return this.isSelect(CB_Pegar);
        }
        public void seleccionarCopiar(bool seleccionar) {
            this.setSelect(CB_Copiar, seleccionar); 
        }
        public void seleccionarPegar(bool seleccionar)
        {
            this.setSelect(CB_Pegar, seleccionar);
        }

        public bool estaSeleccionado_LasEntradasSonProperties()
        {
            //return CB_PegarYAplicar.IsChecked==true;
            return this.isSelect(CB_LasEntradasSonProperties);
        }
        public void seleccionar_LasEntradasSonProperties(bool seleccionar)
        {
            this.setSelect(CB_LasEntradasSonProperties, seleccionar);
        }

        private void paste() {
            setTextoEntrada(this.getTextoDeW());
        }
        private void copy()
        {
            this.copiarW(getTextoSalida());
        }

        public void crearCodigo(Func<string, string> getTextoEditado) {
            if (estaSeleccionadoPegar())
            {
                paste();
            }
            string entrada = getTextoEntrada();
            entrada = getTextoEditado(entrada);
            setTextoSalida(entrada);
            if (estaSeleccionadoCopiar())
            {
                copy();
            }
        }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void button_Click_1(object sender, RoutedEventArgs e)
        //{

        //}

        private void alApretarBotonGeneral(object sender, RoutedEventArgs e)
        {
            crearCodigo(v=>new CreadorPropiedadesWPF(v, estaSeleccionado_LasEntradasSonProperties()).generarCodigo_Properties_Atributos());
        }

        private void AlCerrarLaVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            guardarEA();
            
        }

        private void alApretarBotonClear(object sender, RoutedEventArgs e)
        {
            setTextoEntrada("");
            setTextoSalida("");
        }

        private void alApretarBotonPegar(object sender, RoutedEventArgs e)
        {
            paste();
        }

        private void alApretarBotonCopiar(object sender, RoutedEventArgs e)
        {
            copy();
        }

        private void apreto_Boton_Gets(object sender, RoutedEventArgs e)
        {
            crearCodigo(v => new CreadorPropiedadesWPF(v).generalCodigo_GetMetodos());
        }

        private void alApretar_ToString_Python(object sender, RoutedEventArgs e)
        {
            crearCodigo(v => new CreadorPropiedadesWPF(v).generalCodigo_ToStringPython());
        }

        private void apreto_Boton_Gets_Propiedades(object sender, RoutedEventArgs e)
        {
            crearCodigo(v => new CreadorPropiedadesWPF(v).generalCodigo_GetProperties());
        }
    }
}
