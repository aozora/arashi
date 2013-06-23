using System;
using System.Web.Script.Serialization;

namespace xVal.ServerSide
{
    // This class is designed for use with jQuery Validation's "remote" rule, which expects these kinds of JSON responses
    public class RemoteValidationResult
    {
        public string JsonResponse { get; private set; }

        private RemoteValidationResult(string jsonResponse)
        {
            JsonResponse = jsonResponse;
        }
        public override string ToString()
        {
            return JsonResponse;
        }

        public static RemoteValidationResult Success = new RemoteValidationResult("true");

        public static RemoteValidationResult Failure()
        {
            return Failure(null);
        }

        public static RemoteValidationResult Failure(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return new RemoteValidationResult("false"); // Signals failure without error message

            // Turn the arbitrary string into a safe JSON one
            return new RemoteValidationResult(new JavaScriptSerializer().Serialize(errorMessage));
        }
    }
}