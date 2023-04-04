﻿using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;

namespace IB_projekat.Certificates.DTOS
{
    public class RequestDTO
    {
        public CertificateType CertificateType { get; set; }
        public string SignitureSerialNumber { get; set; }
        public int UserId { get; set; }
        public string Flags { get; set; }
    }
}
