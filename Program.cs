using System;

namespace PrimeNumberSieve
{
    class Program
    {
        static void Main(string[] args)
        {
            var sieve = new Eratosthenes();
            sieve.CalculatePrimes(24);
        }
    }
}
