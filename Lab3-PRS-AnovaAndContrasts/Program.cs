using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_PRS_AnovaAndContrasts
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfAlternatives = 0;
            int numOfMeasurements = 0;
            double confidenceInterval = 0;
            try
            {
                while ((numOfAlternatives <= 1 || numOfMeasurements <= 1) || (confidenceInterval <= 0 || confidenceInterval>=1))
                {
                    Console.Write("Insert the number of measurements: ");
                    numOfMeasurements = int.Parse(Console.ReadLine());
                    Console.Write("Insert the number of alternatives: ");
                    numOfAlternatives = int.Parse(Console.ReadLine());
                    Console.Write("Insert the confidence interval: ");
                    confidenceInterval = double.Parse(Console.ReadLine());
                    Console.Clear();
                }
                AnovaAndContrasts anova = new AnovaAndContrasts(numOfAlternatives, numOfMeasurements);
                double fTabulated = MathNet.Numerics.Distributions.FisherSnedecor.InvCDF
                    (numOfAlternatives-1, (numOfAlternatives * (numOfMeasurements - 1)), 1 - confidenceInterval/(2.0));
                anova.WriteResults(fTabulated,confidenceInterval);
                
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }
    }
}
