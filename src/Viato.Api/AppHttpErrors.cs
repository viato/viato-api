namespace Viato.Api
{
    public sealed class AppHttpErrors
    {
        public const int TorTokenIdAlreadyCreated = 211;

        public const int TorPipelineNotFound = 441;
        public const int TorPipelineIsNotAcitve = 442;
        public const int TorOrganizationNotVerified = 443;

        public const int User2FAAlreadyEnabled = 451;
    }
}
