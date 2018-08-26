using System;

namespace EIP.Common.Core.Ldap {
    internal class Convertor {
        static internal object ChangeType(object source, Type conversionType) {
            if (source != null) {
                switch (conversionType.Name.ToLower()) {
                    case "string[]":
                        object[] array = source as object[];
                        string[] result;
                        if (array != null) {
                            result = new string[array.Length];
                            for (int i = 0; i < array.Length; i++) {
                                result[i] = array[i].ToString();
                            }
                        } else {
                            result = new string[1];
                            result[0] = source.ToString();
                        }
                        return result;
                    case "guid":
                        byte[] array1 = source as byte[];
                        return new Guid(array1);

                }
            }
            return source;
        }
    }
}
