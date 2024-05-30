using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }

    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        float x = (float)obj["x"];
        float y = (float)obj["y"];
        float z = (float)obj["z"];
        return new Vector3(x, y, z);
    }
}

public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WritePropertyName("w");
        writer.WriteValue(value.w);
        writer.WriteEndObject();
    }

    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        float x = (float)obj["x"];
        float y = (float)obj["y"];
        float z = (float)obj["z"];
        float w = (float)obj["w"];
        return new Quaternion(x, y, z, w);
    }
}