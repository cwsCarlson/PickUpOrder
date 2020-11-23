// AccountModel - The partial class of Order, which contains all methods.
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
        // Default Constructor - This is here because Linq methods
        //                       will protest if it isn't.
        public Account()
        {

        }

        // Constructor - Add a user account with the given information.
        //               UserID will be handled when the user
        //               is added to the database.
        public Account(string email, string passwd, AccountType type)
        {
            Email = email;
            PasswordHash = CalculateHash(passwd);
            Type = (int) type;
        }

        // CalculateHash - Applies the hash calculation to input str.
        private string CalculateHash(string str)
        {
            var encoded = Encoding.UTF8.GetBytes(str);
            var hashed = SHA256.Create().ComputeHash(encoded);
            return Convert.ToBase64String(hashed);
        }

        // CheckPassword - Return true if the hash of toCheck is equivalent
        //                 to the value of PasswordHash.
        public bool CheckPassword(string toCheck)
        {
            return PasswordHash.Equals(CalculateHash(toCheck));
        }
    }
}