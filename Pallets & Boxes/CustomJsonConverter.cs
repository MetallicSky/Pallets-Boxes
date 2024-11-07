using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pallets___Boxes
{
    internal class CustomJsonConverter : Newtonsoft.Json.JsonConverter<Pallet>
    {
        public override void WriteJson(JsonWriter writer, Pallet pallet, Newtonsoft.Json.JsonSerializer serializer)
        {
            var jo = new JObject
        {
            { "Id", pallet.Id },
            { "Height", pallet.Height },
            { "Width", pallet.Width },
            { "Length", pallet.Length },
            { "PalletWeight", pallet.PalletWeight },
            { "Boxes", JArray.FromObject(pallet.Boxes, serializer) }  // Serialize _boxes list
            };
            jo.WriteTo(writer);
        }

        public override Pallet ReadJson(JsonReader reader, Type objectType, Pallet existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            // Update the static ID counter
            int Id = jo["Id"]?.Value<int>() ?? 0;
            Pallet._lastPalletId = Math.Max(Pallet._lastPalletId, lastPalletId);

            // Create the new Pallet instance
            var pallet = new Pallet();

            // Populate private fields
            pallet._height = jo["_height"]?.Value<double>() ?? 0;
            pallet._width = jo["_width"]?.Value<double>() ?? 0;
            pallet._length = jo["_length"]?.Value<double>() ?? 0;
            pallet._palletWeight = jo["_palletWeight"]?.Value<double>() ?? 0;

            // Deserialize `Boxes` into the private `_boxes` list
            var boxes = jo["Boxes"]?.ToObject<List<Box>>(serializer) ?? new List<Box>();
            foreach (var box in boxes)
            {
                pallet._boxes.Add(box);
            }

            // Deserialize ExpDate
            if (jo["ExpDate"] != null)
            {
                pallet.ExpDate = DateOnly.Parse(jo["ExpDate"]!.Value<string>());
            }

            return pallet;
        }
    }
}
