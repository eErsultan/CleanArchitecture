using Newtonsoft.Json;

namespace Application.DTOs.Common
{
    public class ErrorDetails
    {
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
