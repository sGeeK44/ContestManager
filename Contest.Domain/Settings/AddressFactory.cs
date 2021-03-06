﻿using System;
using System.ComponentModel.Composition;

namespace Contest.Domain.Settings
{
    [Export(typeof(IAddressFactory))]
    public class AddressFactory : IAddressFactory
    {
        /// <summary>
        ///     Create a new instance of <see cref="T:Contest.Domain.Settings.IAddress" /> with specified param
        /// </summary>
        /// <returns>IAddress's instance</returns>
        public IAddress Create(int streetNumber, string street, string zipCode, string city)
        {
            var result = new Address
            {
                StreetNumber = streetNumber,
                Street = street,
                ZipCode = zipCode,
                City = city
            };

            return result;
        }
    }
}