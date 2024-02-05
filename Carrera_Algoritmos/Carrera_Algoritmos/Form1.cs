﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carrera_Algoritmos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            // Configurar el NotifyIcon
            niErrorLetra = new NotifyIcon();
            niErrorLetra.Icon = SystemIcons.Error;
            niErrorLetra.Visible = true;

            // Manejar el evento KeyPress en el formulario
            this.KeyPress += txtNumBuscar_KeyPress;
        }

        private void gbInsercion_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            lblRamBinaria.Text = "";
            lblRamBurbuja.Text = "";
            lblRamInsercion.Text = "";
            lblRamQuicksort.Text = "";
            lblRamSecuencial.Text = "";
            lblRamApp.Text = "";

            lblTiempoBinaria.Text = "";
            lblTiempoBurbuja.Text = "";
            lblTiempoInsercion.Text = "";
            lblTiempoQuicksort.Text = "";
            lblTiempoSecuencial.Text = "";

            txtNumBuscar.Text = "";
            rtxtBinaria.Text = "";
            rtxtBurbuja.Text = "";
            rtxtInsercion.Text = "";
            rtxtQuicksort.Text = "";
            rtxtSecuencial.Text = "";

            pbBinaria.Value = pbBinaria.Minimum;
            pbBurbuja.Value = pbBurbuja.Minimum;
            pbInsercion.Value = pbInsercion.Minimum;
            pbQuicksort.Value = pbQuicksort.Minimum;
            pbSecuencial.Value = pbSecuencial.Minimum;

            txtNumBuscar.Focus();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            //variables para consumo
            long memoriaInicial = Process.GetCurrentProcess().PrivateMemorySize64; //variable de consumo inicial
            long usoDeMemoriaSecuencial = 0;
            long usoDeMemoriaBinaria = 0;
            long usoDeMemoriaBurbuja = 0;
            long usoDeMemoriaQuicksort = 0;
            long usoDeMemoriaInsercion = 0;



            //variables de tiempo
            Stopwatch tiempoSecuencial = new Stopwatch();
            Stopwatch tiempoBinaria = new Stopwatch();
            Stopwatch tiempoBurbuja = new Stopwatch();
            Stopwatch tiempoQuiksort = new Stopwatch();
            Stopwatch tiempoInsercion = new Stopwatch();


            int numNumerosAleatorios = 10000; // cantidad de números aleatorios
            int[] numerosAleatorios = new int[numNumerosAleatorios];
            int[] numerosAleatoriosOrden = new int[numNumerosAleatorios];
            int[] resBurbuja = new int[numNumerosAleatorios];
            int[] resQuicksort = new int[numNumerosAleatorios];
            int[] resInsercion = new int[numNumerosAleatorios];




            //int[] numerosorden = { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30};
            //int[] numerosnoorden = {1,5,7,3,4,6,2,8,9,10,12,13,15,20,21,17,18,11,22,27,29,30,28,19,24,14,26,16,23,25};


            // generar números aleatorios en paralelo
            Parallel.For(0, numNumerosAleatorios, i =>
            {
                Random random = new Random();
                numerosAleatorios[i] = random.Next(1, 3000001); // Números aleatorios entre 1 y 3000

                //QuickSort(numerosAleatoriosOrden, 0, numerosAleatoriosOrden.Length - 1);
            });


            //Declaracion de variables resultados
            Parallel.Invoke(

                () => Array.Copy(numerosAleatorios, numerosAleatoriosOrden, numerosAleatorios.Length),
                () => Array.Copy(numerosAleatorios, resQuicksort, numerosAleatorios.Length),
                () => Array.Copy(numerosAleatorios, resBurbuja, numerosAleatorios.Length),
                () => Array.Copy(numerosAleatorios, resInsercion, numerosAleatorios.Length)
            );


            Parallel.Invoke(
                () => QuickSort(numerosAleatoriosOrden, 0, numerosAleatoriosOrden.Length - 1)
            );




            if (txtNumBuscar.Text != "")
            {
                //declaracion de variable busqueda
                int numBuscar = int.Parse(txtNumBuscar.Text);

                int resSecuencial = 0; //variable busqueda Secuencial
                int resBinaria = 0; //variable busqueda Binaria

                //Ejecucion de algoritmos de busqueda y ordenamiento en parallelo

                Parallel.Invoke(
                    
                    () => 
                    {
                        long memoriaAntesSecuencial = Process.GetCurrentProcess().PrivateMemorySize64;
                        tiempoSecuencial.Start();
                        resSecuencial = BusquedaSecuencial(numerosAleatorios, numBuscar);
                        tiempoSecuencial.Stop();
                        long memoriaDespuesSecuencial = Process.GetCurrentProcess().PrivateMemorySize64;
                        usoDeMemoriaSecuencial = ((long)(memoriaDespuesSecuencial - memoriaAntesSecuencial));


                    },

                    () =>
                    {
                        long memoriaAntesBinaria = Process.GetCurrentProcess().PrivateMemorySize64;
                        tiempoBinaria.Start();
                        resBinaria = BusquedaBinaria(numerosAleatoriosOrden, numBuscar);
                        tiempoSecuencial.Stop();
                        long memoriaDespuesBinaria = Process.GetCurrentProcess().PrivateMemorySize64;
                        usoDeMemoriaBinaria = ((long)(memoriaDespuesBinaria - memoriaAntesBinaria));
                    },

                    () =>
                    {

                        long memoriaAntesBurbuja = Process.GetCurrentProcess().PrivateMemorySize64;
                        tiempoBurbuja.Start();
                        //Realizar busqueda
                        OrdenamientoBurbuja(resBurbuja);
                        tiempoBurbuja.Stop();

                        long memoriaDespuesBurbuja = Process.GetCurrentProcess().PrivateMemorySize64;
                        // Calcula el uso de memoria de QuickSort
                        usoDeMemoriaBurbuja = ((long)(memoriaDespuesBurbuja - memoriaAntesBurbuja));



                    },

                    () => {

                        long memoriaAntesQuiksort = Process.GetCurrentProcess().PrivateMemorySize64;

                        tiempoQuiksort.Start();
                        //Realizar busqueda
                        QuickSort(resQuicksort, 0, resQuicksort.Length - 1);
                        tiempoQuiksort.Stop();

                        long memoriaDespuesQuicksort = Process.GetCurrentProcess().PrivateMemorySize64;
                        // Calcula el uso de memoria de QuickSort
                        usoDeMemoriaQuicksort = ((long)(memoriaDespuesQuicksort - memoriaAntesQuiksort));

                        
                    },

                    () => {

                        long memoriaAntesInsercion = Process.GetCurrentProcess().PrivateMemorySize64;

                        tiempoInsercion.Start();
                        //Realizar busqueda
                        OrdenamientoPorInsercion(resInsercion);
                        tiempoInsercion.Stop();

                        long memoriaDespuesInsercion = Process.GetCurrentProcess().PrivateMemorySize64;
                        // Calcula el uso de memoria de QuickSort
                        usoDeMemoriaInsercion = ((long)(memoriaDespuesInsercion - memoriaAntesInsercion));

                    }
                    


                );


                //resultados


                //resultado Busqueda Secuencial
                if (resSecuencial != -1)
                {
                    rtxtSecuencial.Text = "El numero fue encontrado en la posición no. " + resSecuencial;
                }
                else
                {
                    rtxtSecuencial.Text = "El numero no existe en el arreglo";
                }
                lblTiempoSecuencial.Text = tiempoSecuencial.Elapsed.ToString();
                lblRamSecuencial.Text = usoDeMemoriaSecuencial.ToString() + " bytes";


                //resultado Busqueda Binaria
                if (resBinaria != -1)
                {
                    rtxtBinaria.Text = "El numero fue encontrado en la posición no. " + resBinaria;
                    //rtxtBinaria.Text = "El numero no existe en el arreglo";
                }
                else
                {
                    rtxtBinaria.Text = "El numero no existe en el arreglo";
                    //rtxtBinaria.Text = "El numero fue encontrado en la posición no. " + resBinaria;
                }
                lblTiempoBinaria.Text = tiempoBinaria.Elapsed.ToString();
                lblRamBinaria.Text = usoDeMemoriaBinaria.ToString() + " bytes";


                //resultado Quicksort
                string resBurbujaString = string.Join(" ", resBurbuja);
                rtxtBurbuja.Text = resBurbujaString;
                lblTiempoBurbuja.Text = tiempoBurbuja.Elapsed.ToString();
                lblRamBurbuja.Text = usoDeMemoriaBurbuja.ToString() + " bytes";


                //resultado Quicksort
                string resQuicksortString = string.Join(" ", resQuicksort);
                rtxtQuicksort.Text = resQuicksortString;
                lblTiempoQuicksort.Text = tiempoQuiksort.Elapsed.ToString();
                lblRamQuicksort.Text = usoDeMemoriaQuicksort.ToString() + " bytes";



                //resultado Quicksort
                string resInsercionString = string.Join(" ", resInsercion);
                rtxtInsercion.Text = resInsercionString;
                lblTiempoInsercion.Text = tiempoInsercion.Elapsed.ToString();
                lblRamInsercion.Text = usoDeMemoriaInsercion.ToString() + " bytes";







            }
            else
            {
                // Mostrar un mensaje con el NotifyIcon
                niErrorLetra.BalloonTipTitle = "Error";
                niErrorLetra.BalloonTipText = "Debe agregar un numero de busqueda primero";
                niErrorLetra.ShowBalloonTip(2000); // Muestra el mensaje durante 2 segundos
                txtNumBuscar.Focus();
            }


            long memoriaFinal = Process.GetCurrentProcess().PrivateMemorySize64; //memoria Final consumida
            long usoMemoria = 0;
            usoMemoria = ((long)((memoriaFinal - memoriaInicial) / (1024.0 * 1024.0)));

            lblRamApp.Text = usoMemoria.ToString() + " MB";

        }

        private void txtNumBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;

                // Mostrar un mensaje con el NotifyIcon
                niErrorLetra.BalloonTipTitle = "Error";
                niErrorLetra.BalloonTipText = "Solo puede agregar numeros";
                niErrorLetra.ShowBalloonTip(2000); // Muestra el mensaje durante 2 segundos
            }

        }


        private void MuestraResultados()
        {

        }


        private static int BusquedaSecuencial(int[] lista, int elemento)
        {
            for (int i = 0; i < lista.Length; i++)
            {
                if (lista[i] == elemento)
                    return i;
            }
            return -1;
        }



        //búsqueda binaria
        private static int BusquedaBinaria(int[] lista, int elemento)
        {
            int inicio = 0;
            int fin = lista.Length - 1;

            while (inicio <= fin)
            {
                int medio = (inicio + fin) / 2;

                if (lista[medio] == elemento)
                    return medio;

                if (lista[medio] < elemento)
                    inicio = medio + 1;
                else
                    fin = medio - 1;
            }

            return -1;
        }



        // Ordenamiento Burbuja
        private static void OrdenamientoBurbuja(int[] lista)
        {
            for (int i = 0; i < lista.Length; i++)
            {
                for (int j = 0; j < lista.Length - 1 - i; j++)
                {
                    if (lista[j] > lista[j + 1])
                    {
                        int temp = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = temp;
                    }
                }
            }
        }



        // Método Quiksort
        private static void QuickSort(int[] lista, int inicio, int fin)
        {
            if (inicio < fin)
            {
                int pivote = PartirQuiksort(lista, inicio, fin);
                QuickSort(lista, inicio, pivote - 1);
                QuickSort(lista, pivote + 1, fin);
            }
        }

        // Método auxiliar para realizar la partición del arreglo
        private static int PartirQuiksort(int[] lista, int inicio, int fin)
        {
            int pivote = lista[fin];
            int i = inicio - 1;

            for (int j = inicio; j < fin; j++)
            {
                if (lista[j] <= pivote)
                {
                    i++;
                    Intercambiar(ref lista[i], ref lista[j]);
                }
            }

            Intercambiar(ref lista[i + 1], ref lista[fin]);
            return i + 1;
        }

        private static void Intercambiar(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }


        //Metodo inserción
        private static void OrdenamientoPorInsercion(int[] lista)
        {
            for (int i = 1; i < lista.Length; i++)
            {
                int clave = lista[i];
                int j = i - 1;

                while (j >= 0 && lista[j] > clave)
                {
                    lista[j + 1] = lista[j];
                    j = j - 1;
                }

                lista[j + 1] = clave;
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void gbQuicksort_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void pbQuicksort_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void lblNumBuscar_Click(object sender, EventArgs e)
        {

        }
    }
}
