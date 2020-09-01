﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Code forked from Betsegaw Tadele's https://github.com/betsegaw/windowwalker/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Plugin.WindowWalker.Components
{
    /// <summary>
    /// Class housing fuzzy matching methods
    /// </summary>
    public static class FuzzyMatching
    {
        /// <summary>
        /// Finds the best match (the one with the most
        /// number of letters adjacent to each other) and
        /// returns the index location of each of the letters
        /// of the matches
        /// </summary>
        /// <param name="text">The text to search inside of</param>
        /// <param name="searchText">the text to search for</param>
        /// <returns>returns the index location of each of the letters of the matches</returns>
        public static List<int> FindBestFuzzyMatch(string text, string searchText)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            searchText = searchText.ToLower();
            text = text.ToLower();

            // Create a grid to march matches like
            // eg.
            //   a b c a d e c f g
            // a x     x
            // c     x       x
            bool[,] matches = new bool[text.Length, searchText.Length];
            for (int firstIndex = 0; firstIndex < text.Length; firstIndex++)
            {
                for (int secondIndex = 0; secondIndex < searchText.Length; secondIndex++)
                {
                    matches[firstIndex, secondIndex] =
                        searchText[secondIndex] == text[firstIndex] ?
                        true :
                        false;
                }
            }

            // use this table to get all the possible matches
            List<List<int>> allMatches = GetAllMatchIndexes(matches);

            // return the score that is the max
            int maxScore = allMatches.Count > 0 ? CalculateScoreForMatches(allMatches[0]) : 0;
            List<int> bestMatch = allMatches.Count > 0 ? allMatches[0] : new List<int>();

            foreach (var match in allMatches)
            {
                int score = CalculateScoreForMatches(match);
                if (score > maxScore)
                {
                    bestMatch = match;
                    maxScore = score;
                }
            }

            return bestMatch;
        }

        /// <summary>
        /// Gets all the possible matches to the search string with in the text
        /// </summary>
        /// <param name="matches"> a table showing the matches as generated by
        /// a two dimensional array with the first dimension the text and the second
        /// one the search string and each cell marked as an intersection between the two</param>
        /// <returns>a list of the possible combinations that match the search text</returns>
        public static List<List<int>> GetAllMatchIndexes(bool[,] matches)
        {
            List<List<int>> results = new List<List<int>>();

            for (int secondIndex = 0; secondIndex < matches.GetLength(1); secondIndex++)
            {
                for (int firstIndex = 0; firstIndex < matches.GetLength(0); firstIndex++)
                {
                    if (secondIndex == 0 && matches[firstIndex, secondIndex])
                    {
                        results.Add(new List<int> { firstIndex });
                    }
                    else if (matches[firstIndex, secondIndex])
                    {
                        var tempList = results.Where(x => x.Count == secondIndex && x[x.Count - 1] < firstIndex).Select(x => x.ToList()).ToList();

                        foreach (var pathSofar in tempList)
                        {
                            pathSofar.Add(firstIndex);
                        }

                        results.AddRange(tempList);
                    }
                }

                results = results.Where(x => x.Count == secondIndex + 1).ToList();
            }

            return results.Where(x => x.Count == matches.GetLength(1)).ToList();
        }

        /// <summary>
        /// Calculates the score for a string
        /// </summary>
        /// <param name="matches">the index of the matches</param>
        /// <returns>an integer representing the score</returns>
        public static int CalculateScoreForMatches(List<int> matches)
        {
            var score = 0;

            for (int currentIndex = 1; currentIndex < matches.Count; currentIndex++)
            {
                var previousIndex = currentIndex - 1;

                score -= matches[currentIndex] - matches[previousIndex];
            }

            return score == 0 ? -10000 : score;
        }
    }
}
