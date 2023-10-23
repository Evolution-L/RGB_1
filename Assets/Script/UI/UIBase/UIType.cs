/*
 *	
 *  Define View's Path And Name
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
	public class UIType {
        private string rootPath = @"prefab/ui.monofile/";
        public string Path { get; private set; }

        public string Name { get; private set; }

        public UIType(string path)
        {
            Path = $"{rootPath}{path}.prefab";
            Name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }

        public static readonly UIType MainView = new UIType("Main");
        public static readonly UIType LoginView = new UIType("Login");
    }
}
