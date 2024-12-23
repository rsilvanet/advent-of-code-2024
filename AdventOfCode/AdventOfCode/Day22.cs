public class Day22 : Day
{
    public override string Solve1() => Input.Sum(x => GetEvolutions(long.Parse(x)).Last()).ToString();

    public override string Solve2()
    {
        var ledger = new Dictionary<(int, int, int, int), int>();
        var control = new HashSet<(long Secret, (int, int, int, int) Sequence)>();

        foreach (var secretNumber in Input.Select(long.Parse))
        {
            var prices = GetEvolutions(secretNumber).Select(x => (int)(x % 10)).ToArray();
            var priceChanges = Enumerable.Range(1, prices.Length - 1).Select(x => prices[x] - prices[x - 1]).ToArray();

            for (var i = 3; i < priceChanges.Length; i++)
            {
                var ledgerKey = (priceChanges[i - 3], priceChanges[i - 2], priceChanges[i - 1], priceChanges[i]);

                if (control.Contains((secretNumber, ledgerKey)))
                {
                    continue;
                }

                control.Add((secretNumber, ledgerKey));
                ledger[ledgerKey] = ledger.ContainsKey(ledgerKey) ? ledger[ledgerKey] + prices[i + 1] : prices[i + 1];
            }
        }

        return ledger.Max(x => x.Value).ToString();
    }

    private IEnumerable<long> GetEvolutions(long secretNumber)
    {
        var newSecretNumber = 0L;

        yield return secretNumber;

        for (var i = 0; i < 2000; i++)
        {
            newSecretNumber = Evolve(secretNumber);
            secretNumber = newSecretNumber;
            yield return secretNumber;
        }
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

