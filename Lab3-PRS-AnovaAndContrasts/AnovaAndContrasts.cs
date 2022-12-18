using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_PRS_AnovaAndContrasts
{
    class AnovaAndContrasts
    {
        private double[,] matrix;
        private int numOfAlternatives;
        private int numOfMeasurements;
        private double[] meansOfColumns;
        private double totalMean;
        private double SSA;
        private double SSE;
        private double SST;
        private double sum;
        private int dfSSA;
        private int dfSSE;
        private int dfSST;
        private double varianceSSA;
        private double varianceSSE;
        double[] effects;
        public AnovaAndContrasts(int numOfAlternatives, int numOfMeasurements)
        {
            matrix = new double[numOfMeasurements, numOfAlternatives];
            this.numOfAlternatives = numOfAlternatives;
            this.numOfMeasurements = numOfMeasurements;
            FillInMatrix(numOfAlternatives, numOfMeasurements);
            sum = 0;
            SSA = 0;
            SSE = 0;
            SST = 0;
            dfSSA = this.numOfAlternatives - 1;
            dfSSE = this.numOfAlternatives * (this.numOfMeasurements - 1);
            dfSST = dfSSA + dfSSE;
            SetMeansOfColumns();
            SetTotalMean();
            SetSSA();
            SetSSE();
            SetSST();
            SetVariances();
            SetEffects();
        }
        public void FillInMatrix(int numOfAlternatives, int numOfMeasurements)
        {
            for (int i = 0; i < numOfAlternatives; i++)
            {
                Console.WriteLine((i + 1) + ". " + "alternative");
                Console.WriteLine("=============================");
                for (int j = 0; j < numOfMeasurements; j++)
                {
                    Console.Write("\t" + (j + 1) + ". " + "measurement:");
                    matrix[j, i] = double.Parse(Console.ReadLine());
                }
            }
            Console.WriteLine("=============================");
        }
        public void SetMeansOfColumns()
        {
            meansOfColumns = new double[numOfAlternatives];
            for (int i = 0; i < numOfAlternatives; i++)
            {
                double mean = 0;
                this.sum = 0;
                for (int j = 0; j < numOfMeasurements; j++)
                {
                    sum += matrix[j, i];
                }
                mean = sum / (double)numOfMeasurements;
                meansOfColumns[i] = mean;
            }
        }
        public void SetTotalMean()
        {
            this.sum = 0;
            for (int i = 0; i < numOfAlternatives; i++)
                sum += meansOfColumns[i];
            totalMean = sum / (double)numOfAlternatives;
        }
        public void SetSSA()
        {
            this.sum = 0;
            for (int i = 0; i < numOfAlternatives; i++)
                    sum += Math.Pow(meansOfColumns[i] - totalMean, 2);
            SSA = numOfMeasurements * sum;
        }
        public void SetSSE()
        {
            for (int i = 0; i < numOfAlternatives; i++)
                for (int j = 0; j < numOfMeasurements; j++)
                    SSE += Math.Pow(matrix[j, i] - meansOfColumns[i], 2);
        }
        public void SetSST()
        {
            SST = SSA + SSE;
        }
        public void SetVariances()
        {
            varianceSSA = SSA / (this.numOfAlternatives - 1);
            varianceSSE = SSE / (this.numOfAlternatives * (this.numOfMeasurements - 1));
            
        }
        public void SetEffects()
        {
            effects = new double[numOfAlternatives];
            for(int i=0;i<numOfAlternatives;i++)
            {
                effects[i] = meansOfColumns[i] - totalMean;
            }
        }
        public double[] GetEffects()
        {
            return effects;
        }
        public double GetVarianceSSA()
        {
            return varianceSSA;
        }
        public double GetVarianceSSE()
        {
            return varianceSSE;
        }
        public double GetSSA()
        {
            return SSA;
        }
        public double GetSSE()
        {
            return SSE;
        }
        public double GetSST()
        {
            return SST;
        }
        public double GetDfSSA()
        {
            return dfSSA;
        }
        public double GetDfSSE()
        {
            return dfSSE;
        }
        public double GetDfSST()
        {
            return dfSST;
        }
        public double GetTotalMean()
        {
            return totalMean;
        }
        public double[] GetMeansOfColumns()
        {
            return meansOfColumns;
        }
        public void WriteResults(double tableValue,double confidenceInterval)
        {
            double SSAdivSST = this.SSA / this.SST;
            double SSEdivSST = this.SSE / this.SST;
            double fComputed = this.varianceSSA / this.varianceSSE;
            string path = @"D:\PRS\Lab3-PRS-AnovaAndContrasts\Lab3-PRS-AnovaAndContrasts\AnovaResults.txt";
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("\t" + "\t" + "ANOVA");
                sw.WriteLine("----------------------------------------");
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("\t" + "ALTERNATIVE");
                sw.Write("RB.MJ.");
                for(int i=0;i<numOfAlternatives;i++)
                {
                    sw.Write("\t" + (i + 1)+".");
                }
                sw.Write("\t" + "UKUPNA SR.VR.");
                sw.WriteLine();
                sw.WriteLine("----------------------------------------");
                for (int i = 0; i < numOfMeasurements; i++)
                {
                    sw.Write((i + 1) + ".");
                    for (int j = 0; j < numOfAlternatives; j++)
                    {
                        double x = Math.Round(matrix[i, j], 4);
                        sw.Write("\t"+x);
                    }
                    sw.WriteLine();
                }
                sw.Write("S.V.KO.");
                for(int i=0;i<numOfAlternatives;i++)
                {
                    sw.Write("\t" + Math.Round(meansOfColumns[i],4));
                }
                sw.Write("\t" + Math.Round(totalMean,4));
                sw.WriteLine();
                sw.Write("EFEKTI");
                for(int i=0;i<numOfAlternatives;i++)
                {
                    sw.Write("\t" + Math.Round(effects[i],4));
                }
                sw.WriteLine();
                sw.WriteLine("----------------------------------------");
                sw.WriteLine("SSA:" + "\t" + Math.Round(SSA,4));
                sw.WriteLine("SSE:" + "\t" + Math.Round(SSE,4));
                sw.WriteLine("SST:" + "\t" + Math.Round(SST, 4));
                sw.WriteLine("SSA/SST: "+ Math.Round(SSAdivSST, 4));
                sw.WriteLine("SSE/SST: " +Math.Round(SSEdivSST, 4));
                sw.WriteLine("SSA var: " + Math.Round(varianceSSA, 4));
                sw.WriteLine("SSE var: " + Math.Round(varianceSSE, 4));
                sw.WriteLine("F[izr]:" + "\t" + Math.Round(fComputed, 4));
                sw.WriteLine("F[tab]:" + "\t" + Math.Round(tableValue, 4));
                sw.WriteLine("----------------------------------------");
                sw.WriteLine("Na osnovu datih mjerenja zakljucujemo da je " + Math.Round(((SSAdivSST) * 100),1) + "% " + "ukupne varijacije");
                sw.WriteLine("zbog razlika izmedju alternativa, dok je " + Math.Round(((SSEdivSST) * 100),1) + "% " + "ukupne varijacije");
                sw.WriteLine("u mjerenjima je zbog gresaka u mjerenjima.");
                if(fComputed<tableValue)
                {
                    sw.WriteLine("Kako je izracunata F vrijednost manja od tabelarne F vrijednosti");
                    sw.WriteLine("moze se zakljuciti da za " + ((1 - confidenceInterval) * 100) + "%-tni interval povjerenja");
                    sw.WriteLine("ne postoji statisticki znacajna razlika izmedju sistema");
                }
                else
                {
                    sw.WriteLine("Kako je izracunata F vrijednost veca od tabelarne F vrijednosti");
                    sw.WriteLine("moze se zakljuciti da za " + ((1 - confidenceInterval/2.0) * 100) + "%-tni interval povjerenja");
                    sw.WriteLine("postoji statisticki znacajna razlika izmedju sistema");
                }
                sw.WriteLine("----------------------------------------");
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("\t" + "\t" + "KONTRASTI");
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("----------------------------------------");
                for(int i=0;i<numOfAlternatives;i++)
                {
                    for(int j=i+1;j<numOfAlternatives;j++)
                    {
                        double contrast = effects[i] - effects[j];
                        double var = (Math.Sqrt(varianceSSE)) * (Math.Sqrt(2 / (double)numOfAlternatives * numOfMeasurements));
                        double tValue = MathNet.Numerics.Distributions.StudentT.InvCDF(0.0, 1.0,dfSSE, 1 - confidenceInterval / 2.0);
                        double c1 = contrast + tValue * var;
                        double c2 = contrast - tValue * var;
                        sw.WriteLine("----------------------------------------");
                        sw.WriteLine("POREDJENJE ALTERNATIVA " + (i + 1) + "-" + (j + 1));
                        sw.WriteLine("----------------------------------------");
                        sw.WriteLine("Kontrast:" + "\t" + Math.Round(contrast, 4));
                        sw.WriteLine("Varijansa:" + "\t" + Math.Round(var, 4));
                        sw.WriteLine("t-vrijednost:" + "\t" + Math.Round(tValue, 4));
                        sw.WriteLine("Interval povjerenja:" + "\t" + "[" + Math.Round(c2,4) + "," + Math.Round(c1,4) + "]");
                        if(c1>0 && c2<0)
                        {
                            sw.WriteLine("Kako interval povjerenja ukljucuje nulu zakljucak je da");
                            sw.WriteLine("ne postoji statisticki znacajna razlika izmedju mjerenja");
                        }
                        else
                        {
                            sw.WriteLine("Kako interval povjerenja ne ukljucuje nulu zakljucak je da");
                            sw.WriteLine("postoji statisticki znacajna razlika izmedju mjerenja");
                        }
                    }
                }
                sw.WriteLine("----------------------------------------");
                sw.WriteLine("Analiza je zavrsena!");

            }
        }
    }
}
