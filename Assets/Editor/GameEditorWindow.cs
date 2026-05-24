using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GameEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/游戏配置")]
        private static void Open()
        {
            var window = GetWindow<GameEditorWindow>("游戏配置", true);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            
            
            tree.Add("判定系统", new JudgeTable());
                
                
            
            
            return tree;
        }

        public class JudgeTable
        {
        
            [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector, HideLabel]
            private readonly List<判定等级> 判定;
        
            public JudgeTable()
            {
                var list = AssetDatabase.FindAssets("t:判定等级")
                    .Select(guid => AssetDatabase.LoadAssetAtPath<判定等级>(AssetDatabase.GUIDToAssetPath(guid)))
                    .ToArray();
                    // .Sort(v => v.name);
                    判定 = list.ToList();
            }
        }

        protected override void OnBeginDrawEditors()
        {
            if (this.MenuTree == null) return;

            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}