using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReneUtiles;
using ReneUtiles.Clases;

namespace ReneCreadorPropiedadesWPF
{
    public class CreadorPropiedadesWPF:ConsolaBasica
    {
        private List<string> entradas;
        private List<string> tipos;
        private bool lasEntradasSonProperties;
        //private bool capitalizeProperties;
        //private bool pisoAtributos;
        public CreadorPropiedadesWPF(string texto
            , bool lasEntradasSonProperties = false
            //, bool capitalizeProperties = true
            //,bool pisoAtributos=true
            ) {
            //{ get; set; }
            //
            string[] paresEntradas= split(texto, ',', '\n');
            this.tipos = new List<string>();
            this.entradas = new List<string>();
            for (int i = 0; i < paresEntradas.Length; i++)
            {

                List<string> separaciones = split(paresEntradas[i], ' ').ToList();
                separaciones.RemoveAll(v =>or(v, "{", "get;", "set;","}"));
                if (separaciones.Count()>0) {
                    //entradas[i] = separaciones[separaciones.Count() - 1];
                    entradas.Add(separaciones.Last().Replace(";", ""));
                    if (separaciones.Count() > 1)
                    {
                        tipos.Add(separaciones[separaciones.Count() - 2]);
                        
                    }
                    else
                    {
                        tipos.Add("string");
                    }
                }
                
            }
            this.lasEntradasSonProperties = lasEntradasSonProperties;
            //this.capitalizeProperties = capitalizeProperties;
            //this.pisoAtributos = pisoAtributos;
        }
        public string generarCodigo_Properties_Atributos() {
            Func<string, string> getNombreTratado = v => Utiles.eliminarAlInicio(v.Trim(), '_');
            Func<string, string> getNombrePropertie=null;

            if (this.lasEntradasSonProperties)
            {
                //getNombrePropertie = v => this.capitalizeProperties ? Utiles.capitalize(getNombreTratado(v)) : getNombreTratado(v);
                getNombrePropertie = v =>  getNombreTratado(v);

            }
            else
            {
                getNombrePropertie = v => Utiles.capitalize(getNombreTratado(v));
            }

            Func<string, string> getNombreAtributo = null;


            getNombreAtributo = v => "_"+Utiles.primerCharMinuscula(getNombreTratado(v));

            int iden = 0;
            string r = "";

            for (int i = 0; i < this.entradas.Count(); i++)
            {
                r += Utiles.identacionLn(iden,1)+"private "+this.tipos[i]+" "+getNombreAtributo(this.entradas[i])+";";
            }

            for (int i = 0; i < this.entradas.Count(); i++)
            {
                r += Utiles.identacionLn(iden, 1) + "public " + this.tipos[i] + " " + getNombrePropertie(this.entradas[i]) ;
                r += Utiles.identacionLn(iden, 1) + "{";
                r += Utiles.identacionLn(iden, 2) + "get";
                r += Utiles.identacionLn(iden, 2) + "{";
                r += Utiles.identacionLn(iden, 3) + "return "+ getNombreAtributo(this.entradas[i]) + ";";
                r += Utiles.identacionLn(iden, 2) + "}";
//                r += Utiles.identacionLn(iden, 1) + "{";
                r += Utiles.identacionLn(iden, 2) + "set";
                r += Utiles.identacionLn(iden, 2) + "{";
                r += Utiles.identacionLn(iden, 3) +  getNombreAtributo(this.entradas[i]) + " = value;";
                r += Utiles.identacionLn(iden, 3) + "OnPropertyChanged(nameof("+ getNombrePropertie(this.entradas[i]) + "));";
                r += Utiles.identacionLn(iden, 2) + "}";
                r += Utiles.identacionLn(iden, 1) + "}";
            }

            return r;
        }

        public string generalCodigo_GetMetodos() {
            Func<string, string> getNombreMetodo = v => "Get" + Utiles.primerCharMayuscula(v);
            int iden = 0;
            string r = "";
            for (int i = 0; i < this.entradas.Count(); i++)
            {
                string atributo = this.entradas[i];
                string tipo = this.tipos[i];
                r += Utiles.identacionLn(iden, 1) + "public " + tipo + " " + getNombreMetodo(atributo)+"()";
                r += Utiles.identacionLn(iden, 1) + "{";
                r += Utiles.identacionLn(iden, 2) + "return this." + atributo + ";";
                r += Utiles.identacionLn(iden, 1) + "}";
            }
            return r;
        }


        public string generalCodigo_GetProperties()
        {
            Func<string, string> getNombrePropertie = v =>  Utiles.primerCharMayuscula(v);
            int iden = 0;
            string r = "";
            for (int i = 0; i < this.entradas.Count(); i++)
            {
                string atributo = this.entradas[i];
                string tipo = this.tipos[i];
                //r += Utiles.identacionLn(iden, 1) + "public " + tipo + " " + getNombreMetodo(atributo) + "()";
                //r += Utiles.identacionLn(iden, 1) + "{";
                //r += Utiles.identacionLn(iden, 2) + "return this." + atributo + ";";
                //r += Utiles.identacionLn(iden, 1) + "}";


                r += Utiles.identacionLn(iden, 1) + "public " + tipo + " " + getNombrePropertie(atributo);
                r += Utiles.identacionLn(iden, 1) + "{";
                r += Utiles.identacionLn(iden, 2) + "get";
                r += Utiles.identacionLn(iden, 2) + "{";
                r += Utiles.identacionLn(iden, 3) + "return this." + atributo + ";";
                r += Utiles.identacionLn(iden, 2) + "}";
               
                
                r += Utiles.identacionLn(iden, 1) + "}";
            }
            return r;
        }

        public string generalCodigo_ToStringPython()
        {
            int iden = 0;
            string separacion1 = Utiles.identacionLn(iden, 1);
            string separacion2 = Utiles.identacionLn(iden, 2);
            string separacion3 = Utiles.identacionLn(iden, 3);

            string mr = "";
            mr += separacion1 + "def __str__(self):";
            mr += separacion2 + "return self.__class__.__name__";
            mr += this.entradas.Count()>0 ? "\\" : "";
            //string separacion3 = getSeparacionln(3,separacion0);
            for (int i = 0; i < this.entradas.Count(); i++)
            {
                string atributo = this.entradas[i];
                if (atributo.Contains(":")) {
                    atributo = subs(atributo, 0, atributo.IndexOf(':'));
                }
                
                mr += separacion3 + "+\" " + atributo + "=\"+str(self." + atributo+")";
                mr += i != this.entradas.Count() - 1?"\\":"";
            }
            

            return mr;
        }




        }
}
