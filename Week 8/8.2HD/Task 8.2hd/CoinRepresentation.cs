using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace CoinRepresentation
{
    public class CoinRepresentation
    {       
        /// <summary>
        /// Dictionary to memoize already calculated Stern's Diatomic Series values.
        /// Memoization is used to avoid redundant calculations by storing previously computed values.
        /// It maps the input sum Z to its corresponding Stern's Diatomic Series value.
        /// </summary>

        static Dictionary<long, long> dict = new Dictionary<long, long>();
                
        /// <summary>
        /// Recursive function to compute Stern's Diatomic Series value for a given input.
        /// SternDiatomicSeriesFunc is a recursive function that computes Stern's 
        /// diatomic series value for a given input Z. It recursively calculates the series value for Z based on its 
        /// parity and stores it in the memoization dictionary for future reference.
        /// </summary>
        /// <param name="Z">The input value for which Stern's Diatomic Series value is computed.</param>
        /// <returns>The Stern's Diatomic Series value for the given input.</returns>
        static long SternDiatomicSeriesFunc(long Z)
        {
            // If the value is already calculated, return it from memo
            if (dict.ContainsKey(Z))
                return dict[Z];

            // Calculate the value of k
            long k = Z / 2;

            // If Z is even, recursively compute and memoize the sum of two previous terms
            if (Z % 2 == 0)
                dict[Z] = SternDiatomicSeriesFunc(k) + SternDiatomicSeriesFunc(k - 1);

            // If Z is odd, recursively compute and memoize the term corresponding to k
            else
                dict[Z] = SternDiatomicSeriesFunc(k);

            // Return the computed value
            return dict[Z];
        }
                
        /// <summary>
        /// Computes Stern's Diatomic Series value for the given sum using memoization.
        /// </summary>
        /// <param name="sum">The input sum for which Stern's Diatomic Series value is computed.</param>
        /// <returns>The Stern's Diatomic Series value for the given sum.</returns>
        public static long Solve(long sum)
        {
            /*Base Case Initialization: In the Solve function, memo[0] = 1 initializes the memoization with the base 
             *case of the series, where Z = 0 corresponds to the value 1.
             *Initialize memoization with the base case */
             
             dict[0] = 1;

            // Compute and return Stern's diatomic series value for the given sum
            return SternDiatomicSeriesFunc(sum);
        }
    }


}
