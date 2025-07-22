using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KeyAuth
{
	// Token: 0x0200000D RID: 13
	public class json_wrapper
	{
		// Token: 0x06000084 RID: 132 RVA: 0x0000237F File Offset: 0x0000057F
		public static bool is_serializable(Type to_check)
		{
			return to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004614 File Offset: 0x00002814
		public json_wrapper(object obj_to_work_with)
		{
			this.current_object = obj_to_work_with;
			Type type = this.current_object.GetType();
			this.serializer = new DataContractJsonSerializer(type);
			bool flag = !json_wrapper.is_serializable(type);
			if (flag)
			{
				throw new Exception(string.Format("the object {0} isn't a serializable", this.current_object));
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000466C File Offset: 0x0000286C
		public object string_to_object(string json)
		{
			byte[] bytes = Encoding.Default.GetBytes(json);
			object result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = this.serializer.ReadObject(memoryStream);
			}
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000239D File Offset: 0x0000059D
		public T string_to_generic<T>(string json)
		{
			return (T)((object)this.string_to_object(json));
		}

		// Token: 0x04000039 RID: 57
		private DataContractJsonSerializer serializer;

		// Token: 0x0400003A RID: 58
		private object current_object;
	}
}
