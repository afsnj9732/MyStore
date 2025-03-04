﻿using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Dtos.Infos
{
    public class PaymentInfo
    {
        public IEnumerable<CartResultModel> CartItems { get; set; }
    }
}
