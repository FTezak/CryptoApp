using System;

namespace CryptoAPI.Tasks
{
    public class PercentageDifference
    {

        public decimal CalculatePercentageDifference(decimal oldValue, decimal newValue)
        {
            if (oldValue == 0) return 0;

            if (newValue > oldValue)
            {
                return Math.Round(((newValue - oldValue) / oldValue) * 100, 2);
            }

            return Math.Round(- (((oldValue - newValue) / oldValue) * 100), 2);
        }
    }
}