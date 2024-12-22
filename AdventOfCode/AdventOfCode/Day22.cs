public class Day22 : Day
{
    public override string Solve1()
    {
        var total = 0L;

        foreach (var item in Input.Select(long.Parse))
        {
            var secretNumber = item;
            var newSecretNumber = 0L;

            for (var i = 0; i < 2000; i++)
            {
                newSecretNumber = Evolve(secretNumber);
                secretNumber = newSecretNumber;
            }

            total += secretNumber;
        }

        return total.ToString();
    }

    public override string Solve2()
    {
        var ledger = new Dictionary<(long Secret, string Sequence), int>();

        foreach (var initial in Input.Select(long.Parse))
        {
            var secretNumber = initial;
            var newSecretNumber = 0L;
            var evolutions = new List<long>([secretNumber]);

            for (var i = 0; i < 2000; i++)
            {
                newSecretNumber = Evolve(secretNumber);
                secretNumber = newSecretNumber;
                evolutions.Add(secretNumber);
            }

            var prices = evolutions.Select(x => (int)char.GetNumericValue(x.ToString().Last())).ToArray();
            var priceChanges = new List<int>();

            for (int i = 1; i < prices.Length; i++)
            {
                priceChanges.Add(prices[i] - prices[i - 1]);
            }

            for (var i = 3; i < priceChanges.Count; i++)
            {
                var sequence = string.Join(",", priceChanges[(i - 3)..(i + 1)]);
                var ledgerKey = (Secret: initial, Sequence: sequence);

                if (ledger.ContainsKey(ledgerKey))
                {
                    continue;
                }

                ledger.Add(ledgerKey, prices[i + 1]);
            }
        }

        return ledger.GroupBy(x => x.Key.Sequence).Select(x => (Sequence: x.Key, Total: x.Sum(z => z.Value))).Max(x => x.Total).ToString();
    }

    private long Evolve(long secretNumber)
    {
        long newSecretNumber = secretNumber * 64L;
        newSecretNumber = secretNumber ^ newSecretNumber;
        newSecretNumber = newSecretNumber % 16777216;

        secretNumber = newSecretNumber;

        newSecretNumber = secretNumber / 32L;
        newSecretNumber = secretNumber ^ newSecretNumber;
        newSecretNumber = newSecretNumber % 16777216;

        secretNumber = newSecretNumber;

        newSecretNumber = secretNumber * 2048L;
        newSecretNumber = secretNumber ^ newSecretNumber;
        newSecretNumber = newSecretNumber % 16777216;

        return newSecretNumber;
    }
}

