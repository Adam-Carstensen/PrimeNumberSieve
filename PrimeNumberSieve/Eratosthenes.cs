using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrimeNumberSieve
{
    public class Eratosthenes
    {
        public List<KeyValuePair<int, int>>[] CompositeFactors { get; set; }

        /// <summary>
        /// Generates an array of Composite Number Factors, separated by Prime Index, from a number sieve. 
        /// </summary>
        /// <param name="bits">Maximum number represented by 2^n bits.  Values above 26 are very long</param>
        public void CalculatePrimes(int bits)
        {
            var maxRange = Math.Pow(2, bits);

            Console.WriteLine($"Calculating Primes to {maxRange.ToString("N0")}");

            CompositeFactors = new List<KeyValuePair<int, int>>[(int)maxRange];

            double primesFound = 0;

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            for (var index = 3; index < maxRange; index += 2)
            {
                if (index % 2 == 0) continue;

                if (CompositeFactors[index] == null) //prime found
                {
                    primesFound++;
                    if (primesFound % 1000 == 0) Console.WriteLine($"Total Found: {primesFound.ToString("N0")} / {maxRange.ToString("N0")} ");

                    PopulatePrimeProducts(maxRange, CompositeFactors, index);
                }
            }

            sw.Stop();
            Console.WriteLine($"Found Primes: {primesFound.ToString("N0")} / {maxRange.ToString("N0")} ({bits} bits) in {sw.Elapsed.ToString("g")}.");

            TestFactorization(maxRange, CompositeFactors);
            ProjectTimelines(bits, sw.Elapsed.TotalSeconds);
        }

        private void PopulatePrimeProducts(double maxRange, List<KeyValuePair<int, int>>[] compositeFactors, int prime)
        {
            var i = 2;
            int nextProduct = prime * i;
            while (nextProduct < maxRange)
            {
                var primeProducts = compositeFactors[nextProduct];
                if (primeProducts == null)
                {
                    primeProducts = new List<KeyValuePair<int, int>>();
                    compositeFactors[nextProduct] = primeProducts;
                }
                primeProducts.Add(new KeyValuePair<int, int>(prime, i));
                nextProduct = prime * ++i;
            }
        }

        private void TestFactorization(double maxIndex, List<KeyValuePair<int, int>>[] numbers)
        {
            Console.WriteLine("Testing Prime Factorization:");

            var random = new Random();
            for (int test = 1; test <= 30; test++)
            {
                var testNumber = random.Next((int)maxIndex - 1);
                var primeFactors = GetFactors(testNumber);
                int product = 1;
                foreach (var prime in primeFactors) product *= prime;

                string factorString = string.Empty;
                foreach (var item in primeFactors)
                {
                    if (factorString.Length != 0) factorString += ", ";
                    factorString += item;
                }

                Console.WriteLine($"{testNumber.ToString("N0")} | Product: {product.ToString("N0")} | Factors: [{factorString}]");
            }
        }

        public List<int> GetFactors(int number)
        {
            return GetFactors(number, new List<int>());
        }

        private List<int> GetFactors(int number, List<int> factors)
        {
            var compoundFactors = CompositeFactors[number];
            if (compoundFactors == null)
            {
                if (number == 2)
                {
                    factors.Add(2);
                    return factors;
                }

                if (number % 2 == 0)
                {
                    factors.Add(2);
                    return GetFactors(number / 2, factors);
                }

                factors.Add(number);
                return factors;
            }

            var highestFactor = compoundFactors.OrderByDescending(item => item.Key).First();
            factors.Add(highestFactor.Key);
            GetFactors(highestFactor.Value, factors);
            return factors;
        }

        private void ProjectTimelines(int bits, double totalSeconds)
        {
            Console.WriteLine("Processing Estimates:");
            bool pastSunDeath = false;
            bool pastUniverseAge = false;
            var ageOfUniverse = 13700000000;
            for (int i = 1; i <= 256 - bits; i++)
            {
                totalSeconds *= 2;

                if (i > (80 - bits) && i % 8 != 0) continue;

                var yearQuotient = totalSeconds / 60 / 60 / 24 / 365;
                var universalQuotient = yearQuotient / ageOfUniverse;
                var universes = Math.Floor(universalQuotient);

                if (universalQuotient >= 1) yearQuotient = yearQuotient - (universes * ageOfUniverse);
                var years = Math.Floor(yearQuotient);

                var daysProduct = (yearQuotient - years) * 365;
                var days = Math.Floor(daysProduct);
                var hoursProduct = (daysProduct - days) * 24;
                var hours = Math.Floor(hoursProduct);
                var minutesProduct = (hoursProduct - hours) * 60;
                var minutes = Math.Floor(minutesProduct);
                var secondsProduct = (minutesProduct - minutes) * 60;
                var seconds = Math.Floor(secondsProduct);

                if (years > 5000000000 && !pastSunDeath)
                {
                    Console.WriteLine();
                    Console.WriteLine("5 billion years: Our Sun expanded into a Red Giant and baked Earth.");
                    Console.WriteLine();
                    pastSunDeath = true;
                }

                if (universes >= 1 && !pastUniverseAge)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{ageOfUniverse.ToString("N0")} years: Age of our Universe.");
                    Console.WriteLine();
                    pastUniverseAge = true;
                }

                Console.Write($"{i + bits} bits in ");

                var segments = new List<string>();

                if (universes >= 1)
                    segments.Add($"{universes.ToString("N0")} universes (13.7b)");

                if (years >= 1)
                    segments.Add($"{years.ToString("N0")} years");

                if (days >= 1)
                    segments.Add($"{days} days");

                if (hours >= 1)
                    segments.Add($"{hours} hours");

                if (minutes >= 1)
                    segments.Add($"{minutes} minutes");

                if (seconds >= 1)
                    segments.Add($"{seconds} seconds");

                Console.Write(CommaCombine(segments));

                Console.WriteLine();
            }
        }

        private string CommaCombine(List<string> items, bool useAnd = true)
        {
            string combined = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                string item = items[i];
                if (combined.Length != 0)
                {
                    if (items.Count > 2) combined += ",";
                    combined += " ";
                    if (useAnd && i == items.Count - 1) combined += "and ";
                }
                combined += item;
            }

            return combined;
        }
    }
}
