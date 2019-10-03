using EppLib.Entities;
using System;

namespace EppLib.OpenProvider.ImplTests
{
    internal static class Ensure
    {
        internal static void Success(EppResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (response.Code.Equals("1000") || response.Code.Equals("1001") ||
                response.Code.Equals("1300") || response.Code.Equals("1301")
                )
                return;

            throw new Exception($"{response.Code} {response.Message}");
        }

        internal static void IsNotNullOrEmpty(string name, string serverTransactionId)
        {
            if (string.IsNullOrWhiteSpace(serverTransactionId))
                throw new Exception($"Value {name} cannot be null or empty");
        }

        internal static void ArgumentNotNull(string name, object action)
        {
            if (action == null)
                throw new ArgumentNullException(name);
        }
    }
}