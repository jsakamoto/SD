﻿namespace SD.Shared.Helper
{
    public static class EnumHelper
    {
        public static TEnum[] GetArray<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>();
        }

        public static EnumObject[] GetObjArray<TEnum>(bool translate = true) where TEnum : struct, Enum
        {
            return GetList<TEnum>(translate).ToArray();
        }

        public static IEnumerable<EnumObject> GetList<TEnum>(bool translate = true) where TEnum : struct, Enum
        {
            foreach (var val in GetArray<TEnum>())
            {
                var attr = val.GetCustomAttribute(translate);

                yield return new EnumObject(Convert.ToInt32(val), val, attr?.Name, attr?.Description, attr?.Group);
            }
        }
    }

    public class EnumObject
    {
        public EnumObject(int Value, object ValueObject, string? Name, string? Description, string? Group)
        {
            this.Value = Value;
            this.ValueObject = ValueObject;
            this.Name = Name;
            this.Description = Description;
            this.Group = Group;
        }

        public int Value { get; set; }
        public object ValueObject { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
    }
}