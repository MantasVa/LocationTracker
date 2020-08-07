using DataParser;
using DataParser.Handlers.Map;
using DataParser.Infrastructure.Visitor;
using DataParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace LocationTracker.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MarkersHandler _markersHandler;
        private IList<GpsData> _gpsData;
        public MainWindow()
        {
            InitializeComponent();
            var mapInitializer = new MapInitializer();
            _markersHandler = new MarkersHandler(mapInitializer);
            gmapHost.Child = mapInitializer.GetGMapControl();
        }


        private void DecodeButton_Click(object sender, RoutedEventArgs e)
        {
            var text = Regex.Replace(new TextRange(TextInput.Document.ContentStart, TextInput.Document.ContentEnd).Text, @"\t|\n|\r|\s", "");
            ExpandTreeViewButton.Visibility = Visibility.Visible;
            TextInput.Document.Blocks.Clear();
            TextInput.Document.Blocks.Add(new Paragraph(new Run(text.ToUpper())));

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Insert data.");
                return;
            }

            try
            {
                string transferType;
                if (tcpRadioButton.IsChecked != null && (bool)tcpRadioButton.IsChecked)
                {
                    transferType = "TCP";
                }
                else
                {
                    transferType = "UDP";
                }
                CompositeData data = new PacketDecoder().Decode(transferType, text);
                HandleGpsListView(data);
                HandleAvlTableDataGrid(data);

                treeView.ItemsSource = data.Data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Corrupted data inserted.");
            }
        }

        private void HandleGpsListView(CompositeData data)
        {
            var gpsDataVisitor = new GpsDataVisitor();
            data.Accept(gpsDataVisitor);
            _gpsData = gpsDataVisitor.GpsData;
            gpsElementsListView.ItemsSource = _gpsData;
            _markersHandler.LoadMarkers(_gpsData);
        }

        private void HandleAvlTableDataGrid(CompositeData data)
        {
            var listViewVisitor = new TransposedPacketDataVisitor();
            data.Accept(listViewVisitor);
            var dataTable = listViewVisitor.DataTable;
            AvlTableDataGrid.DataContext = dataTable.DefaultView;
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var data = e.NewValue as BaseData;

            if (data == null)
            {
                return;
            }

            if (treeView.SelectedItem as CompositeData != null)
            {
                var composite = treeView.SelectedItem as CompositeData;
                var timestamp = composite.Data.First();

                foreach (GpsData gpsData in gpsElementsListView.Items)
                {
                    if (gpsData.Timestamp == timestamp.Value)
                    {
                        gpsElementsListView.SelectedItem = gpsData;
                        _markersHandler.CenterMapToMarker(gpsData);
                    }
                }
            }
            else
            {
                gpsElementsListView.SelectedItem = null;
            }

            var text = new TextRange(TextInput.Document.ContentStart, TextInput.Document.ContentEnd);
            text.ClearAllProperties();

            var startPosition = data.ArraySegment.Offset * 2;
            var endPosition = startPosition + data.ArraySegment.Count * 2;

            var start = text.Start.GetPositionAtOffset(startPosition);
            var end = text.Start.GetPositionAtOffset(endPosition);

            var range = new TextRange(start, end);
            range.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);

        }

        private void GpsElementsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (GpsData item in e.AddedItems)
            {
                _markersHandler.UpdateSelectedMarker(item);
            }

        }
        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer1.ScrollToHorizontalOffset(e.HorizontalOffset);
        }


        private void ShowHideMapButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowHideMapButton.Content.ToString() == "Hide Map")
            {
                GridColumn2.Visibility = Visibility.Collapsed;
                mainGridColumn2.Width = new GridLength(0);
                GridColumn3.Visibility = Visibility.Collapsed;
                mainGridColumn3.Width = new GridLength(0);

                ShowHideMapButton.Content = "Show Map";
            }
            else if (ShowHideMapButton.Content.ToString() == "Show Map")
            {
                GridColumn2.Visibility = Visibility.Visible;
                mainGridColumn2.Width = new GridLength(5);
                GridColumn3.Visibility = Visibility.Visible;
                mainGridColumn3.Width = new GridLength(300, GridUnitType.Auto);
                ShowHideMapButton.Content = "Hide Map";
            }
        }

        private void ExpandTreeView_Click(object sender, RoutedEventArgs e)
        {
            if (ExpandTreeViewButton.Content.ToString() == "Expand")
            {
                ExpandTreeViewButton.Content = "Collapse";
                foreach (var item in treeView.Items)
                {
                    DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(item);
                    ((TreeViewItem)dObject).ExpandSubtree();
                }

            }
            else if (ExpandTreeViewButton.Content.ToString() == "Collapse")
            {
                ExpandTreeViewButton.Content = "Expand";
                foreach (var item in treeView.Items)
                {
                    DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(item);
                    CollapseTreeviewItems(((TreeViewItem)dObject));
                }
            }
        }

        private void CollapseTreeviewItems(TreeViewItem Item)
        {
            Item.IsExpanded = false;

            foreach (var item in Item.Items)
            {
                DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(item);

                if (dObject != null)
                {
                    ((TreeViewItem)dObject).IsExpanded = true;

                    if (((TreeViewItem)dObject).HasItems)
                    {
                        CollapseTreeviewItems(((TreeViewItem)dObject));
                    }
                }
            }
        }

        private void AvlTableDataGrid_Checked(object sender, RoutedEventArgs e)
        {
            if (ScrollViewer1 != null)
            {
                ScrollViewer1.Visibility = Visibility.Hidden;
            }

            if (treeView != null)
            {
                treeView.Visibility = Visibility.Hidden;
            }
            if (AvlTableDataGrid != null)
            {
                AvlTableDataGrid.Visibility = Visibility.Visible;
            }
        }


        private void TreeView_Checked(object sender, RoutedEventArgs e)
        {
            if (ScrollViewer1 != null)
            {
                ScrollViewer1.Visibility = Visibility.Visible;
            }

            if (treeView != null)
            {
                treeView.Visibility = Visibility.Visible;
            }
            if (AvlTableDataGrid != null)
            {
                AvlTableDataGrid.Visibility = Visibility.Hidden;
            }

        }
    }
}
