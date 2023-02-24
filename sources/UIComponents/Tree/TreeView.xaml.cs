﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RevitDBExplorer.UIComponents.Tree.Items;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.UIComponents.Tree
{
    public partial class TreeView : UserControl
    {
        public TreeView()
        {
            InitializeComponent();
        }


        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.DataContext is TreeVM treeView) 
            {
                if (e.NewValue is TreeItem treeViewItemVM)
                {
                    treeView.RaiseSelectedItemChanged(treeViewItemVM);
                }
            }
        }


        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            var item = Mouse.DirectlyOver as FrameworkElement;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (item?.DataContext is TreeItem treeItem)
                {
                    string text = "???";

                    if (item?.DataContext is GroupTreeItem groupTreeItem)
                    {
                        text = groupTreeItem.Name;
                    }
                    if (item?.DataContext is SnoopableObjectTreeItem snoopableObjectTreeItem)
                    {
                        text = snoopableObjectTreeItem?.Object?.Name;
                    }


                    DataObject data = new DataObject();
                    data.SetData("Inputs", TreeVM.GetObjectsForTransfer(treeItem) ?? Enumerable.Empty<object>());
                    data.SetData(DataFormats.StringFormat, text ?? "");
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
                    //e.Handled = true;
                }
            }

            base.OnPreviewMouseMove(e);
        }
    }
}