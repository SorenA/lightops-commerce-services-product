﻿using LightOps.Commerce.Services.Product.Api.Models;

namespace LightOps.Commerce.Services.Product.Domain.Models
{
    public class Image : IImage
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public double FocalCenterTop { get; set; }
        public double FocalCenterLeft { get; set; }
    }
}