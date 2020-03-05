using System;
using System.Text;

namespace Viato.Api.Tor
{
    public class TorToken
    {
        private const string _sep = ":";

        public TorToken(
            Guid id,
            long sourceOrgId,
            long destinationOrgId,
            decimal amount,
            string signature = null)
        {
            Id = id;
            SourceOrgId = sourceOrgId;
            DestinationOrgId = destinationOrgId;
            Amount = amount;
            Signature = signature;
        }

        public Guid Id { get; private set; }

        public long SourceOrgId { get; private set; }

        public long DestinationOrgId { get; private set; }

        public decimal Amount { get; private set; }

        public string Signature { get; private set; }

        public static TorToken Parse(string token)
        {
            token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var tokenArray = token.Split(_sep);
            if (tokenArray.Length != 5)
            {
                throw new FormatException("Token has invalid format: it should be devided to 4 pieces.");
            }

            return new TorToken(
                Guid.Parse(tokenArray[0]),
                long.Parse(tokenArray[1]),
                long.Parse(tokenArray[2]),
                decimal.Parse(tokenArray[3]),
                tokenArray[4]);
        }

        public bool Verify(byte[] pubKey)
        {
            var tokenPayload = GetTokenPayload();
            return ECKey.Verify(tokenPayload.GetBytes(), Signature.HexToByteArray(), pubKey);
        }

        // TODO: in the future it should be implemented in UI
        public string Protect(byte[] privKey)
        {
            var ecKey = ECKey.FromPrivateKey(privKey);
            var tokenPayload = GetTokenPayload();
            var signature = ecKey.Sign(tokenPayload.GetBytes());
            return Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{tokenPayload}{_sep}{signature.ToHex()}"));
        }

        private string GetTokenPayload()
        {
            return $"{Id}{_sep}{SourceOrgId}{_sep}{DestinationOrgId}{_sep}{Amount}";
        }
    }
}
