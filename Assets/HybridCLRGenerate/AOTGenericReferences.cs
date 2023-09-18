using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"LitJSON.dll",
		"System.Core.dll",
		"UnityEngine.AssetBundleModule.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<int>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,AnimConfig>
	// System.Collections.Generic.Dictionary.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,GrowableCfgItem>
	// System.Collections.Generic.Dictionary.Enumerator<object,ItemCfgItem>
	// System.Collections.Generic.Dictionary.Enumerator<object,NpcCfgItem>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,AnimConfig>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,GrowableCfgItem>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,ItemCfgItem>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,NpcCfgItem>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,AnimConfig>
	// System.Collections.Generic.Dictionary.KeyCollection<int,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,GrowableCfgItem>
	// System.Collections.Generic.Dictionary.KeyCollection<object,ItemCfgItem>
	// System.Collections.Generic.Dictionary.KeyCollection<object,NpcCfgItem>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,AnimConfig>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,GrowableCfgItem>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,ItemCfgItem>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,NpcCfgItem>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,AnimConfig>
	// System.Collections.Generic.Dictionary.ValueCollection<int,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,GrowableCfgItem>
	// System.Collections.Generic.Dictionary.ValueCollection<object,ItemCfgItem>
	// System.Collections.Generic.Dictionary.ValueCollection<object,NpcCfgItem>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,AnimConfig>
	// System.Collections.Generic.Dictionary<int,float>
	// System.Collections.Generic.Dictionary<object,GrowableCfgItem>
	// System.Collections.Generic.Dictionary<object,ItemCfgItem>
	// System.Collections.Generic.Dictionary<object,NpcCfgItem>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<AnimConfig>
	// System.Collections.Generic.EqualityComparer<GrowableCfgItem>
	// System.Collections.Generic.EqualityComparer<ItemCfgItem>
	// System.Collections.Generic.EqualityComparer<NpcCfgItem>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,AnimConfig>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,GrowableCfgItem>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,ItemCfgItem>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,NpcCfgItem>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<GrowableCfgItem>
	// System.Collections.Generic.IEnumerable<ItemCfgItem>
	// System.Collections.Generic.IEnumerable<NpcCfgItem>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,AnimConfig>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,GrowableCfgItem>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,ItemCfgItem>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,NpcCfgItem>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<GrowableCfgItem>
	// System.Collections.Generic.IEnumerator<ItemCfgItem>
	// System.Collections.Generic.IEnumerator<NpcCfgItem>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,AnimConfig>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,GrowableCfgItem>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,ItemCfgItem>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,NpcCfgItem>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,AnimConfig>
	// System.Collections.Generic.KeyValuePair<int,float>
	// System.Collections.Generic.KeyValuePair<object,GrowableCfgItem>
	// System.Collections.Generic.KeyValuePair<object,ItemCfgItem>
	// System.Collections.Generic.KeyValuePair<object,NpcCfgItem>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<AnimConfig>
	// System.Collections.Generic.ObjectEqualityComparer<GrowableCfgItem>
	// System.Collections.Generic.ObjectEqualityComparer<ItemCfgItem>
	// System.Collections.Generic.ObjectEqualityComparer<NpcCfgItem>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<object,byte>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// }}

	public void RefMethods()
	{
		// object LitJson.JsonMapper.ToObject<object>(string)
		// object System.Linq.Enumerable.FirstOrDefault<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// object System.Threading.Interlocked.CompareExchange<object>(object&,object,object)
		// object UnityEngine.AssetBundle.LoadAsset<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// bool UnityEngine.GameObject.TryGetComponent<object>(object&)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Resources.Load<object>(string)
	}
}