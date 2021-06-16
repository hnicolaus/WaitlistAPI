using Domain.Exceptions;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using System;

namespace Domain.Services
{
    public class AdminService
    {
        private readonly int _adminLoginVerificationExpiryMinutes = 1;

        private readonly IAdminRepository _adminRepository;
        public ITextMessageClient _textMessageClient;

        public AdminService(IAdminRepository adminRepository, ITextMessageClient textMessageClient)
        {
            _adminRepository = adminRepository;
            _textMessageClient = textMessageClient;
        }

        public Admin GetAdmin(string userName, string password)
        {
            var admin = _adminRepository.GetAdmin(userName, password);
            if (admin == null)
            {
                throw new AdminNotFoundException();
            }

            return admin;
        }

        public void VerifyLoginVerification(Admin admin, string verificationCode)
        {
            if (admin.LoginVerification == null)
            {
                throw new InvalidRequestException("VerificationCode has not been requested.");
            }
            if (!admin.LoginVerification.IsValid(verificationCode))
            {
                throw new InvalidRequestException("VerificationCode code is invalid.");
            }
        }

        public void SendLoginVerificationSMS(Admin admin)
        {
            var message = "Front-desk login verification code, valid for " + _adminLoginVerificationExpiryMinutes + " minute(s): " + admin.LoginVerification.VerificationCode;
            _textMessageClient.SendTextMessage(admin.PhoneNumber, message);
        }

        public void UpdateAdminVerificationCode(Admin admin)
        {
            var verificationCode = Helper.GenerateRandomString(length: 8);
            admin.LoginVerification = new SMSVerification(verificationCode, DateTime.Now.AddMinutes(_adminLoginVerificationExpiryMinutes));
            _adminRepository.SaveChanges();
        }

        public void ResetLoginVerification(Admin admin)
        {
            admin.LoginVerification = null;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _adminRepository.SaveChanges();
        }
    }
}
