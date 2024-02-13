using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Attendance.API
{
    class ApiRequest
    {
        [DataMember(Name = "result", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "result")]
        public Object Result { get; set; }
        /// <summary>
        /// The name of the request/response to be processed.
        /// </summary>
        /// <value>The name of the request/response to be processed.</value>
        [DataMember(Name = "total", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "total")]
        public string total { get; set; }

        /// <summary>
        /// The version of the request/response.
        /// </summary>
        /// <value>The version of the request/response.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }



        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    [DataContract]
    public class ApiRequest<T>
    {

        /// <summary>
        /// The name of the request/response to be processed.
        /// </summary>
        /// <value>The name of the request/response to be processed.</value>
        [DataMember(Name = "total", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "total")]
        public string total { get; set; }

        /// <summary>
        /// The version of the request/response.
        /// </summary>
        /// <value>The version of the request/response.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }


        [DataMember(Name = "Result", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "Result")]
        public T Result { get; set; }


        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
