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
            var mImpe = SIFTMatchImperativeUnSafe(sift1, sift2);
            var tcImpe = Environment.TickCount;
            var mFunc = SIFTMatchFunctional(sift1, sift2);
            var tcFunc = Environment.TickCount;
            var mImpeLib = SIFTMatchLibImperative(sift1, sift2);
            var tcImpeLib = Environment.TickCount;
            var isCorrect = mFunc.IsEqual(mImpe);
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

        static int[] SIFTMatchImperativeUnSafe(double[][] _sift1, double[][] _sift2)
        {
            var h1 = _sift1.Length;
            var h2 = _sift2.Length;
            var sift1 = new float[h1 * 128];
            var sift2 = new float[h2 * 128];
            var matched = new int[h1];
            unsafe
            {
                fixed (float* s1 = &sift1[0])
                {
                    fixed (float* s2 = &sift2[0])
                    {
                        for (int i = 0; i < h1; i++)
                        {
                            for (int j = 0; j < 128; j++)
                            {
                                s1[i * 128 + j] = (float)_sift1[i][j];
                            }
                        }
                        for (int i = 0; i < h2; i++)
                        {
                            for (int j = 0; j < 128; j++)
                            {
                                s2[i * 128 + j] = (float)_sift2[i][j];
                            }
                        }

                        var minI = 0;
                        var ss1 = s1;
                        for (int i = 0; i < h1; i++, ss1 += 128)
                        {
                            var min = float.MaxValue;
                            var index2 = 0;
                            for (int j = 0; j < h2; j++)
                            {
                                var dist = 0.0f;
                                for (int d = 0; d < 128; d++, index2++)
                                {
                                    var tmp = ss1[d] - s2[index2];
                                    dist += tmp * tmp;
                                }
                                if (min > dist)
                                {
                                    min = dist;
                                    minI = j;
                                }
                            }
                            matched[i] = minI;
                        }
                    }
                }
            }
            return matched;
        }

        static int[] SIFTMatchFunctional(double[][] sift1, double[][] sift2)
        {
            return sift1.Select(s1 =>
                Enumerable.Range(0, sift2.Length)
                    .Select(i =>
                    {
                        var tmp = sift2[i].Subtract(s1);
                        return new { I = i, Dist = tmp.ElementwiseMultiply(tmp).Sum() };
                    }).OrderBy(x => x.Dist)
                    .First().I).ToArray();
        }
    }
}