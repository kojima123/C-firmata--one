﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sample_0007
{
    class FFT
    {
        private double[] dftIn;
        private int size;
        public double[] reFFT;
        public  double[] imFFT;
   
    

        public FFT(double[] dftIn, int size, out double[] reFFT_2, out double[] imFFT_2)
        {
            // TODO: Complete member initialization
            this.dftIn = dftIn;
            this.size = size;
         
                {
        
            double[] windata = new double[size];
            double[] dftInIm = new double[size];
            // TODO: Complete member initialization
            this.dftIn = dftIn;
            this.size = size;

            Windowing(dftIn, out windata);
            FFTmat(windata, dftInIm, out reFFT, out imFFT, 10);

           reFFT_2= reFFT;
            imFFT_2= imFFT;

        }
        }
        // 窓関数
        public void Windowing(double[] data, out double[] windata)
        {
            int size = data.Length;
            windata = new double[size];

            for (int i = 0; i < size; i++)
            {
                double winValue = 0;
                // 各々の窓関数
                winValue = 0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (size - 1));
             


                // 窓関数を掛け算
                windata[i] = data[i] * winValue;
            }

        }

        // FFT
        public static void FFTmat(double[] inputRe, double[] inputIm, out double[] outputRe, out double[] outputIm, int bitSize)
        {
            int dataSize = 1 << bitSize;
            int[] reverseBitArray = BitScrollArray(dataSize);

            outputRe = new double[dataSize];
            outputIm = new double[dataSize];

            // バタフライ演算のための置き換え
            for (int i = 0; i < dataSize; i++)
            {
                outputRe[i] = inputRe[reverseBitArray[i]];
                outputIm[i] = inputIm[reverseBitArray[i]];
            }

            // バタフライ演算
            for (int stage = 1; stage <= bitSize; stage++)
            {
                int butterflyDistance = 1 << stage;
                int numType = butterflyDistance >> 1;
                int butterflySize = butterflyDistance >> 1;

                double wRe = 1.0;
                double wIm = 0.0;
                double uRe = System.Math.Cos(System.Math.PI / butterflySize);
                double uIm = -System.Math.Sin(System.Math.PI / butterflySize);

                for (int type = 0; type < numType; type++)
                {
                    for (int j = type; j < dataSize; j += butterflyDistance)
                    {
                        int jp = j + butterflySize;
                        double tempRe = outputRe[jp] * wRe - outputIm[jp] * wIm;
                        double tempIm = outputRe[jp] * wIm + outputIm[jp] * wRe;
                        outputRe[jp] = outputRe[j] - tempRe;
                        outputIm[jp] = outputIm[j] - tempIm;
                        outputRe[j] += tempRe;
                        outputIm[j] += tempIm;
                    }
                    double tempWRe = wRe * uRe - wIm * uIm;
                    double tempWIm = wRe * uIm + wIm * uRe;
                    wRe = tempWRe;
                    wIm = tempWIm;
                }
            }
        }

        // ビットを左右反転した配列を返す
        private static int[] BitScrollArray(int arraySize)
        {
            int[] reBitArray = new int[arraySize];
            int arraySizeHarf = arraySize >> 1;

            reBitArray[0] = 0;
            for (int i = 1; i < arraySize; i <<= 1)
            {
                for (int j = 0; j < i; j++)
                    reBitArray[j + i] = reBitArray[j] + arraySizeHarf;
                arraySizeHarf >>= 1;
            }
            return reBitArray;
        }
 
    }
}
