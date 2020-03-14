using System;
using System.Text;

namespace Viato.Api.Tor
{
    public class TorToken
    {
        private const string _sep = ":";

        public TorToken(
            Guid id,
            long pipelineId,
            decimal amount,
            string signature = null)
        {
            Id = id;
            PipelineId = pipelineId;
            Amount = amount;
            Signature = signature;
        }

        public Guid Id { get; private set; }

        public long PipelineId { get; private set; }

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
                decimal.Parse(tokenArray[2]),
                tokenArray[3]);
        }

        public static bool TryParse(string token, out TorToken torToken)
        {
            torToken = default;

            try
            {
                torToken = Parse(token);
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
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
            return $"{Id}{_sep}{PipelineId}{_sep}{Amount}";
        }
    }
}
