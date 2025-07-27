using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace BoxOfCoins
{
    public class BoxOfCoins
    {

        public static int Solve(int[] boxes)
        {
            int n = boxes.Length;
            int[,] dp = new int[n, n];

            // Initialize for the base case when only one box is considered
            for (int i = 0; i < n; i++)
            {
                dp[i, i] = boxes[i];
            }

            // Fill the dp table for increasing lengths of subarrays
            for (int length = 2; length <= n; length++)
            {
                for (int i = 0; i <= n - length; i++)
                {
                    int j = i + length - 1;
                    // Alex's turn: He can choose either boxes[i] or boxes[j]
                    // If he chooses boxes[i], then the remainder is dp[i+1, j] considering Cindy plays optimally from i+1 to j
                    // If he chooses boxes[j], then the remainder is dp[i, j-1] considering Cindy plays optimally from i to j-1
                    // Since it's Alex's turn, he subtracts Cindy's optimal result from his choice
                    dp[i, j] = Math.Max(boxes[i] - dp[i + 1, j], boxes[j] - dp[i, j - 1]);
                }
            }

            // dp[0, n-1] contains the difference between Alex's and Cindy's scores from the entire range of boxes
            return dp[0, n - 1];
        }        
    }
}