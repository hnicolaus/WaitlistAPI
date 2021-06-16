using System;

namespace Domain.Models
{
    public class SMSVerification
    {
        public SMSVerification(string verificationCode, DateTime? expiryDateTime)
        {
            VerificationCode = verificationCode;
            ExpiryDateTime = expiryDateTime;
        }

        public string VerificationCode { get; set; }
        public DateTime? ExpiryDateTime { get; set; }
        public bool IsValid(string authenticationCode)
        {
            return authenticationCode == VerificationCode && DateTime.Now <= ExpiryDateTime;
        }
    }
}
