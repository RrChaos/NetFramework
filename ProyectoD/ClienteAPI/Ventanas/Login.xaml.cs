﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ClienteAPI;

namespace ClienteAPI.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }

        public partial class Entrar
        {
            public string Mail;
            public string Contrasena;
            public Entrar(string mail, string contrasena)
            {
                Mail = mail;
                Contrasena = contrasena;
            }
        }

        private void Button_entrar_Click(object sender, RoutedEventArgs e)
        {

            //Instancia de la clase Entrar para poder crear el string "salida" (pide URL del controlador y un objeto serializado en JSON)
            var nuevo = new Entrar(this.TexBox_mail.Text, this.TexBox_contrasena.Text);

            //Serializacion JSON de la instancia nuevo
            string loginJson = JsonConvert.SerializeObject(nuevo, Newtonsoft.Json.Formatting.Indented);

            //request y response (guardado en Servicios.cs) al API (DEVUELVE UN STRING)
            string salida1 = Servicios.Servicios.PostRequest("http://localhost:50414/api/clientes/login/entrar", loginJson);

            //Se quitan las "" que salen de más por defecto
            string salida2 = salida1.Remove(0, 1);
            int len = salida2.Length;

            //esta es la salida buena
            string salida = salida2.Remove(len - 1);


            if (salida.IndexOf(" ") == -1 && salida.StartsWith("eyJh"))
            {
                Servicios.TokenClass.Token = salida;


                ////request y response para que devuelva el ID
                string idlogin1 = Servicios.Servicios.PostRequestJWT("api/clientes/login/id", loginJson).ToString();

                //Se quitan las "" que salen de más por defecto
                string idlogin2 = idlogin1.Remove(0, 1);
                int idlen = idlogin2.Length;

                //en idlogin queda almacenado el id del usuario estrado
                string idlogin = idlogin2.Remove(idlen - 1);

                VentanaMapa mapa = new VentanaMapa(idlogin);
                this.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Hidden;
                mapa.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show(salida);
            }
        }

        private void Button_registrar_Click(object sender, RoutedEventArgs e)
        {
            RegistroCliente registro = new RegistroCliente();
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            registro.Visibility = Visibility.Visible;
        }

        private void Button_volver_Click(object sender, RoutedEventArgs e)
        {
            Bienvenida bienvenida = new Bienvenida();
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            bienvenida.Visibility = Visibility.Visible;
        }
    }
}
