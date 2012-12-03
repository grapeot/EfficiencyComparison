using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace SIFTMatch
{
    class Program
    {
        static void Main(string[] args)
        {
            // construct test data
            var sift1 = Matrix.Random(1000, 128).ToArray();
            var sift2 = Matrix.Random(1000, 128).ToArray();

            // invoke different approaches and verify the results are the same
            var tc0 = Environment.TickCount;
            var mLib = SIFTMatchWithLib(sift1, sift2);
            var tcLib = Environment.TickCount;
            var mImpe = SIFTMatchImperative(sift1, sift2);
            var tcImpe = Environment.TickCount;
            var mFunc = SIFTMatchFunctional(sift1, sift2);
            var tcFunc = Environment.TickCount;
            var mImpeLib = SIFTMatchLibImperative(sift1, sift2);
            var tcImpeLib = Environment.TickCount;
            var isCorrect = mLib.IsEqual(mImpe);
            isCorrect = isCorrect && mImpe.IsEqual(mFunc);
            isCorrect = isCorrect && mImpeLib.IsEqual(mFunc);

            // output the verification result as well as the time
            Console.WriteLine("Result is {0}.", isCorrect ? "correct" : "incorrect");
            Console.WriteLine("Lib costs {0}ms.", tcLib - tc0);
            Console.WriteLine("Imperative style costs {0}ms.", tcImpe - tcLib);
            Console.WriteLine("Functional style costs {0}ms.", tcFunc - tcImpe);
            Console.WriteLine("Imperative style with lib costs {0}ms.", tcImpeLib - tcFunc);
        }

        static int[] SIFTMatchWithLib(double[][] sift1, double[][] sift2)
        {
            var s2 = sift2.ToMatrix();
            return sift1.Select(s1 =>
            {
                var tmp = Enumerable.Repeat(s1, sift1.Length).ToArray().ToMatrix().Subtract(s2);
                var dist = tmp.ElementwiseMultiply(tmp).Sum(1);
                int minI;
                dist.Min(out minI);
                return minI;
            }).ToArray();
        }

        static int[] SIFTMatchLibImperative(double[][] sift1, double[][] sift2)
        {
            var s2 = sift2.ToMatrix();
            var matches = new int[sift1.Length];
            for (int i = 0; i < sift1.Length; i++)
            {
                var s1 = sift1[i];
                var tmp = Enumerable.Repeat(s1, sift1.Length).ToArray().ToMatrix().Subtract(s2);
                var dist = tmp.ElementwiseMultiply(tmp).Sum(1);
                int minI;
                dist.Min(out minI);
                matches[i] = minI;
            }
            return matches;
        }

        static int[] SIFTMatchImperative(double[][] sift1, double[][] sift2)
        {
            var matched = new List<int>();
            foreach (var s1 in sift1)
            {
                var d1 = s1;
                var minI = 0;
                var min = float.MaxValue;
                for (int j = 0; j < sift2.Length; j++)
                {
                    var d2 = sift2[j];
                    var dist = 0.0f;
                    for (int i = 0; i < 128; i++)
                    {
                        dist += (float)((d1[i] - d2[i]) * (d1[i] - d2[i]));
                    }
                    if (min > dist)
                    {
                        min = dist;
                        minI = j;
                    }
                }
                matched.Add(minI);
            }
            return matched.ToArray();
        }

        static int[] SIFTMatchFunctional(double[][] sift1, double[][] sift2)
        { 
            return sift1.Select(s1 =>
                Enumerable.Range(0, sift2.Length)
                    .Select(i => 
                    {
                        var tmp = sift2[i].Subtract(s1);
                        return new {I = i, Dist = tmp.ElementwiseMultiply(tmp).Sum()};
                    }).OrderBy(x => x.Dist)
                    .First().I).ToArray();
        }
    }
}