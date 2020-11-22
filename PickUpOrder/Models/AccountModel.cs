// AccountModel - the partial class of Order, which contains all methods.
//                The object itself is defined in DatabaseModel.

using System;
using System.Security.Cryptography;
using System.Text;

namespace PickUpOrder.Models
{
    // OrderStatus - An enum used to reference the Type value.
    public enum AccountType
    {
        Customer = 1,
        Employee = 2,
        Manager = 3
    }

    public partial class Account
    {
        // CheckPassword - Return true if the hash of toCheck is equivalent
        //                 to the value of PasswordHash.
        public bool CheckPassword(string toCheck)
        {
            // Calculate the hash of toCheck.
            var shaInstance = SHA256.Create();
            var encoded = Encoding.UTF8.GetBytes(toCheck);
            var hashed = shaInstance.ComputeHash(encoded);
            var hashedStr = Convert.ToBase64String(hashed);

            System.Diagnostics.Debug.WriteLine(hashedStr);

            // Return whether this hash is the same.
            return PasswordHash.Equals(hashedStr);
        }
    }
}