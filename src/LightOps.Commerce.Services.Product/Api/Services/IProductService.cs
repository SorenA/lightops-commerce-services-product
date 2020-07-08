﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Services.Product.Api.Models;

namespace LightOps.Commerce.Services.Product.Api.Services
{
    public interface IProductService
    {
        Task<IProduct> GetByIdAsync(string id);
        Task<IProduct> GetByHandleAsync(string handle);

        Task<IList<IProduct>> GetByCategoryIdAsync(string categoryId);
        Task<IList<IProduct>> GetBySearchAsync(string searchTerm);
    }
}